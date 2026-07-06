using Study.Utilities;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _2DProject
{
    public class WorkShopInspectorController : MonoBehaviour
    {
        [Header("Inspector Panel Ref")]
        [SerializeField] private Transform fieldPanelParent;
        [SerializeField] private FieldRow fieldRowPrefab;
        [SerializeField] private TMP_Text moduleNameText;

        private GraphicRaycaster raycaster;

        private ModuleInfo currentModule;
        private Canvas canvas;

        private void Start()
        {
            ClearPanel();
            canvas = GetComponent<Canvas>();
            raycaster = GetComponent<GraphicRaycaster>();
        }

        private void Update()
        {
            if (IsValidScene() == false)
            {
                canvas.enabled = false;
                return;
            }
            else
            {
                canvas.enabled = true;
            }
            CheckMouseClick();
            CheckCurrentModuleStillEquipped();
        }

        private bool IsValidScene()
        {
            return SceneManager.GetActiveScene().buildIndex == (int)SceneController.GameScene.WorkShop;
        }

        private void CheckMouseClick()
        {
            if (SimpleInput.GetMouseButtonDown(0) == false)
                return;

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = SimpleInput.GetMousePosition();

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.TryGetComponent(out ModuleSlot slot))
                {
                    HandleSlotClicked(slot);
                    break;
                }
            }
        }

        private void HandleSlotClicked(ModuleSlot slot)
        {
            if (slot.IsEmpty)
                return;

            ModuleInventory inventory = ModuleInventorySystem.Instance.GetInventoryOrNull(slot);

            if (inventory == null)
                return;
            if (inventory.Type != InventoryType.Player)
                return;

            ShowModule(slot.Icon);
        }

        private void CheckCurrentModuleStillEquipped()
        {
            if (currentModule == null) return;

            bool stillEquipped = ModuleDataManager.Instance.EquippedModules.Contains(currentModule);

            if (stillEquipped == false)
            {
                currentModule = null;
                ClearPanel();
                moduleNameText.text = "Inspector";
            }
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