using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;
namespace Study.PrimitiveAndVector_Deactivate
{
    public class CapsulePlayer : MonoBehaviour
    {
        public enum State
        {
            Idle = 0,   // 대기
            Left,       // 왼쪽 이동
            Right       // 오른쪽 이동
        }

        public GameObject[] SunGlasses;

        [Header("Settings")]
        public int maxJumpCount = 2;
        public float speed = 2.0f;
        public float jumpPower = 3.0f;
        public float groundCheckDistance = 0.5f;  // 바닥 검사 길이. 짧으면 짧을수록 디테일해짐
        public float skinWidth = 0.02f; // 캐릭터의 주위에 박스를 만들기위해 보정 용도 사용하는 변수
        public float gravity = -9.81f; // 중력 가속도(-9.81 = 현실세계)
        public LayerMask collisionLayer;

        private State currentState = State.Idle;
        private Rigidbody2D rBody;
        private Collider2D col;
        private float XVector;
        private float YVector;
        private float verticalVelocity = 0.0f; // 실시간 수직 가속 운동 값
        private bool isGrounded = false; // 땅에 닿았는지 체크 하는 용도
        private bool jumpRequested = false;
        private int jumpCount;

        private void Awake()
        {
            rBody = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();

            rBody.bodyType = RigidbodyType2D.Kinematic;
            jumpCount = 0;
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                jumpRequested = true;
                Debug.Log("jump");
            }
        }

        private void FixedUpdate()
        {
            isGrounded = CheckGrounded();
            ModifyMoveVector();
            Move();
        }

        private void SetSunGlassState(State state)
        {
            // 상태가 변할때만 아래 로직 실행
            if (currentState == state) return;

            SunGlasses[(int)currentState].SetActive(false);
            SunGlasses[(int)state].gameObject.SetActive(true);
            currentState = state;
        }

        private void ModifyMoveVector()
        {
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                SetSunGlassState(State.Left);
                XVector = -1 * speed;
            }
            else if (Keyboard.current.rightArrowKey.isPressed)
            {
                SetSunGlassState(State.Right);
                XVector = speed;
            }
            else
            {
                SetSunGlassState(State.Idle);
                XVector = 0.0f;
            }

            ApplyJump();

            // r키로 리셋
            if (Keyboard.current.rKey.isPressed) transform.position = new Vector3(-8.65f, -1.08f, 0);

        }

        private void Move()
        {
            Vector3 virtualVector;
            virtualVector = new Vector3(XVector, YVector + ApplyGravaity(), 0);

            Vector3 actualVector = virtualVector;
            actualVector.x = ResolveAxisMovement(actualVector.x, Vector3.right);
            actualVector.y = ResolveAxisMovement(actualVector.y, Vector3.up);

            Vector3 moveVector = actualVector * Time.fixedDeltaTime;

            // 내 위치와 이동량 더해주기
            rBody.MovePosition(transform.position + moveVector);
        }

        private void ApplyJump()
        {
            if (jumpRequested == false) return;
            jumpRequested = false; //우편함을 비웁니다.

            // 점프 횟수 제어
            if (jumpCount >= maxJumpCount) return;

            // 점프 하는 순간 y속도(수직 속도)를 "확" 끌어 올립니다.
            // 이후 ApplyGravity가 매 프레임 이 속도를 깎아서 포물선을 만들도록
            // 유도합니다
            YVector = jumpPower;
            jumpCount++;
            isGrounded = false; // 바닥에서 떨어졌음을 설정합니다.
        }

        private float ApplyGravaity()
        {
            const float groundStickSpeed = -2.0f;

            // 바닥에 있는 상태이면서 위로 올라가는 힘(점프)이 없다면
            if (isGrounded && verticalVelocity <= 0)
            {
                return groundStickSpeed;
            }
            else // 공중에 있는 상태
            {
                // 물리엔진에 영향을 주는 코드라서 Time.fixedDeltaTime을 사용합니다.
                // 속도 = 속도 + 가속도 * 시간 (매 프레임마다 지속적으로 감소 합니다)
                verticalVelocity += gravity * Time.fixedDeltaTime;
                return verticalVelocity;
            }
        }

        // 감지되지 않으면 false 반환
        private bool CheckGrounded()
        {
            Bounds box = GetCastBox();
            RaycastHit2D hit = Physics2D.BoxCast(
                box.center, box.size, 0.0f, Vector2.down, groundCheckDistance, collisionLayer);

            bool result = (hit.collider != null);
            return result;
        }


        // 한축의 이동을 해결하는 함수 (수평과 수직 공용)
        // move : 그 축으로 이동하려는 거리(부호 있음)
        // axis : 방향(Right, Up을 기준으로 동작함)
        // 반환 : 벽/바닥/천장 등에 막히면 줄어든 값을, 안 막히면 그대로의 이동량
        private float ResolveAxisMovement(float move, Vector3 axis)
        {
            if (move == 0.0f) return 0.0f;

            // 부호에 따라 실제로 검사할 방향을 정합니다
            Vector3 dir = move > 0.0f ? axis : -axis;
            float distance = Mathf.Abs(move);
            // Mathf.Abs(값) : 전달된 값을 절대값으로 반환하는 함수입니다.

            // Physics 검사를 진행합니다.
            // BoxCast를 위해서는 Box가 필요한데, 현재 캐릭터는 Capsule Collider를
            // 갖고 있습니다. AABB 검사를 이용하기 위해 임의의 박스를 생성합니다.
            // 우리는 이 Box를 Bounds 부릅니다.

            // 콜라이더보다 살짝 작은 박스를 그 방향으로 쏴 봅니다.
            Bounds box = GetCastBox();
            RaycastHit2D hit = Physics2D.BoxCast(
                box.center, box.size, 0.0f, dir, distance, collisionLayer);

            // 쏴보니까, 아무것도 없다면 원하는 만큼 이동해라
            if (hit.collider == null) return move;

            // 무언가가 있다면, 충돌 지점보다 skinWidth만큼 앞에서 멈춥니다
            float allowed = Mathf.Max(0, hit.distance - skinWidth);

            return Mathf.Sign(move) * allowed;
            // Mathf.Sign(값) 함수는 전달된 값의 부호만을 반환 합니다
        }

        // CastBox를 하기위한 Bounds를 반환하는 함수입니다.
        private Bounds GetCastBox()
        {
            Bounds box = col.bounds; //캡슐콜라이더도 bounds를 갖고 있습니다.

            // 박스 크기를 사방으로 조금씩 줄여줍니다.
            Vector3 size = box.size;
            size.x -= skinWidth * 2;
            size.y -= skinWidth * 2;
            box.size = size;

            return box;
        }


        #region ResolveAxisMovement 시각화 코드

        [Header("캐스트 시각화 설정")]
        // 시각화 표시 on/off
        public bool showCastGizmos = true;

        // 시각화에서 박스를 쏴 볼 거리 (보기 좋게, 실제 이동 거리와는 무관)
        public float castPreviewDistance = 0.5f;

        // 선/박스를 그릴 때 쓰는 1x1 흰색 텍스처
        private static Texture2D whiteTexture;

        // # OnGUI : 매 프레임 화면(게임 뷰)에 GUI를 그리는 이벤트 함수
        private void OnGUI()
        {
            if (showCastGizmos == false) return;

            Camera cam = Camera.main;
            if (cam == null || col == null) return;

            // 캐스트에 쓸 박스(콜라이더보다 살짝 작게)를 여기서 직접 계산합니다.
            Bounds bounds = col.bounds;
            Vector2 boxCenter = bounds.center;
            Vector2 boxSize = (Vector2)bounds.size - Vector2.one * (skinWidth * 2.0f);

            // 상/하/좌/우 네 방향으로 박스를 쏴 보고 그 결과를 그립니다.
            DrawBoxCast(cam, boxCenter, boxSize, Vector2.up);
            DrawBoxCast(cam, boxCenter, boxSize, Vector2.down);
            DrawBoxCast(cam, boxCenter, boxSize, Vector2.left);
            DrawBoxCast(cam, boxCenter, boxSize, Vector2.right);
        }

        // 한 방향으로 BoxCast를 직접 수행하고, 그 모양을 박스로 그립니다.
        private void DrawBoxCast(Camera cam, Vector2 center, Vector2 size, Vector2 dir)
        {
            RaycastHit2D hit = Physics2D.BoxCast(
                center, size, 0.0f, dir, castPreviewDistance, collisionLayer);

            Rect startRect = WorldBoxToGuiRect(cam, center, size);
            Rect endRect = WorldBoxToGuiRect(cam, center + dir * castPreviewDistance, size);

            // 검사 영역(시작~끝 전체) : 노란색
            DrawRectOutline(Encapsulate(startRect, endRect), 2.0f, Color.yellow);
            // 캐스트 시작 박스 : 흰색
            DrawRectOutline(startRect, 2.0f, Color.white);

            if (hit.collider != null)
            {
                // 무언가 걸렸으면 걸린 자리에 빨간 박스
                Rect hitRect = WorldBoxToGuiRect(cam, center + dir * hit.distance, size);
                DrawRectOutline(hitRect, 2.0f, Color.red);
            }
            else
            {
                // 아무것도 없으면 끝까지 간 자리에 초록 박스
                DrawRectOutline(endRect, 2.0f, Color.green);
            }
        }

        // 월드 공간의 축정렬 박스(center, size)를 GUI 좌표 Rect로 변환합니다.
        private Rect WorldBoxToGuiRect(Camera cam, Vector2 center, Vector2 size)
        {
            float z = transform.position.z;
            Vector3 worldMin = new Vector3(center.x - size.x * 0.5f, center.y - size.y * 0.5f, z);
            Vector3 worldMax = new Vector3(center.x + size.x * 0.5f, center.y + size.y * 0.5f, z);

            // 월드 좌표 -> 스크린 좌표 (스크린은 좌하단이 원점)
            Vector3 a = cam.WorldToScreenPoint(worldMin);
            Vector3 b = cam.WorldToScreenPoint(worldMax);

            float left = Mathf.Min(a.x, b.x);
            float right = Mathf.Max(a.x, b.x);
            // GUI 좌표는 좌상단이 원점이고 y가 아래로 증가 -> y를 뒤집어 줍니다.
            float top = Screen.height - Mathf.Max(a.y, b.y);
            float bottom = Screen.height - Mathf.Min(a.y, b.y);

            return new Rect(left, top, right - left, bottom - top);
        }

        // 두 Rect를 모두 감싸는 가장 작은 Rect를 만듭니다.
        private Rect Encapsulate(Rect a, Rect b)
        {
            return Rect.MinMaxRect(
                Mathf.Min(a.xMin, b.xMin), Mathf.Min(a.yMin, b.yMin),
                Mathf.Max(a.xMax, b.xMax), Mathf.Max(a.yMax, b.yMax));
        }

        // 사각형 "테두리"를 얇은 박스 4개(위/아래/왼/오른)로 그립니다.
        private void DrawRectOutline(Rect r, float thickness, Color color)
        {
            if (whiteTexture == null)
            {
                whiteTexture = new Texture2D(1, 1);
                whiteTexture.SetPixel(0, 0, Color.white);
                whiteTexture.Apply();
            }

            Color prev = GUI.color;
            GUI.color = color;

            GUI.DrawTexture(new Rect(r.xMin, r.yMin, r.width, thickness), whiteTexture);              // 위
            GUI.DrawTexture(new Rect(r.xMin, r.yMax - thickness, r.width, thickness), whiteTexture);  // 아래
            GUI.DrawTexture(new Rect(r.xMin, r.yMin, thickness, r.height), whiteTexture);             // 왼쪽
            GUI.DrawTexture(new Rect(r.xMax - thickness, r.yMin, thickness, r.height), whiteTexture); // 오른쪽

            GUI.color = prev;
        }


        #endregion

    }
}