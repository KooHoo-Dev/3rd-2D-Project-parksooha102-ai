using UnityEngine;

namespace _2DProject
{
    public class RicochetModule : IBulletModule
    {
        private Bullet bullet;

        private int maxRicochetCount;
        private int currentRicochetCount;
        private bool isRicocheted;

        public RicochetModule(RicochetModuleData data)
        {
            maxRicochetCount = data.MaxRicochetCount;
        }

        public void OnInit(Bullet bullet)
        {
            this.bullet = bullet;
            currentRicochetCount = maxRicochetCount;
            isRicocheted = false;
        }

        public void OnHitEnemy(RaycastHit2D hit)
        {
        }

        public void OnHitWall(RaycastHit2D hit)
        {
            ProcessRicochet(hit);
        }

        public bool CanSurviveEnemy(RaycastHit2D hit)
        {
            return false;
        }

        public bool CanSurviveWall(RaycastHit2D hit)
        {
            return isRicocheted;
        }

        public void OnUpdate()
        {
        }

        private void ProcessRicochet(RaycastHit2D hit)
        {
            if (currentRicochetCount <= 0)
            {
                isRicocheted = false;
                return;
            }

            currentRicochetCount--;
            isRicocheted = true;

            Vector3 reflected = Vector3.Reflect(bullet.transform.up, hit.normal);
            reflected = reflected.normalized;

            bullet.transform.position = hit.point + hit.normal * 0.01f;

            bullet.transform.up = reflected;
        }

    }
}