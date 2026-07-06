using UnityEngine;

namespace _2DProject
{
    public static class BulletFactory
    {
        public static Bullet Create(BulletPool pool, Vector3 position, Vector3 direction)
        {
            Bullet bullet = pool.Get();

            BaseModuleData baseData = ModuleDataManager.Instance.BaseModule;
            int damage = baseData.Damage;
            float speed = baseData.BulletSpeed;

            for (int i = 0; i < ModuleDataManager.Instance.EquippedModules.Count; i++)
            {
                ModuleInfo info = ModuleDataManager.Instance.EquippedModules[i];
                IBulletModule module = info.CreateModule();
                if (module == null)
                    continue;

                module.ResetState();
                bullet.AddModule(module);
            }

            bullet.Initialize(position, direction, damage, speed);
            return bullet;
        }

        public static Bullet CreateFromParent(Bullet parent, Vector3 position, Vector3 direction, int damage)
        {
            Bullet bullet = parent.BulletPool.Get();

            for (int i = 0; i < parent.Modules.Count; i++)
            {
                IBulletModule module = parent.Modules[i];
                bullet.AddModule(module.Clone());
            }

            bullet.Initialize(position, direction, damage, parent.Speed);

            return bullet;
        }
    }
}