using UnityEngine;

namespace Study.MineSweeper
{
    public class Tile : MonoBehaviour
    {
        public enum RevealResult
        {
            None, Correct, Mine,
        }

        public enum AroundMine
        {
            Zero, One, Two, Three, Four, Five, Six, Seven, Eight,
        }

        [Header("Tile Sprite")]
        public SpriteRenderer tileRenderer;
        public SpriteRenderer innerTileRenderer;
        public SpriteRenderer shapeRenderer;
        public Sprite Flag;
        public Sprite Mine;
        public Sprite[] shapeEnum;

        public bool isUnlock { get; private set; }
        public bool isMine { get; private set; }
        public bool isFlaged { get; private set; }
        public AroundMine aroundMineCount { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }

        private void Awake()
        {
            tileRenderer = tileRenderer.GetComponent<SpriteRenderer>();
            innerTileRenderer = innerTileRenderer.GetComponent<SpriteRenderer>();
            shapeRenderer = shapeRenderer.GetComponent<SpriteRenderer>();
            isUnlock = false;
            isMine = false;
            isFlaged = false;
            aroundMineCount = AroundMine.Zero;
            tileRenderer.color = Color.gray;
            innerTileRenderer.color = Color.white;
        }

        public RevealResult Reveal()
        {
            if (isUnlock || isFlaged) return RevealResult.None;

            isUnlock = true;

            tileRenderer.color = Color.white;
            innerTileRenderer.color = Color.black;

            if (isMine)
            {
                shapeRenderer.sprite = Mine;
                return RevealResult.Mine;
            }
            else
            {
                shapeRenderer.sprite = shapeEnum[(int)aroundMineCount];
                return RevealResult.Correct;
            }
        }

        public void SetShape (AroundMine shape)
        {
            aroundMineCount = shape;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public void FlagState()
        {
            if (isUnlock) return;

            isFlaged = !isFlaged;
            Debug.Log("깃발");
            if (isFlaged) shapeRenderer.sprite = Flag;
            else shapeRenderer.sprite = null;
        }

        public void SetMine()
        {
            isMine = true;
        }

        public void SetAroundMineCount(int count)
        {
            aroundMineCount = (AroundMine)count;
        }

        public void Set2DArray(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Reset()
        {
            isUnlock = false;
            isMine = false;
            isFlaged = false;
            aroundMineCount = AroundMine.Zero;
            tileRenderer.color = Color.gray;
            innerTileRenderer.color = Color.white;
            shapeRenderer.sprite = shapeEnum[(int)aroundMineCount];
        }

    }
}