using Study.Utilities;
using UnityEngine;

namespace _2DProject
{
    public class Bullet : MonoBehaviour
    {
        // 1. 특정 방향으로 운동
        // 2. 적과 충돌하면 데미지를 줌(전투이벤트 생성 후 전달)
        // 3. 재사용 가능해야함

        [SerializeField] private GameObject Owner;
        [SerializeField] private Vector3 startLocalPosition;
        [SerializeField] private Vector3 moveVector = Vector3.up;
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private string opponentTag;

        private bool isFired = false;
        private int Damage;

        // Bullet 객체를 초기화합니다
        public void Set(int damage)
        {
            isFired = false;
            gameObject.SetActive(true);
            transform.localPosition = startLocalPosition + Owner.transform.position;
            Damage = damage;
        }

        public void Fire()
        {
            gameObject.SetActive(true);
            isFired = true;
        }

        private void Update()
        {
            if (isFired == false) return;

            // 아래는 운동로직
            transform.Translate(moveVector * (speed * Time.deltaTime), Space.Self);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(opponentTag))
            {
                // 전투 이벤트 처리로직이 들어가야하나 진도상 생략
                gameObject.SetActive(false);
            }
        }

    }
}