using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    /// <summary>
    /// ModuleInfo(에셋 또는 런타임 인스턴스)별로 IBulletModule을 재사용하는 풀.
    /// 발사할 때마다 new 하지 않고, 다 쓴 모듈을 반납받아 재사용한다.
    /// </summary>
    public static class BulletModulePool
    {
        private static readonly Dictionary<ModuleInfo, Stack<IBulletModule>> pools
            = new Dictionary<ModuleInfo, Stack<IBulletModule>>();

        /// <summary>
        /// info에 대응하는 모듈을 하나 빌려온다. 재사용 가능한 게 있으면 그걸 쓰고,
        /// 없으면 새로 만든다. 반환 전 ResetState()를 호출해 상태를 초기화한다.
        /// </summary>
        public static IBulletModule Rent(ModuleInfo info)
        {
            Stack<IBulletModule> stack = GetOrCreateStack(info);

            bool isNew = stack.Count == 0;
            IBulletModule module = isNew ? info.CreateModule() : stack.Pop();
            Debug.Log(isNew ? "새로 생성" : "풀에서 재사용"); // 실제로 재사용되는지 확인

            if (module == null)
            {
                UnityEngine.Debug.LogWarning($"모듈 생성 실패: {info}");
                return null;
            }

            module.ResetState();
            return module;
        }

        /// <summary>
        /// 다 쓴 모듈을 풀에 반납한다. 반드시 Rent할 때 쓴 info와 짝을 맞춰서 호출해야 한다.
        /// </summary>
        public static void Return(ModuleInfo info, IBulletModule module)
        {
            if (info == null || module == null)
                return;

            Stack<IBulletModule> stack = GetOrCreateStack(info);
            stack.Push(module);
        }

        private static Stack<IBulletModule> GetOrCreateStack(ModuleInfo info)
        {
            if (!pools.TryGetValue(info, out Stack<IBulletModule> stack))
            {
                stack = new Stack<IBulletModule>();
                pools[info] = stack;
            }

            return stack;
        }
    }
}