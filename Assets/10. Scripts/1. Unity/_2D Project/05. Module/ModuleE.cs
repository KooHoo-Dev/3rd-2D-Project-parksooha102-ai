using UnityEngine;

namespace _2DProject
{
    public class ModuleE : IBulletModule
    {
        private Bullet bullet;
        private int maxGenaration = 3;
        private int splitAmount = 3;
        private float splitAngle = 45.0f;
        private float multiplier = 0.5f;

        private int currentGeneration = 0;

        private static int nextGeneration;
        private static bool isTransferring;

        public void OnInit(Bullet bullet)
        {
            this.bullet = bullet;

            if (isTransferring)
            {
                currentGeneration = nextGeneration;
            }
            else
            {
                currentGeneration = 0;
            }
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

        private bool ProcessSplit(RaycastHit2D hit)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject == null) 
                return false;

            if (currentGeneration < maxGenaration)
            {
                isTransferring = true;
                nextGeneration = currentGeneration + 1;

                float startAngle = -((splitAmount - 1) * splitAngle) * 0.5f;
                for (int i = 0; i < splitAmount; i++)
                {
                    float angleOffset = startAngle + (i * splitAngle);
                    Quaternion rotation = Quaternion.Euler(0, 0, angleOffset);
                    Vector3 newDirection = rotation * bullet.transform.up;

                    Bullet subBullet = bullet.BulletPool.Get();

                    subBullet.Set(bullet.transform.position, newDirection, Mathf.RoundToInt(bullet.damage * multiplier), hit.collider);
                }

                isTransferring = false;

                bullet.Kill();
            }

            return false;
        }

    }
}