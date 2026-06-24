using Study.Utilities;
using UnityEngine;

namespace Study_2D_Direction_And_Rotation
{
    public class Study_WorldMousePoint : MonoBehaviour
    {
        private float ScreenWidth { get; set; }
        private float ScreenHeight { get; set; }

        private void Start()
        {
            // Screen 클래스에 접근해서 현재 화면의 너비와 높이값을 가져옵니다.
            ScreenWidth = Screen.width;
            ScreenHeight = Screen.height;
        }

        private void Update()
        {
            Vector2 mousePosition = SimpleInput.GetMousePosition();

            // 화면 좌표계에서 World(Scene)의 좌표계로 전환
            Vector3 worldMousePoint = Camera.main.ScreenToWorldPoint(mousePosition);
            // 전환할때는 Camera의 기능을 이용

            worldMousePoint.z = 0;

            transform.position = worldMousePoint;
        }

    }
}