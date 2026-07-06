using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _2DProject
{
    public class ModuleInventoryInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [field: SerializeField]
        public ModuleSlot CursorSlot { get; private set; }

        private GraphicRaycaster raycaster;

        private ModuleSlot startSlot;
        private ModuleSlot endSlot;

        private void Start()
        {
            raycaster = GetComponent<GraphicRaycaster>();
        }

        private ModuleSlot GetSlotOrNull(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();

            raycaster.Raycast(eventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.TryGetComponent(out ModuleSlot slot))
                {
                    if (slot == CursorSlot) continue;
                    else return slot;
                }
            }

            return null;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            startSlot = GetSlotOrNull(eventData);
            if (startSlot == null) return;
            if (startSlot.IsEmpty) return;

            if (startSlot.Icon.IsFixed)
            {
                startSlot = null;
                return;
            }

            CursorSlot.SetItem(startSlot.Icon);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (startSlot == null) return;
            CursorSlot.SetPosition(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (startSlot == null) return;

            endSlot = GetSlotOrNull(eventData);

            if (endSlot == null)
            {
                ResetCursor();
                return;
            }

            if (endSlot.IsEmpty)
            {
                endSlot.SetItem(CursorSlot.Icon);
                startSlot.SetItem(null);
                CheckTrade(startSlot, endSlot);
            }
            else
            {
                ModuleInventory startInven = ModuleInventorySystem.Instance.GetInventoryOrNull(startSlot);
                ModuleInventory endInven = ModuleInventorySystem.Instance.GetInventoryOrNull(endSlot);

                if (startInven.Type == endInven.Type)
                {
                    startSlot.SetItem(endSlot.Icon);
                    endSlot.SetItem(CursorSlot.Icon);
                }
            }

            ResetCursor();
        }

        private void ResetCursor()
        {
            CursorSlot.SetItem(null);
            startSlot = null;
        }

        private void CheckTrade(ModuleSlot start, ModuleSlot end)
        {
            ModuleInventory startInven = ModuleInventorySystem.Instance.GetInventoryOrNull(start);
            ModuleInventory endInven = ModuleInventorySystem.Instance.GetInventoryOrNull(end);
            ModuleInfo icon = CursorSlot.Icon;

            if (startInven == null || endInven == null) return;
            if (startInven.Type == endInven.Type) return;

            EquipSystem.EquipInfo tradeInfo = new EquipSystem.EquipInfo();
            tradeInfo.Start = startSlot;
            tradeInfo.End = endSlot;
            tradeInfo.Item = icon;

            switch (endInven.Type)
            {
                case InventoryType.List:
                    tradeInfo.EquipType = EquipSystem.EquipType.Unequip;
                    break;
                case InventoryType.Player:
                    tradeInfo.EquipType = EquipSystem.EquipType.Equip;
                    break;
            }

            EquipSystem.Instance.RequestTradeEvent(tradeInfo);
        }
    }

}

