using UnityEngine;

namespace _2DProject
{
    public class PenetrateModule : IBulletModule
    {
        private PenetrateModuleData data;
        private int maxPenetrateCount = 3;

        private int currentPenetrateCount;

        public PenetrateModule(PenetrateModuleData data)
        {
            this.data = data;
            maxPenetrateCount = data.MaxPenetrateCount;
        }

        public void Bind(Bullet bullet)
        {
        }

        public void ResetState()
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
            Debug.Log($"관통! {currentPenetrateCount} 남음");
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