using UnityEngine;

namespace _2DProject
{
    public class PenetrateModule : IBulletModule
    {
        private PenetrateModuleData data;
        private int maxPenetrateLimit = 3;

        private int currentPenetrateCount;

        public PenetrateModule(PenetrateModuleData data)
        {
            this.data = data;
            maxPenetrateLimit = data.MaxPenetrateLimit;
        }

        public void Bind(Bullet bullet)
        {
        }

        public void ResetState()
        {
            currentPenetrateCount = maxPenetrateLimit;
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

        public IBulletModule Clone()
        {
            PenetrateModule clone = new PenetrateModule(data);

            clone.currentPenetrateCount = currentPenetrateCount;

            return clone;
        }

    }
}