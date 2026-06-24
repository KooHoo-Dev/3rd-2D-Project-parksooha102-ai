using UnityEngine;

namespace Study.PrimitiveAndVector
{
    public class Study_Lerp : MonoBehaviour
    {
        public bool isRun = true;
        public float speed = 1.0f;
        public float goalTime = 2.0f;
        public Vector3 goalPosition;

        private float currentTime = 0.0f;
        private Vector3 startPosition;

        private void Start()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            if (isRun)
            {
                currentTime += Time.deltaTime;

                float progress = currentTime / goalTime;
                Vector3 currentPositon = startPosition + (goalPosition - startPosition) * progress;
                transform.position = currentPositon;
            }

            if(currentTime > goalTime)
            {
                currentTime = 0.0f;
                Vector3 temp = startPosition;
                startPosition = goalPosition;
                goalPosition = temp;
            }
        }

    }
}