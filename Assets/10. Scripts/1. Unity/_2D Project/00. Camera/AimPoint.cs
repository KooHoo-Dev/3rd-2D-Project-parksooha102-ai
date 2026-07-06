using Study.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _2DProject
{
    public class AimPoint : MonoBehaviour
    {
        public Vector3 WorldAimPoint { get; private set; }
        private SpriteRenderer spriteRenderer;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (SimpleInput.GetKey(Key.Tab))
                spriteRenderer.enabled = false;
            else
                spriteRenderer.enabled = true;

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            WorldAimPoint = Camera.main.ScreenToWorldPoint(mousePosition);
            WorldAimPoint = new Vector3 (WorldAimPoint.x, WorldAimPoint.y, 0);
            transform.position = WorldAimPoint;
        }

    }
}