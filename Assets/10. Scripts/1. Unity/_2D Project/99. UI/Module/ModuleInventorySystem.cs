using Jay;
using System.Collections.Generic;

namespace _2DProject
{
    public class ModuleInventorySystem : SingletonBase<ModuleInventorySystem>
    {
        private List<ModuleInventory> Inventories { get; set; } = new List<ModuleInventory>();

        public void Register(ModuleInventory inventory)
        {
            Inventories.Add(inventory);
        }

        public void Remove(ModuleInventory inventory)
        {
            Inventories.Remove(inventory);
        }

        public ModuleInventory GetInventoryOrNull(string targetName)
        {
            for (int i = 0; i < Inventories.Count; i++)
            {
                if (Inventories[i].name.Equals(targetName))
                    return Inventories[i];
            }

            return null;
        }

        public ModuleInventory GetInventoryOrNull(ModuleSlot slot)
        {
            for (int i = 0; i < Inventories.Count; ++i)
            {
                if (Inventories[i].IsIn(slot)) return Inventories[i];
            }

            return null;
        }

    }
}