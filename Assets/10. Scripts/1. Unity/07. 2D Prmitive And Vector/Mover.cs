using UnityEngine;

namespace Study.PrimitiveAndVector
{
    public class Mover : MonoBehaviour
    {

        public float speed = 1.0f;
        public float goalTime = 2.0f;
        public Vector3 goalPosition;

        private float currentTime = 0.0f;
        private Vector3 startPosition;

        private Rigidbody2D rBody;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            rBody = GetComponent<Rigidbody2D>();
            rBody.bodyType = RigidbodyType2D.Kinematic; // 프로그래머가 키네마틱 운동으로 

            startPosition = transform.position;
            goalPosition += startPosition + goalPosition;
        }

        // Update is called once per frame
        void FixedUpdate()
        {

            currentTime += Time.fixedDeltaTime;

            float progress = currentTime / goalTime;
            Vector3 currentPositon = Vector3.Lerp(startPosition, goalPosition, progress);

            rBody.MovePosition(currentPositon);


            if (currentTime > goalTime)
            {
                currentTime = 0.0f;
                Vector3 temp = startPosition;
                startPosition = goalPosition;
                goalPosition = temp;
            }
        }
    }
}