using UnityEngine;

namespace _2DProject
{
    public interface IBulletModule
    {
        void OnInit(Bullet bullet);
        bool OnHitEnemy(RaycastHit2D hit);
        bool OnHitWall(RaycastHit2D hit);
        void OnUpdate();
    }
}