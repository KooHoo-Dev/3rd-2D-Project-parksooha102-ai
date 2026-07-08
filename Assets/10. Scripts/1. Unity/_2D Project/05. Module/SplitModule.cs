using UnityEngine;

namespace _2DProject
{
    public class SplitModule : IBulletModule
    {
        private Bullet bullet;

        private SplitModuleData data;
        private int maxGeneration = 3;
        private int splitAmount = 3;
        private float splitAngle = 45.0f;
        private float multiplier = 0.5f;

        private int currentGeneration = 0;
        private Collider2D excludedTarget;

        public SplitModule(SplitModuleData data)
        {
            this.data = data;
            maxGeneration = data.MaxGeneration;
            splitAmount = data.SplitAmount;
            splitAngle = data.SplitAngle;
            multiplier = data.Multiplier;
        }

        public void Bind(Bullet bullet)
        {
            this.bullet = bullet;
        }

        public void ResetState()
        {
            currentGeneration = 0;
            excludedTarget = null;
        }

        public void OnHitEnemy(RaycastHit2D hit)
        {
            ProcessSplit(hit);
        }

        public void OnHitWall(RaycastHit2D hit)
        {
            ProcessSplit(hit);
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
            SplitModule clone = new SplitModule(data);

            clone.currentGeneration = currentGeneration;
            clone.excludedTarget = excludedTarget;

            return clone;
        }

        private void ProcessSplit(RaycastHit2D hit)
        {
            if (hit.collider == excludedTarget)
                return;

            if (currentGeneration >= maxGeneration)
                return;

            excludedTarget = hit.collider;
            currentGeneration++;

            float startAngle = -((splitAmount - 1) * splitAngle) * 0.5f;

            for (int i = 0; i < splitAmount; i++)
            {
                float angleOffset = startAngle + (i * splitAngle);
                Quaternion rotation = Quaternion.Euler(0, 0, angleOffset);
                Vector3 newDirection = rotation * bullet.transform.up;
                int newDamage = Mathf.RoundToInt(bullet.Damage * multiplier);

                Bullet subBullet = BulletFactory.CreateFromParent(bullet, bullet.transform.position, newDirection, newDamage);
                //subBullet.AddHitSet(hit.collider);
            }

            bullet.Kill();
        }

    }
}