using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace _2DProject
{
    public class Bullet : MonoBehaviour
    {
        [field: Header("Settings")]
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public string OpponentTag { get; private set; }
        [field: SerializeField] public string AllyTag { get; private set; }
        [field: SerializeField] public LayerMask WallLayer { get; private set; }

        [field: Header("References")]
        [field: SerializeField] public BulletPool BulletPool { get; private set; }
        [field: SerializeField] public CombatEntity Owner { get; private set; }

        public int Damage { get; private set; }
        public IReadOnlyList<IBulletModule> Modules => modules;

        private readonly List<IBulletModule> modules = new List<IBulletModule>();
        private readonly Dictionary<Collider2D, float> targetIgnoreTimers = new Dictionary<Collider2D, float>();
        private HashSet<Collider2D> hitSet = new HashSet<Collider2D>();

        private bool isInitialized;
        private float bulletAliveTime;
        private const float MAX_ALIVE_TIME = 5f;
        private const float HIT_COOLDOWN = 0.2f;

        private void FixedUpdate()
        {
            if (!isInitialized) return;

            bulletAliveTime += Time.fixedDeltaTime;

            if (bulletAliveTime >= MAX_ALIVE_TIME)
                Kill();

            for (int i = 0; i < modules.Count; i++)
                modules[i].OnUpdate();

            MoveBullet();
        }

        public void Initialize(Vector3 spawnPosition, Vector3 direction, int damage)
        {
            transform.position = spawnPosition;
            transform.up = direction;
            Damage = damage;

            bulletAliveTime = 0f;
            targetIgnoreTimers.Clear();

            for (int i = 0; i < modules.Count; i++)
            {
                modules[i].Bind(this);
            }

            gameObject.SetActive(true);
            isInitialized = true;
        }

        private void MoveBullet()
        {
            float moveDistance = Speed * Time.fixedDeltaTime;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, moveDistance);

            if (hit.collider != null && hit.collider.CompareTag(AllyTag) == false)
            {
                transform.position = hit.point;
                ProcessHit(hit);

                if (gameObject.activeSelf == false)
                    return;

                float remaining = moveDistance - hit.distance;

                if (remaining > 0f)
                    transform.position += transform.up * remaining;
                return;
            }

            transform.position += transform.up * moveDistance;
        }

        private void ProcessHit(RaycastHit2D hit)
        {
            GameObject target = hit.collider.gameObject;

            bool isEnemy = target.CompareTag(OpponentTag);
            bool isWall = ((1 << target.layer) & WallLayer.value) != 0;

            // 대상이 적도 아니고 벽도 아니면 처리 안 함
            if (!isEnemy && !isWall) return;

            if (isEnemy && !CanHit(hit.collider)) return;

            // 2. 모듈들에게 생존 여부와 로직 수행을 위임
            bool canSurvive = false;

            if (isEnemy)
            {
                // 적 타격 로직 수행
                ExecuteEnemyHitLogic(hit);

                foreach (var module in modules)
                {
                    // 관통 모듈은 여기서 true를 반환하여 canSurvive를 true로 만들 것임
                    if (module.CanSurviveEnemy(hit)) canSurvive = true;
                }
            }
            else if (isWall)
            {
                foreach (var module in modules)
                {
                    module.OnHitWall(hit);
                    if (module.CanSurviveWall(hit)) canSurvive = true;
                }
            }

            // 3. 모듈 중 단 하나라도 생존을 원하면 Kill 하지 않음
            if (!canSurvive)
            {
                Kill();
            }
        }

        private void ExecuteEnemyHitLogic(RaycastHit2D hit)
        {
            // 모듈 로직 수행
            foreach (var module in modules)
            {
                module.OnHitEnemy(hit);
            }

            // 데미지 계산 및 전달
            HurtBox hurtBox = CombatSystem.Instance.GetHurtBoxOrNull(hit.collider);
            if (hurtBox != null)
            {
                CombatEvent @event = new CombatEvent
                {
                    EventType = CombatEventType.DamageEvent,
                    Amount = Damage,
                    Position = hit.collider.ClosestPoint(transform.position)
                };
                CombatSystem.Instance.To(Owner, hurtBox.Owner, @event);
            }
        }

        private bool CanHit(Collider2D collider)
        {
            // 이미 이번 통과(Pass) 과정에서 맞춘 적이라면 무시
            if (hitSet.Contains(collider)) return false;

            // 처음 맞춘 적이라면 기록에 추가
            hitSet.Add(collider);
            return true;
        }

        public void Kill()
        {
            isInitialized = false;
            gameObject.SetActive(false);
        }

        public void AddModule(IBulletModule module)
        {
            if (!modules.Contains(module))
            {
                modules.Add(module);
            }
        }

        public void ClearModules()
        {
            modules.Clear();
        }
    }
}