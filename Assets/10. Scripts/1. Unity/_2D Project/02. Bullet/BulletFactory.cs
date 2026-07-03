using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    public static class BulletFactory
    {
        public static Bullet Create(BulletPool pool,Vector3 position,Vector3 direction,int damage,Collider2D inheritedHitObject = null)
        {
            Bullet bullet = pool.Get();

            // 장착된 모듈 생성
            foreach (ModuleInfo info in ModuleDataManager.Instance.EquippedModules)
            {
                IBulletModule module = info.CreateModule();

                if (module != null)
                    bullet.AddModule(module);
            }

            // 총알 발사
            bullet.Set(position, direction, damage,inheritedHitObject);

            return bullet;
        }
    }
}