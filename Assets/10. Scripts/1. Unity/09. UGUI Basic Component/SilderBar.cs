using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Study_SilderBar
{
    public class SilderBar : MonoBehaviour
    {
        private Slider[] sliders { get; set; }
        [field:SerializeField] private TMP_Text[] SliderTexts { get; set; }

        private void Awake()
        {
            sliders = GetComponentsInChildren<Slider>();
        }

        private void Update()
        {
        }

        private void UpdateSliderText()
        {
            // 모든 슬라이더 벨류들을 텍스트에 표시
            for (int i = 0; i < SliderTexts.Length; i++)
            {
                Slider targetSlider = sliders[i];
                TMP_Text targetText = SliderTexts[i];

                // 텍스트를 수정하는 두 가지 방법
                targetText.text = targetSlider.value.ToString();    // 텍스트에 바로 대입 아래 방법을 권장

                string text = $"{targetSlider.value * 100:F0}";  // 슬라이더의 값을 소수점 둘째 자리까지 문자열로 변환
                targetText.SetText(text);  // SetText() 함수를 호출해서 변경 최적화된 방법
            }
        }

        public void OnValueChanged(float value)
        {
            Debug.Log($"{value:F2}");
            UpdateSliderText();
        }

    }
}