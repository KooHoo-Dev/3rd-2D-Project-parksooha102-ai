using UnityEngine;
namespace _2DProject
{
    public class ModuleSlotSetting : MonoBehaviour
    {
        private ModuleSlot Slot;
        public ModuleInfo StartModule;

        private void Start()
        {
            Slot = GetComponent<ModuleSlot>();
            Slot.SetItem(StartModule);
        }
    }
}