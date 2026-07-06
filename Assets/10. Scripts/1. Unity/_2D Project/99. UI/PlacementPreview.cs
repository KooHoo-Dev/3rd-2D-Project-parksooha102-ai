using UnityEngine;

namespace _2DProject
{
    public class PlacementPreview : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private static readonly Color ValidColor = new Color(0f, 1f, 0f, 0.5f);
        private static readonly Color InvalidColor = new Color(1f, 0f, 0f, 0.5f);

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            foreach (Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>())
                rb.simulated = false;

            foreach (MonoBehaviour script in GetComponentsInChildren<MonoBehaviour>())
            {
                if (script == this)
                    continue;

                script.enabled = false;
            }
        }

        public void SetValid(bool isValid)
        {
            if (spriteRenderer == null) return;
            spriteRenderer.color = isValid ? ValidColor : InvalidColor;
        }
    }
}