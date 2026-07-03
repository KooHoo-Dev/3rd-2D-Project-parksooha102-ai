using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace _2DProject
{
    public class ModuleSlot : MonoBehaviour
    {
        [field: SerializeField] public Image Background { get; private set; }
        [field: SerializeField] public Image Content { get; private set; }
        [field: SerializeField] public TMP_Text NameText { get; private set; }

        private RectTransform rectTransform;
        private ModuleInfo module;

        public bool IsEmpty => (module == null);
        
        public ModuleInfo Icon => module;

        private void Start()
        {
            SetItem(null);

            rectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// Null을 허용하는 함수입니다. Null 전달시 슬롯이 비워집니다.
        /// </summary>
        /// <param name="inputItem"></param>
        public void SetItem(ModuleInfo inputItem)
        {
            module = inputItem;

            if (module == null)
            {
                Color adjustedColor = Background.color;
                adjustedColor.a = 0;
                Background.color = adjustedColor;
                Content.sprite = null;
                Content.enabled = false;
                NameText.text = string.Empty;
            }
            else
            {
                Color adjustedColor = Background.color;
                adjustedColor.a = 1;
                Background.color = adjustedColor;
                Content.sprite = module.Icon;
                Content.enabled = true;
                NameText.text = module.Description;
            }
        }

        public void SetPosition(Vector2 inputPosition)
        {
            rectTransform.anchoredPosition = inputPosition;
        }

    }
}
