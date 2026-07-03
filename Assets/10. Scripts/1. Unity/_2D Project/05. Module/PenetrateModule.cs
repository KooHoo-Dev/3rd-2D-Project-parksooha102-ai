using UnityEngine;

namespace _2DProject
{
    public class PenetrateModule : IBulletModule
    {
        private int maxPenetrateCount = 3;
        private int currentPenetrateCount;

        public PenetrateModule(PenetrateModuleData data)
        {
            maxPenetrateCount = data.MaxPenetrateCount;
        }

        public void OnInit(Bullet bullet)
        {
            currentPenetrateCount = maxPenetrateCount;
        }

        public void OnHitEnemy(RaycastHit2D hit)
        {
        }

        public void OnHitWall(RaycastHit2D hit)
        {
        }

        public bool CanSurviveEnemy(RaycastHit2D hit)
        {
            if (currentPenetrateCount <= 0)
                return false;

            currentPenetrateCount--;
            return true;
        }

        public bool CanSurviveWall(RaycastHit2D hit)
        {
            return false;
        }

        public void OnUpdate()
        {
        }

    }
}