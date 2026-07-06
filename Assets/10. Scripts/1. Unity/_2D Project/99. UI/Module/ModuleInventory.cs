using UnityEngine;

namespace _2DProject
{
    public enum InventoryType
    {
        None,
        Player,
        List,
    }

    public class ModuleInventory : MonoBehaviour
    {
        [Header("Inventory Settings")]
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public InventoryType Type = InventoryType.None;
        [field: SerializeField] public int SizeRow { get; private set; } = 7;
        [field: SerializeField] public int SizeColumn { get; private set; } = 1;

        public ModuleSlot[,] SlotGrid { get; private set; }

        private void Awake()
        {
            InitSlotGrid();

            void InitSlotGrid()
            {
                SlotGrid = new ModuleSlot[SizeRow, SizeColumn];

                ModuleSlot[] slots = GetComponentsInChildren<ModuleSlot>();

                for (int i = 0; i < slots.Length; ++i)
                {
                    int rowIndex = i / SizeColumn;
                    int columnIndex = i % SizeColumn;

                    SlotGrid[rowIndex, columnIndex] = slots[i];
                }
            }
        }

        private void Start()
        {
            ModuleInventorySystem.Instance.Register(this);
        }

        public bool IsIn(ModuleSlot slot)
        {
            if (slot == null) return false;

            for (int x = 0; x < SlotGrid.GetLength(0); ++x)
            {
                for (int y = 0; y < SlotGrid.GetLength(1); ++y)
                {
                    if (SlotGrid[x, y] == slot) return true;
                }
            }

            return false;
        }
    }

}

