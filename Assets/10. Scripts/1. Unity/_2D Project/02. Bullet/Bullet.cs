using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    public class Bullet : MonoBehaviour
    {
        [field: SerializeField] public float speed { get; private set; }
        [field: SerializeField] public string OpponentTag { get; private set; }
        [field: SerializeField] public string AllyTag { get; private set; }
        [field: SerializeField] public LayerMask WallLayer { get; private set; }
        [field: SerializeField] public BulletPool BulletPool { get; private set; }
        [field: SerializeField] public CombatEntity Owner { get; private set; }

        private bool isFired;
        public int damage { get; private set; }

        private float bulletAliveTime;
        private const float BULLET_DEATH_TIME = 5f;
        private const float COOLDOWN = 0.2f;

        private List<IBulletModule> modules = new List<IBulletModule>();
        private readonly Dictionary<Collider2D, float> targetIgnoreTimers = new Dictionary<Collider2D, float>();

        public void Set(Vector3 spawnPosition, Vector3 direction, int damage, Collider2D inheritedHitObject = null)
        {
            gameObject.SetActive(true);

            transform.position = spawnPosition;
            transform.up = direction;

            this.damage = damage;

            isFired = true;

            bulletAliveTime = 0f;

            targetIgnoreTimers.Clear();

            if (inheritedHitObject != null)
            {
                targetIgnoreTimers.Add(inheritedHitObject, bulletAliveTime + 100f);
            }

            foreach (IBulletModule m in modules)
            {
                m.OnInit(this);
            }
        }

        private void Awake()
        {
            AddModule(new ModuleA());
            //AddModule(new ModuleB());
            //AddModule(new ModuleC());
            AddModule(new ModuleD());
            AddModule(new ModuleE());
            //AddModule(new ModuleF());
        }

        private void FixedUpdate()
        {
            if (isFired == false)
                return;

            bulletAliveTime += Time.fixedDeltaTime;

            if (bulletAliveTime >= BULLET_DEATH_TIME)
            {
                Kill();
                return;
            }

            for (int i = 0; i < modules.Count; i++)
            {
                modules[i].OnUpdate();
            }

            MoveBullet();
        }

        private void MoveBullet()
        {
            float moveDistance = speed * Time.fixedDeltaTime;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, moveDistance);

            if (hit.collider != null && hit.collider.CompareTag(AllyTag) == false)
            {
                transform.position = hit.point;

                ProcessHit(hit);

                if (!gameObject.activeSelf)
                    return;
            }

            transform.position += transform.up * moveDistance;
        }

        private void ProcessHit(RaycastHit2D hit)
        {
            GameObject target = hit.collider.gameObject;
            bool canSurvive = false;

            if (target.CompareTag(OpponentTag))
            {
                if (CanHit(hit.collider) == false)
                    return;

                ProcessEnemyHit(hit);

                foreach (IBulletModule m in modules)
                {
                    if (m.CanSurviveEnemy(hit)) canSurvive = true;
                }
            }
            else if (WallLayer.Contains(target))
            {
                ProcessWallHit(hit);

                foreach (IBulletModule m in modules)
                {
                    if (m.CanSurviveWall(hit)) canSurvive = true;
                }
            }
            else
            {
                return;
            }

            if (canSurvive == false)
            {
                Kill();
            }
        }

        private bool CanHit(Collider2D collider)
        {
            if (targetIgnoreTimers.TryGetValue(collider, out float ignoreUntilTime))
            {
                if (bulletAliveTime < ignoreUntilTime)
                {
                    return false;
                }
            }

            targetIgnoreTimers[collider] = bulletAliveTime + COOLDOWN;
            return true;
        }

        private void ProcessEnemyHit(RaycastHit2D hit)
        {
            foreach (IBulletModule m in modules)
                m.OnHitEnemy(hit);

            HurtBox hurtBox = CombatSystem.Instance.GetHurtBoxOrNull(hit.collider);

            if (hurtBox == null)
            {
                Debug.LogWarning($"HurtBox not found for collider: {hit.collider.name}");
            }
            else
            {
                CombatEntity sender = Owner;
                CombatEntity receiver = hurtBox.Owner;

                CombatEvent @event;
                @event.EventType = CombatEventType.DamageEvent;
                @event.Amount = damage;
                @event.Position = hit.collider.ClosestPoint(sender.transform.position);

                CombatSystem.Instance.To(sender, receiver, @event);
            }
        }

        private void ProcessWallHit(RaycastHit2D hit)
        {
            foreach (IBulletModule m in modules)
                m.OnHitWall(hit);
        }

        public void Kill()
        {
            isFired = false;
            gameObject.SetActive(false);
        }

        public void AddModule(IBulletModule module)
        {
            if (modules.Contains(module) == false)
            {
                modules.Add(module);
            }
        }
    }
}