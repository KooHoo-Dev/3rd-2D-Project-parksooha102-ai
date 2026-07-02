using UnityEngine;

namespace _2DProject
{
    public class ModuleC : IBulletModule
    {
        Bullet bullet;
        private float explosionRadius = 5f;
        private float explosionMultiplier = 0.5f;
        private ExplosionPool explosionPool = new ExplosionPool();

        public void OnInit(Bullet bullet)
        {
            this.bullet = bullet;
        }

        public void OnHitEnemy(RaycastHit2D hit)
        {
            ProcessExplode();
        }

        public void OnHitWall(RaycastHit2D hit)
        {
            ProcessExplode();
        }

        public bool CanSurviveEnemy(RaycastHit2D hit)
        {
            return false;
        }

        public bool CanSurviveWall(RaycastHit2D hit)
        {
            return false;
        }

        public void OnUpdate()
        {
        }

        private void ProcessExplode()
        {
            Vector3 center = bullet.transform.position;
            Collider2D[] targets = Physics2D.OverlapCircleAll(center, explosionRadius);

            ExplosionEffect explosionEffect = explosionPool.Get();

            if (explosionEffect != null)
            {
                explosionEffect.SetExplosionRadius(explosionRadius);
                explosionEffect.Set(center);
            }

            foreach (Collider2D target in targets)
            {
                Vector2 direction = (target.transform.position - center).normalized;
                float distance = Vector2.Distance(center, target.transform.position);

                bool blocked = Physics2D.Linecast(center, target.transform.position, bullet.WallLayer);

                if (blocked)
                    continue;

                HurtBox hurtBox = CombatSystem.Instance.GetHurtBoxOrNull(target);
                
                if (hurtBox == null)
                {
                    Debug.LogWarning("HurtBox not found for collider: " + target.name);
                    continue;
                }

                CombatEntity sender = bullet.Owner;
                CombatEntity receiver = hurtBox.Owner;

                CombatEvent @event;
                @event.EventType = CombatEventType.DamageEvent;
                @event.Amount = Mathf.RoundToInt(bullet.damage * explosionMultiplier);
                @event.Position = target.ClosestPoint(sender.transform.position);

                CombatSystem.Instance.To(sender, receiver, @event);
            }
        }

    }
}