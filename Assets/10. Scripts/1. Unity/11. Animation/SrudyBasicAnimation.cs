using Study.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Study_BasicAnimation
{
    public class Study_BasicAnimation : MonoBehaviour
    {
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        public float speed = 1.0f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            /* SetBool 사용
            bool isWalk = SimpleInput.GetKey(Key.RightArrow) || SimpleInput.GetKey(Key.LeftArrow);
            bool isRun = SimpleInput.GetKey(Key.LeftShift);

            animator.SetBool("IsWalk", isWalk);
            animator.SetBool("IsRun", isRun);*/
            StudyAnimator();
        }

        private void StudyAnimator()
        {
            float moveDirection = SimpleInput.GetAxisHorizontalRaw();
            // moveDirection의 speed를 이용해서 애니메이터에게 일을 시켜보자

            // 캐릭터의 방향 전환과 애니메이션 적용값을 같이 구현
            float absMoveDirection = Mathf.Abs(moveDirection);
            animator.SetFloat("Movement", absMoveDirection);

            if (absMoveDirection > 0 )
            {
                spriteRenderer.flipX = (moveDirection < 0.0f);
            }

            bool isRun = SimpleInput.GetKey(Key.LeftShift);
            animator.SetBool("IsRun", isRun);

            float applySpeed = speed * (isRun ? 2.5f : 1.0f);

            transform.Translate(Vector3.right * moveDirection *  applySpeed * Time.deltaTime);
        }

    }
}