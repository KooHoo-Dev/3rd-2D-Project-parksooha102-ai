using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace _2DProject
{
    public class WorkShopInspectorController : MonoBehaviour
    {
        [Header("Inspector Panel Ref")]
        [SerializeField] private Transform fieldPanelParent;      // 필드들이 생성될 부모 (Inspector 영역)
        [SerializeField] private FieldRow fieldRowPrefab;       // 필드 한 줄 (라벨+슬라이더) 프리팹
        [SerializeField] private TMP_Text moduleNameText;         // 인스펙터 상단 모듈 이름 표시

        private ModuleInfo currentModule;

        private void Start()
        {
            // 씬의 모든 슬롯에 클릭 이벤트 구독
            ModuleSlot[] allSlots = FindObjectsByType<ModuleSlot>(FindObjectsSortMode.None);

            foreach (ModuleSlot slot in allSlots)
            {
                slot.OnSlotClicked += HandleSlotClicked;
            }

            ClearPanel();
        }

        private void HandleSlotClicked(ModuleSlot slot)
        {
            ModuleInventory inventory = ModuleInventorySystem.Instance.GetInventoryOrNull(slot);

            if (inventory == null) return;

            // Build List(장착 슬롯)가 아니면 무시
            if (inventory.Type != InventoryType.Player) return;

            ShowModule(slot.Icon);
        }

        private void ShowModule(ModuleInfo module)
        {
            currentModule = module;

            ClearPanel();

            moduleNameText.text = module.Description;

            List<ModuleFieldInfo> fields = module.GetEditableFields();

            foreach (ModuleFieldInfo field in fields)
            {
                CreateFieldRow(field);
            }
        }

        private void CreateFieldRow(ModuleFieldInfo field)
        {
            FieldRow row = Instantiate(fieldRowPrefab, fieldPanelParent);
            row.Setup(currentModule, field);
        }

        private void ClearPanel()
        {
            foreach (Transform child in fieldPanelParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}