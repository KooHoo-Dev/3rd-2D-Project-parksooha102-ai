using UnityEngine;

namespace _2DProject
{
    public interface IBulletModule
    {
        void Bind(Bullet bullet);
        void ResetState();

        void OnHitEnemy(RaycastHit2D hit);
        void OnHitWall(RaycastHit2D hit);
        bool CanSurviveEnemy(RaycastHit2D hit);
        bool CanSurviveWall(RaycastHit2D hit);

        void OnUpdate();
        IBulletModule Clone();
    }
}