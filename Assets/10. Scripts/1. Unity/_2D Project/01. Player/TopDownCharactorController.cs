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
            // rBody.bodyType = RigidbodyType2D.Kinematic;

            col = GetComponent<CircleCollider2D>();
        }

        private void FixedUpdate()
        {
            Vector3 desireMove = moveInput * speed * Time.fixedDeltaTime + externalDelta;

            externalDelta = Vector3.zero;

            Vector3 moveVector = Vector3.zero;
            // moveVector.x = ResolveAxisMovement(desireMove.x, Vector3.right);
            // moveVector.y = ResolveAxisMovement(desireMove.y, Vector3.up);
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

        #region Private Methods (운동 파이프라인)

        /*
        private float ResolveAxisMovement(float move, Vector3 axis)
        {
            if (move == 0.0f) return 0.0f;

            Vector3 dir = (move > 0.0f ? axis : -axis);
            float distance = Mathf.Abs(move);
            Bounds box = GetCastBox();

            RaycastHit2D hit = Physics2D.BoxCast(box.center, box.size, 0.0f, dir, distance, collisionLayer);

            if (hit.collider == null) return move;

            float allowed = Mathf.Max(0, hit.distance - skinWidth);

            return Mathf.Sign(move) * allowed;
        }

        private Bounds GetCastBox()
        {
            Bounds box = col.bounds;

            Vector3 size = box.size;
            size.x -= skinWidth * 2;
            size.y -= skinWidth * 2;
            box.size = size;

            return box;
        }*/
        /*
        private float ResolveAxisMovement(float move, Vector3 axis)
        {
            if (move == 0.0f) return 0.0f;

            Vector3 dir = (move > 0.0f ? axis : -axis);
            float distance = Mathf.Abs(move);

            float castRadius = col.radius - skinWidth;

            RaycastHit2D hit = Physics2D.CircleCast(transform.position, castRadius, dir, distance, collisionLayer);

            if (hit.collider == null) return move;

            float allowed = Mathf.Max(0, hit.distance - skinWidth);

            return Mathf.Sign(move) * allowed;
        }*/

        #endregion

    }
}