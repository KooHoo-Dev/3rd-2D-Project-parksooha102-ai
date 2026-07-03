using Jay;
using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    public class ModuleDataManager : SingletonBase<ModuleDataManager>
    {
        // 장착된 모듈 리스트 (실험실 씬의 Bullet이 참조할 데이터)
        public List<ModuleInfo> EquippedModules = new List<ModuleInfo>();

        // 싱글톤이 생성/초기화될 때 호출되는 훅
        protected override void OnInitialize()
        {
            Debug.Log("[ModuleDataManager] 데이터 매니저가 준비되었습니다.");
        }

        // 모듈 장착 메서드
        public void Equip(ModuleInfo module)
        {
            if (!EquippedModules.Contains(module))
            {
                EquippedModules.Add(module);
                Debug.Log($"{module.Description} 장착됨");
            }
        }

        // 모듈 해제 메서드
        public void Unequip(ModuleInfo module)
        {
            if (EquippedModules.Remove(module))
            {
                Debug.Log($"{module.Description} 해제됨");
            }
        }
    }
}