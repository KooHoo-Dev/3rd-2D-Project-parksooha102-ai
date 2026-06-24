using UnityEngine;
using UnityEngine.UI; // UnityEngine.UI 네임스페이스를 사용하여 UI 관련 클래스를 사용할 수 있도록 합니다.


namespace Study_ProgressBar
{
    public class Study_ProgressBar : MonoBehaviour
    {
        [field: SerializeField] public Image progressBarA { get; private set; } // 인스펙터에서 할당 가능하지만 외부에서 읽기 전용으로 사용할 수 있는 프로퍼티입니다.
        [field: SerializeField] public Image progressBarB { get; private set; }
        [field: SerializeField] public Image progressBarC { get; private set; }
        [field: SerializeField] public Image progressBarD { get; private set; }

        [field: SerializeField] public int MaxNumber { get; set; } = 100;
        [field: SerializeField] public int SumAmount { get; set; } = 1;

        private int currentNumber = 0;

        private Image[] progressBars;

        private void Start()
        {
            progressBars = new Image[] { progressBarA, progressBarB, progressBarC, progressBarD };
        }

        private void Update()
        {
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            currentNumber += SumAmount;
            float fillAmount = (float)currentNumber / MaxNumber;
            if (currentNumber >= MaxNumber) currentNumber = 0;

            // Image 배열을 돌면서 각 프로그레스 바의 fillAmount를 업데이트합니다.
            for(int i = 0; i < progressBars.Length; i++) {
                progressBars[i].fillAmount = fillAmount;
            }



        }

    }
}