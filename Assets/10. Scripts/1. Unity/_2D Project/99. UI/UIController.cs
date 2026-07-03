using Study.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using Jay;

namespace _2DProject
{
    public class UIController : SingletonBase<UIController>
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

        protected override void OnInitialize()
        {
            base.OnInitialize();
            canvas = GetComponent<Canvas>();
        }

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