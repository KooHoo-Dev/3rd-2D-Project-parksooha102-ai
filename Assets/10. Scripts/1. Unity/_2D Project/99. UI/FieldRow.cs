using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace _2DProject
{
    public class FieldRow : MonoBehaviour
    {
        [Header("UI Ref")]
        [SerializeField] private TMP_Text label;
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text currentValueText;
        [SerializeField] private TMP_Text minValueText;
        [SerializeField] private TMP_Text maxValueText;

        private ModuleInfo targetModule;
        private ModuleFieldInfo fieldInfo;

        public void Setup(ModuleInfo module, ModuleFieldInfo field)
        {
            targetModule = module;
            fieldInfo = field;

            label.text = field.Label;

            slider.minValue = field.MinValue;
            slider.maxValue = field.MaxValue;
            slider.wholeNumbers = field.IsInteger;

            slider.onValueChanged.RemoveAllListeners();
            slider.value = field.CurrentValue;

            currentValueText.text = FormatValue(field.CurrentValue);
            minValueText.text = FormatValue(field.MinValue);
            maxValueText.text = FormatValue(field.MaxValue);

            slider.onValueChanged.AddListener(HandleSliderChanged);
        }

        private void HandleSliderChanged(float newValue)
        {
            targetModule.SetFieldValue(fieldInfo.Label, newValue);
            currentValueText.text = FormatValue(newValue);
        }

        private string FormatValue(float value)
        {
            if (fieldInfo.IsInteger)
            {
                return Mathf.RoundToInt(value).ToString();
            }
            else
            {
                return value.ToString("F1");
            }
        }

    }
}