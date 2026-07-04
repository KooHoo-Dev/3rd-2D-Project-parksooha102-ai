using UnityEngine;

namespace _2DProject
{
    public class RicochetModule : IBulletModule
    {
        private Bullet bullet;

        private RicochetModuleData data;
        private int maxRicochetCount;

        private int currentRicochetCount;
        private bool isRicocheted;

        public RicochetModule(RicochetModuleData data)
        {
            this.data = data;
            maxRicochetCount = data.MaxRicochetCount;
        }

        public void Bind(Bullet bullet)
        {
            this.bullet = bullet;
        }

        public void ResetState()
        {
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

        public IBulletModule Clone()
        {
            RicochetModule clone = new RicochetModule(data);

            clone.currentRicochetCount = currentRicochetCount;
            clone.isRicocheted = isRicocheted;

            return clone;
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