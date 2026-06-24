using UnityEngine;
using UnityEngine.InputSystem;

namespace _2DProject
{
    public class AimPoint : MonoBehaviour
    {
        public Vector3 worldAimPoint { get; private set; }

        private void Update()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            worldAimPoint = Camera.main.ScreenToWorldPoint(mousePosition);
            worldAimPoint = new Vector3 (worldAimPoint.x, worldAimPoint.y, 0);
            transform.position = worldAimPoint;
        }

    }
}