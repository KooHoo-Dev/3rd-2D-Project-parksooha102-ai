using Study.Utilities;
using UnityEngine;
using UnityEngine.UIElements;

namespace Study_2D_Direction_And_Rotation
{
    public class Study_Transform : MonoBehaviour
    {
        [field: SerializeField] private Transform Circle;

        private void Update()
        {
            Vector3 goalPosition = Circle.position;

            Vector3 myPosition = transform.position;

            Vector3 dir = (goalPosition - myPosition).normalized;
            transform.up = dir;
        }


    }
}
