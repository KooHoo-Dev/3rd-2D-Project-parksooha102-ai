using Jay;
using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    public class ModuleDataManager : SingletonBase<ModuleDataManager>
    {
        public List<ModuleInfo> EquippedModules = new List<ModuleInfo>();

        [Header("Base Module")]
        public BaseModuleData BaseModule;

        private void Start()
        {
            Equip(BaseModule);
        }

        public void Equip(ModuleInfo module)
        {
            if (EquippedModules.Contains(module) == false)
            {
                EquippedModules.Add(module);
            }
        }

        public void Unequip(ModuleInfo module)
        {
            EquippedModules.Remove(module);
        }
    }
}