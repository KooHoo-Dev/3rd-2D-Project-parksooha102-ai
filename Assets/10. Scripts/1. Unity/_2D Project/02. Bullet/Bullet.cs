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
        private HashSet<Collider2D> hitSet = new HashSet<Collider2D>();
        private bool isInitialized;
        private float bulletAliveTime;
        private const float MAX_ALIVE_TIME = 5f;

        private enum HitType
        {
            None,
            Enemy,
            Wall
        }

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
            hitSet.Clear();

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
            HitType hitType = GetHitType(hit.collider);

            if (hitType == HitType.None)
            {
                hitSet.Clear();
                transform.position += transform.up * moveDistance;

                return;
            }

            transform.position = hit.point;
            ProcessHit(hit, hitType);

            float remaining = moveDistance - hit.distance;

            if (remaining > 0f)
                transform.position += transform.up * remaining;

            return;
        }

        private HitType GetHitType(Collider2D collider)
        {
            if (collider == null)
                return HitType.None;

            if (collider.CompareTag(OpponentTag))
                return HitType.Enemy;

            if (WallLayer.Contains(collider.gameObject.layer))
                return HitType.Wall;

            return HitType.None;
        }

        private void ProcessHit(RaycastHit2D hit, HitType hitType)
        {
            bool canSurvive = false;

            switch (hitType)
            {
                case HitType.Enemy:

                    if (TryRegisterHit(hit.collider) == false)
                        return;

                    for(int i = 0; i < modules.Count; i++)
                    {
                        modules[i].OnHitEnemy(hit);

                        if (!gameObject.activeSelf)
                            return;

                        if (modules[i].CanSurviveEnemy(hit))
                            canSurvive = true;
                    }

                    ApplyDamageTo(hit);

                    break;

                case HitType.Wall:

                    for(int i = 0; i < modules.Count; i++)
                    {
                        modules[i].OnHitWall(hit);

                        if (!gameObject.activeSelf)
                            return;

                        if (modules[i].CanSurviveWall(hit))
                            canSurvive = true;
                    }
                    break;
            }

            if (canSurvive == false)
                Kill();
        }

        private void ApplyDamageTo(RaycastHit2D hit)
        {
            HurtBox hurtBox = CombatSystem.Instance.GetHurtBoxOrNull(hit.collider);

            if (hurtBox != null)
            {
                CombatEvent @event;

                @event.EventType = CombatEventType.DamageEvent;
                @event.Amount = Damage;
                @event.Position = hit.collider.ClosestPoint(transform.position);

                CombatSystem.Instance.To(Owner, hurtBox.Owner, @event);
            }
        }

        private bool TryRegisterHit(Collider2D collider)
        {
            return hitSet.Add(collider);
        }

        public void Kill()
        {
            isInitialized = false;
            gameObject.SetActive(false);
        }

        public void AddModule(IBulletModule module)
        {
            if (modules.Contains(module) == false)
            {
                modules.Add(module);
            }
        }

        public void ClearModules()
        {
            modules.Clear();
        }

        public void AddHitSet(Collider2D collider)
        {
            hitSet.Add(collider);
        }

        private static int activeBulletCount;

        private void OnEnable()
        {
            activeBulletCount++;
        }

        private void OnDisable()
        {
            activeBulletCount--;
        }

        private void Update()
        {
                Debug.LogWarning(activeBulletCount);
        }

    }
}