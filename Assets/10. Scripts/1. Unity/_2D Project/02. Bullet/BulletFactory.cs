using UnityEngine;

namespace _2DProject
{
    public static class BulletFactory
    {
        public static Bullet Create(BulletPool pool, Vector3 position, Vector3 direction, int damage)
        {
            Bullet bullet = pool.Get();

            for (int i = 0; i < ModuleDataManager.Instance.EquippedModules.Count; i++)
            {
                ModuleInfo info = ModuleDataManager.Instance.EquippedModules[i];
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

            for (int i = 0; i < parent.Modules.Count; i++)
            {
                IBulletModule module = parent.Modules[i];
                bullet.AddModule(module.Clone());
            }

            bullet.Initialize(position, direction, damage);
            return bullet;
        }

    }
}