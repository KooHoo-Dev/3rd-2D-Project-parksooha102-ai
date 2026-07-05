using UnityEngine;
namespace _2DProject
{
    public class ModuleSlotSetting : MonoBehaviour
    {
        public ModuleSlot Slot;
        public ModuleInfo StartModule;

        private void Start()
        {
            Slot.SetItem(null);
            Slot.SetItem(StartModule);
        }
    }
}