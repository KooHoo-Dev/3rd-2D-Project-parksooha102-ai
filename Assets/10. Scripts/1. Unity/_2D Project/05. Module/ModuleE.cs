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

        public void OnInit(Bullet bullet)
        {
            this.bullet = bullet;
        }

        public void OnUpdate()
        {
        }

        public bool OnHitEnemy(RaycastHit2D hit)
        {
            return ProcessSplit();
        }

        public bool OnHitWall(RaycastHit2D hit)
        {
            return ProcessSplit();
        }

        private bool ProcessSplit()
        {
            if (bullet.genaration < maxGenaration)
            {
                float startAngle = -((splitAmount - 1) * splitAngle) * 0.5f;

                for (int i = 0; i < splitAmount; i++)
                {
                    Bullet newBullet = bullet.BulletPool.Get();

                    float currentAngle = startAngle + (i * splitAngle);
                    Vector3 newDir = Quaternion.Euler(0, 0, currentAngle) * bullet.transform.up;
                    newBullet.Set(bullet.transform.position, newDir, Mathf.RoundToInt(bullet.damage * multiplier), bullet.genaration + 1);
                }
            }

            return false;
        }

    }
}