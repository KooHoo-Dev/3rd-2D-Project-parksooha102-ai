using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Study.MineSweeper
{
    public class MineSweeperManager : MonoBehaviour
    {
        public static MineSweeperManager Instance;

        public TileSet tileSet;

        [Header("gameover settings")]
        public List<GameObject> loseEnableObjects;
        public List<GameObject> winEnableObjects;
        public SpriteRenderer[] timer;
        public Sprite[] number;

        private Camera mainCamera;
        private float timeLimit;
        private float remainTime;
        private bool isFirstClick = true;

        private void Awake()
        {
            NewGame();
        }

        private void NewGame()
        {
            tileSet.SetBoard();
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            mainCamera = Camera.main;
            timeLimit = 60.0f;
            remainTime = timeLimit;
            SetTimer();

            for (int i = 0; i < loseEnableObjects.Count; i++) loseEnableObjects[i].SetActive(false);
            for (int i = 0; i < winEnableObjects.Count; i++) winEnableObjects[i].SetActive(false);
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && tileSet.isRun) LeftClick(true);

            if (Mouse.current.rightButton.wasPressedThisFrame && tileSet.isRun) LeftClick(false);

            if (Keyboard.current.rKey.wasPressedThisFrame) Restart();
        }

        private void FixedUpdate()
        {
            if (isFirstClick != true && tileSet.isRun)
            {
                remainTime -= Time.deltaTime;
                SetTimer();
            }

            if (remainTime <= 0)
            {
                remainTime = 0;
                GameOver();
            }
        }

        private void Reveal(Tile tile)
        {
            if (tile == null) return;

            if (isFirstClick)
            {
                tileSet.SetMine(tile.x, tile.y);
                isFirstClick = false;
            }

            if (tile.isUnlock) tileSet.QuickReveal(tile.x, tile.y);
            else tileSet.Reveal(tile.x, tile.y);
        }
        
        private void CheckFlag(Tile tile)
        {
            Debug.Log("우클릭");
            if (tile == null) return;
            tileSet.CheckFlag(tile.x, tile.y);
        }

        private void LeftClick(bool isLeft)
        {
            Debug.Log("왼클릭");
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector3.forward);

            if (hit.collider == null)
            {
                Debug.Log("적중 안함");
                return;
            }

            Debug.Log("적중");

            Tile tile = hit.collider.GetComponent<Tile>();
            if (isLeft) Reveal(tile);
            else CheckFlag(tile);
        }

        private void SetTimer()
        {
            if (remainTime > 99.0f) remainTime = 99.0f;
            timer[0].sprite = number[(int)remainTime % 10];
            timer[1].sprite = number[(int)remainTime / 10];
        }

        public void GameOver()
        {
            for (int i = 0; i < loseEnableObjects.Count; i++) loseEnableObjects[i].SetActive(true);
        }

        public void GameClear()
        {
            for (int i = 0; i < winEnableObjects.Count; i++) winEnableObjects[i].SetActive(true);

        }

        public void Restart()
        {
            remainTime = timeLimit;
            SetTimer();
            isFirstClick = true;
            tileSet.ResetBoard();
            for (int i = 0; i < loseEnableObjects.Count; i++) loseEnableObjects[i].SetActive(false);
            for (int i = 0; i < winEnableObjects.Count; i++) winEnableObjects[i].SetActive(false);
        }
    }
}