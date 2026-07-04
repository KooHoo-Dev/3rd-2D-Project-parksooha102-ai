using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    public static class BulletFactory
    {
        public static Bullet Create(BulletPool pool, Vector3 position, Vector3 direction, int damage)
        {
            Bullet bullet = pool.Get();

            foreach (ModuleInfo info in ModuleDataManager.Instance.EquippedModules)
            {
                IBulletModule module = info.CreateModule();
                if (module == null)
                {
                    Debug.LogWarning($"모듈 생성 실패: {info}");
                    continue;
                }

                module.ResetState();
                bullet.AddModule(module);
            }

            bullet.Initialize(position, direction, damage);
            return bullet;
        }

        public static Bullet CreateFromParent(Bullet parent, Vector3 position, Vector3 direction, int damage)
        {
            Bullet bullet = parent.BulletPool.Get();
            bullet.ClearModules();

            foreach (IBulletModule module in parent.Modules)
                bullet.AddModule(module.Clone());

            bullet.Initialize(position, direction, damage);
            return bullet;
        }

    }
}