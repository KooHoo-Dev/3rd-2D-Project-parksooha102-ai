using UnityEngine;

namespace _2DProject
{
    public class ExplosionModule : IBulletModule
    {
        Bullet bullet;

        private ExplosionModuleData data;
        private float explosionRadius = 5f;
        private int explosionDamage = 10;

        private ExplosionPool explosionPool = new ExplosionPool();

        public ExplosionModule(ExplosionModuleData data)
        {
            this.data = data;
            explosionRadius = data.ExplosionRadius;
            explosionDamage = data.ExplosionDamage;
        }

        public void Bind(Bullet bullet)
        {
            this.bullet = bullet;
        }

        public void ResetState()
        {
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

        public IBulletModule Clone()
        {
            ExplosionModule clone = new ExplosionModule(data);
            return clone;
        }

        private void ProcessExplode()
        {
            Vector3 center = bullet.transform.position - bullet.transform.up * 0.05f;
            Collider2D[] targets = Physics2D.OverlapCircleAll(center, explosionRadius);

            ExplosionEffect explosionEffect = explosionPool.Get();

            if (explosionEffect != null)
            {
                explosionEffect.SetExplosionRadius(explosionRadius);
                explosionEffect.Set(center);
            }

            for (int i = 0; i < targets.Length; i++)
            {
                Collider2D target = targets[i];
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
                @event.Amount = explosionDamage;
                @event.Position = target.ClosestPoint(sender.transform.position);

                CombatSystem.Instance.To(sender, receiver, @event);
            }
        }

    }
}