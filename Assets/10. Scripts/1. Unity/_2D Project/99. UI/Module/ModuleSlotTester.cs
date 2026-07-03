using Study.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _2DProject
{
    public class ModuleSlotTester : MonoBehaviour
    {
        public ModuleSlot Slot;
        public ModuleInfo TestModule;

        private void Start()
        {
            Slot.SetItem(null);
        }

        private void Update()
        {
            if (SimpleInput.GetKeyDown(Key.Digit1))
            {
                Slot.SetItem(TestModule);
            }

            if (SimpleInput.GetKeyDown(Key.Digit2))
            {
                Slot.SetItem(null);
            }
        }
    }
}