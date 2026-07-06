using Study.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Jay;

namespace _2DProject
{
    public class LabUIController : SingletonBase<LabUIController>
    {
        public enum PlacementMode
        {
            None,
            Dummy,
            Wall,
            Eraser
        }

        [Header("Prefabs")]
        [SerializeField] private GameObject dummyPrefab;
        [SerializeField] private GameObject wallPrefab;

        [Header("Placement Check")]
        [SerializeField] private float wallBaseLength = 1f;
        [SerializeField] private UnityEngine.UI.Image cursorIcon;

        [SerializeField] private Sprite wallIcon;
        [SerializeField] private Sprite eraserIcon;

        [SerializeField] private float wallThickness = 1f;
        [SerializeField] private float wallThicknessMin = 0.2f;
        [SerializeField] private float wallThicknessMax = 3f;

        private PlacementMode currentMode = PlacementMode.None;
        private Canvas canvas;

        private GameObject previewObject;
        private PlacementPreview previewPreview;
        private GameObject currentPreviewPrefab;

        private bool isDraggingWall;
        private Vector2 wallStartPos;

        private GameObject eraserHighlightTarget;
        private SpriteRenderer eraserHighlightRenderer;
        private Color eraserOriginalColor;

        private Camera mainCamera;
        private readonly Collider2D[] overlapResults = new Collider2D[16];

        protected override void OnInitialize()
        {
            base.OnInitialize();
            canvas = GetComponent<Canvas>();
            mainCamera = Camera.main;
        }

        private bool IsValidScene()
        {
            return SceneManager.GetActiveScene().buildIndex == (int)SceneController.GameScene.Lab;
        }

        private bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        private Vector2 GetMouseWorldPos()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            return mainCamera.ScreenToWorldPoint(mousePos);
        }

        private void Update()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            bool isValidScene = IsValidScene();
            bool isPressed = SimpleInput.GetKey(Key.Tab);

            bool isValid = isValidScene && isPressed;

            canvas.enabled = isValid;

            if (isValid == false)
            {
                ExitPlacementUI();
                return;
            }

            bool blockedByUI = IsPointerOverUI();

            if (blockedByUI)
            {
                ClearPreview();
                CancelWallDrag();
                ClearEraserHighlight();
                return;
            }

            switch (currentMode)
            {
                case PlacementMode.Dummy:
                    UpdateDummyPlacement();
                    break;

                case PlacementMode.Wall:
                    UpdateWallPlacement();
                    break;

                case PlacementMode.Eraser:
                    UpdateEraser();
                    break;
            }

            UpdateCursorIcon(isValid);
        }

        private void UpdateCursorIcon(bool enabled)
        {
            if (enabled == false)
                return;

            if (currentMode == PlacementMode.None)
            {
                cursorIcon.enabled = false;
                return;
            }

            cursorIcon.enabled = true;

            Vector2 mousePos = Mouse.current.position.ReadValue();

            cursorIcon.rectTransform.position = mousePos + new Vector2(30, -30);

            switch (currentMode)
            {
                case PlacementMode.Dummy:
                    cursorIcon.enabled = false;
                    break;

                case PlacementMode.Wall:
                    cursorIcon.sprite = wallIcon;
                    break;

                case PlacementMode.Eraser:
                    cursorIcon.sprite = eraserIcon;
                    break;
            }
        }

        private void ExitPlacementUI()
        {
            ClearPreview();
            CancelWallDrag();
            ClearEraserHighlight();
        }

        #region Mode Select

        public void SelectDummy()
        {
            currentMode = PlacementMode.Dummy;
            ClearPreview();
            CancelWallDrag();
            ClearEraserHighlight();
        }

        public void SelectWall()
        {
            currentMode = PlacementMode.Wall;
            ClearPreview();
            CancelWallDrag();
            ClearEraserHighlight();
        }

        public void SelectEraser()
        {
            currentMode = PlacementMode.Eraser;
            ClearPreview();
            CancelWallDrag();
        }

        #endregion

        #region Dummy Placement

        private void UpdateDummyPlacement()
        {
            Vector2 mousePos = GetMouseWorldPos();

            EnsurePreview(dummyPrefab);
            previewObject.transform.position = mousePos;
            previewObject.transform.rotation = Quaternion.identity;
            
            bool canPlace = IsPlacementValid();
            previewPreview.SetValid(canPlace);

            if (canPlace && Mouse.current.leftButton.wasPressedThisFrame)
            {
                Instantiate(dummyPrefab, mousePos, Quaternion.identity);
            }
        }

        #endregion

        #region Wall Placement

        private void UpdateWallPlacement()
        {
            Vector2 mousePos = GetMouseWorldPos();

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                isDraggingWall = true;
                wallStartPos = mousePos;
                EnsurePreview(wallPrefab);
            }

            if (isDraggingWall)
            {
                Vector2 delta = mousePos - wallStartPos;

                float length = Mathf.Max(delta.magnitude, 0.1f);

                Vector2 direction = delta.normalized;

                if (direction == Vector2.zero)
                    direction = Vector2.right;

                float wallAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                Vector2 center = wallStartPos + delta * 0.5f;

                float scroll = Mouse.current.scroll.ReadValue().y;

                if (Mathf.Abs(scroll) > 0.01f)
                {
                    wallThickness += scroll * 0.1f;
                    wallThickness = Mathf.Clamp(wallThickness, wallThicknessMin, wallThicknessMax);
                }

                previewObject.transform.position = center;
                previewObject.transform.rotation = Quaternion.Euler(0, 0, wallAngle);
                previewObject.transform.localScale = new Vector3(length / wallBaseLength, wallThickness, 1f);

                bool isValid = IsPlacementValid();
                previewPreview.SetValid(isValid);
                
                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    if (isValid)
                    {
                        GameObject wall = Instantiate(wallPrefab, center, Quaternion.Euler(0, 0, wallAngle));
                        wall.transform.localScale = new Vector3(length / wallBaseLength, wallThickness, 1f);
                    }

                    CancelWallDrag();
                    ClearPreview();
                }
            }
        }

        private void CancelWallDrag()
        {
            isDraggingWall = false;
        }

        #endregion

        #region Eraser

        private void UpdateEraser()
        {
            Vector2 worldPos = GetMouseWorldPos();
            Collider2D hit = Physics2D.OverlapPoint(worldPos);

            GameObject hitObject = hit != null ? hit.gameObject : null;
            bool isPlayer = hit != null && hit.GetComponentInParent<PlayerController>() != null;

            if (hitObject != eraserHighlightTarget)
            {
                ClearEraserHighlight();

                if (hitObject != null && isPlayer == false)
                {
                    SpriteRenderer renderer = hitObject.GetComponentInChildren<SpriteRenderer>();

                    if (renderer != null)
                    {
                        eraserHighlightTarget = hitObject;
                        eraserHighlightRenderer = renderer;
                        eraserOriginalColor = renderer.color;

                        Color faded = eraserOriginalColor;
                        faded.a = eraserOriginalColor.a * 0.5f;
                        renderer.color = faded;
                    }
                }
            }

            if (Mouse.current.leftButton.wasPressedThisFrame == false)
                return;

            if (hit == null) return;

            if (isPlayer)
            {
                Debug.Log("플레이어는 지울 수 없습니다.");
                return;
            }

            ClearEraserHighlight();
            Destroy(hit.gameObject);
        }

        private void ClearEraserHighlight()
        {
            if (eraserHighlightRenderer != null)
            {
                eraserHighlightRenderer.color = eraserOriginalColor;
            }

            eraserHighlightTarget = null;
            eraserHighlightRenderer = null;
        }

        #endregion

        #region Preview Management

        private void EnsurePreview(GameObject prefab)
        {
            if (currentPreviewPrefab == prefab && previewObject != null)
                return;

            ClearPreview();

            previewObject = Instantiate(prefab);

            SetLayerRecursively(previewObject, LayerMask.NameToLayer("Preview"));

            previewPreview = previewObject.AddComponent<PlacementPreview>();
            currentPreviewPrefab = prefab;
        }

        private void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform child in obj.transform)
                SetLayerRecursively(child.gameObject, layer);
        }

        private void ClearPreview()
        {
            if (previewObject != null)
            {
                Destroy(previewObject);
                previewObject = null;
                previewPreview = null;
            }
        }

        private bool IsPlacementValid()
        {
            Collider2D col = previewObject.GetComponentInChildren<Collider2D>();

            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = false;

            int count = col.Overlap(filter, overlapResults);

            for (int i = 0; i < count; i++)
            {
                if (overlapResults[i] == col)
                    continue;

                return false;
            }

            return true;
        }

        #endregion

    }
}