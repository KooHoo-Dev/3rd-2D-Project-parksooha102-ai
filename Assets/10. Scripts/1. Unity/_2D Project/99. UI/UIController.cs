using Study.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // 씬제어를 위한 기능들이 담겨있는 네임스페이스
using Jay;
using _2DProject;

namespace Study_SceneManagement
{
    // # 동기적 씬 로딩
    // - 가장 간단한 방법으로 유니티에서 가장 많이 사용합니다.
    // - 씬의 모든 요소들을 불러오며, 불러오자마자 현재 씬을 바꿔줍니다.

    public class Study_SceneManagement : SingletonBase<Study_SceneManagement>
    {
        public enum PlacementMode
        {
            None,
            Dummy,
            Wall,
            Eraser
        }

        [SerializeField] private GameObject dummyPrefab;
        [SerializeField] private GameObject wallPrefab;

        private PlacementMode currentMode = PlacementMode.None;

        private Canvas canvas;

        // Awake가 아닌 SingletonBase의 OnInitialize 함수를 재정의하여 초기화 합니다
        protected override void OnInitialize()
        {
            base.OnInitialize();
            canvas = GetComponent<Canvas>();
        }

        // Update is called once per frame
        private void Update()
        {
            bool isPressed = SimpleInput.GetKey(Key.Tab);
            canvas.enabled = isPressed;

            if (!isPressed)
                return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                PlaceObject();
            }
        }

        // 각 버튼에 해당하는 오브젝트를 마우스의 월드 위치에 생성

        // 1번 버튼: 허수아비 더미
        public void SelectDummy()
        {
            currentMode = PlacementMode.Dummy;
        }

        // 2번 버튼: 벽
        public void SelectWall()
        {
            currentMode = PlacementMode.Wall;
        }

        // 3번 버튼: 지우개
        public void SelectEraser()
        {
            currentMode = PlacementMode.Eraser;
        }

        private void PlaceObject()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);

            switch (currentMode)
            {
                case PlacementMode.Dummy:
                    Instantiate(dummyPrefab, worldPos, Quaternion.identity);
                    break;

                case PlacementMode.Wall:
                    Instantiate(wallPrefab, worldPos, Quaternion.identity);
                    break;

                case PlacementMode.Eraser:

                    Collider2D hit = Physics2D.OverlapPoint(worldPos);

                    if (hit != null)
                    {
                        hit.gameObject.SetActive(false);
                    }

                    break;
            }
        }

    }
}