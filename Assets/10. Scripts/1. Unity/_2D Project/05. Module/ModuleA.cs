using UnityEngine;

namespace _2DProject
{
    public class ModuleA : IBulletModule
    {
        private Bullet bullet;

        private int maxRicochetCount = 3;
        private int currentRicochetCount;

        public void OnInit(Bullet bullet)
        {
            this.bullet = bullet;
            currentRicochetCount = maxRicochetCount;
        }

        public void OnUpdate()
        {
        }

        public bool OnHitEnemy(RaycastHit2D hit)
        {
            return false;
        }

        public bool OnHitWall(RaycastHit2D hit)
        {
            return ProcessRicochet(hit);
        }

        private bool ProcessRicochet(RaycastHit2D hit)
        {
            if (currentRicochetCount <= 0)
            {
                return false;
            }

            currentRicochetCount--;

            Vector3 reflected = Vector3.Reflect(bullet.transform.up, hit.normal);

            bullet.transform.position = hit.point + hit.normal * 0.01f;
            bullet.transform.up = reflected;

            return true;
        }
    }
}