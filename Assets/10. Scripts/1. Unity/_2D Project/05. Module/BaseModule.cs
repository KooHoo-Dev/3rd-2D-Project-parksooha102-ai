using UnityEngine;

namespace _2DProject
{
    public class BaseModule : IBulletModule
    {
        private BaseModuleData data;

        public BaseModule(BaseModuleData data)
        {
            this.data = data;
        }

        public void Bind(Bullet bullet) { }
        public void ResetState() { }
        public void OnHitEnemy(RaycastHit2D hit) { }
        public void OnHitWall(RaycastHit2D hit) { }
        public bool CanSurviveEnemy(RaycastHit2D hit) => false;
        public bool CanSurviveWall(RaycastHit2D hit) => false;
        public void OnUpdate() { }

        public IBulletModule Clone()
        {
            return new BaseModule(data);
        }
    }
}