using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _2DProject
{
    // Label(TMP_Text) + Slider + ValueText(TMP_Text) 로 구성된 프리팹에 붙이는 컴포넌트
    public class SliderRow : MonoBehaviour
    {
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text valueText;

        private Action<float> onValueChanged;

        public void Setup(string label, float value, float min, float max, Action<float> setter)
        {
            labelText.text = label;
            onValueChanged = setter;

            slider.onValueChanged.RemoveAllListeners();
            slider.minValue = min;
            slider.maxValue = max;
            slider.value = value;

            UpdateValueText(value);

            slider.onValueChanged.AddListener(OnSliderChanged);
        }

        private void OnSliderChanged(float newValue)
        {
            onValueChanged?.Invoke(newValue);
            UpdateValueText(newValue);
        }

        private void UpdateValueText(float value)
        {
            if (valueText != null)
                valueText.text = value.ToString("0.##");
        }
    }
}