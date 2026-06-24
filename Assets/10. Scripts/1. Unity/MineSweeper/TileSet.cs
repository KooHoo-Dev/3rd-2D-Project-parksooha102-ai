using System.Collections.Generic;
using UnityEngine;

namespace Study.MineSweeper
{
    public class TileSet : MonoBehaviour
    {
        public const float BOARD_SIZE = 8.0f;
        public Tile[] tiles;
        public Tile[,] tiles2D;
        public int xLength;
        public int yLength;
        public float boxSize;
        public Vector3 firstBoxPos;
        public int mineCount;
        public int correct { get; private set; }
        public int answer { get; private set; }
        public bool isRun { get; private set; }

        public void SetBoard()
        {
            Make2DArray();
            correct = 0;
            answer = tiles.Length - mineCount;
            isRun = true;
        }

        private void Make2DArray()
        {
            Debug.Log("Make2DArray");
            tiles2D = new Tile[xLength, yLength];
            SetXLengthYLength(xLength, yLength);

            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    tiles2D[i, j] = tiles[i + j * xLength];
                    tiles[i + j * xLength].Set2DArray(i, j);
                    float x = firstBoxPos.x + boxSize * i;
                    float y = firstBoxPos.y + boxSize * j;
                    Vector3 position = new Vector3(x, y, 0.0f);

                    tiles[i + j * xLength].transform.localPosition = position;
                    tiles2D[i, j].GetComponent<Collider2D>();
                    Debug.Log($" tiles2D ： {tiles2D[i, j] }, tiles : {tiles[i + j * xLength]}, position : {position}"  );
                }
            }
        }

        private void CheckAroundMine()
        {
            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    CheckAroundMine(i, j);
                }
            }
        }

        private void CheckAroundMine(int x, int y)
        {
            if (tiles2D[x, y].isMine) return;

            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int nx = x + i;
                    int ny = y + j;

                    if (i == 0 && j == 0) continue;
                    if (IsValid(nx, ny) == false) continue;

                    if (tiles2D[nx, ny].isMine)
                    {
                        count++;
                    }
                }
            }

            tiles2D[x, y].SetAroundMineCount(count);
        }

        // 자기자신과 가생이는 예외처리
        private bool IsValid(int x, int y)
        {
            if (x < 0 || x >= xLength || y < 0 || y >= yLength) return false;
            else return true;
        }

        public void Reveal(int x, int y)
        {
            Tile.RevealResult result = Tile.RevealResult.None;
            if (tiles2D[x, y].isUnlock || tiles2D[x, y].isFlaged) return;

            result = tiles2D[x, y].Reveal();

            if (result == Tile.RevealResult.Mine)
            {
                Debug.Log("패배");
                GameOver();
                return;
            }
            else if (result == Tile.RevealResult.Correct)
            {
                correct++;

                if (correct >= answer)
                {
                    Debug.Log("승리!");
                    GameClear();
                    return;
                }

                if (tiles2D[x, y].aroundMineCount == Tile.AroundMine.Zero)
                {
                    RevealAround(x, y);
                }
            }

        }

        public void QuickReveal(int x, int y)
        {
            if (tiles2D[x, y].isUnlock == false) return;
            if (tiles2D[x, y].isMine) return;

            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int nx = x + i;
                    int ny = y + j;

                    if (i == 0 && j == 0) continue;
                    if (IsValid(nx, ny) == false) continue;

                    if (tiles2D[nx, ny].isFlaged)
                    {
                        count++;
                    }
                }
            }

            if ((int)tiles2D[x, y].aroundMineCount == count) RevealAround(x, y);
            else return;
        }

        private void RevealAround(int x, int y)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int nx = x + i;
                    int ny = y + j;

                    // 자기 자신, 가생이 예외처리
                    if (i == 0 && j == 0) continue;
                    else if (IsValid(nx, ny) == false) continue;
                    else Reveal(nx, ny);
                }
            }
        }

        public void SetMine(int x, int y)
        {
            List<Tile> temp = new List<Tile>(tiles);

            temp.RemoveAt(x + y * xLength);

            for (int i = 0; i < mineCount; i++)
            {
                int rand = Random.Range(0, temp.Count);

                temp[rand].SetMine();

                temp.RemoveAt(rand);
            }

            CheckAroundMine();
        }

        public void CheckFlag(int x, int y)
        {
            if (tiles2D[x, y].isUnlock) return;
            tiles2D[x, y].FlagState();
        }

        private void GameOver()
        {
            isRun = false;
            MineSweeperManager.Instance.GameOver();
        }

        private void GameClear()
        {
            isRun = false;
            MineSweeperManager.Instance.GameClear();
        }

        public void SetXLengthYLength(int x, int y)
        {
            xLength = x;
            yLength = y;
            boxSize = (x < y) ? BOARD_SIZE / x : BOARD_SIZE / y;

            firstBoxPos.x = -(((float)x / 2) * boxSize) + (boxSize / 2);
            firstBoxPos.y = -(((float)y / 2) * boxSize) + (boxSize / 2);
        }

        public void ResetBoard()
        {
            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    tiles2D[i, j].Reset();
                }
            }
            correct = 0;
            answer = tiles.Length - mineCount;
            isRun = true;
        }

    }
}