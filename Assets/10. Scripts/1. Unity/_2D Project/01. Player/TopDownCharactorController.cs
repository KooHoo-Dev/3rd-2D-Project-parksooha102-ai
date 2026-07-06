using UnityEngine;

namespace _2DProject
{
    public class TopDownCharactorController : MonoBehaviour
    {
        [Header("Settings")]
        public float speed = 2.0f;
        public float skinWidth = 0.02f;
        public LayerMask collisionLayer;

        private Rigidbody2D rBody;
        private CircleCollider2D col;

        private Vector3 externalDelta = Vector3.zero;
        private Vector3 moveInput = new Vector3();

        #region Unity Methods

        private void Awake()
        {
            rBody = GetComponent<Rigidbody2D>();

            col = GetComponent<CircleCollider2D>();
        }

        private void FixedUpdate()
        {
            Vector3 desireMove = moveInput * speed * Time.fixedDeltaTime + externalDelta;

            externalDelta = Vector3.zero;

            Vector3 moveVector = Vector3.zero;
            moveVector = desireMove;

            rBody.MovePosition(transform.position + moveVector);
        }

        #endregion

        #region Public Methods (명령 통로)

        public void SetMoveInput(Vector2 inputVector)
        {
            moveInput.x = Mathf.Clamp(inputVector.x, -1.0f, 1.0f);
            moveInput.y = Mathf.Clamp(inputVector.y, -1.0f, 1.0f);

            moveInput = moveInput.normalized;
        }

        public void AddExternalMovement(Vector3 delta)
        {
            externalDelta += delta;
        }

        #endregion

    }
}