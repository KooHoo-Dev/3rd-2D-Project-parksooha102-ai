using UnityEngine;

namespace Study_ActionPlatformer
{
    public class BossBullet : MonoBehaviour
    {
        // 1. 특정 방향으로 운동
        // 2. 플레이어와 충돌하면 데미지를 줌(전투이벤트 생성 후 전달)
        // 3. 재사용 가능해야함

        [SerializeField] private Vector3 startLocalPosition;
        [SerializeField] private Vector3 moveVector = Vector3.up;
        [SerializeField] private float speed = 1.0f;
        // 못맞췄을 경우
        [SerializeField] private float lifeTime = 5.0f;
        private float currentTime = 0;
        private bool isFired = false;

        // Bullet 객체를 초기화합니다
        public void Set(int damage)
        {
            isFired = false;
            gameObject.SetActive(true);
            transform.localPosition = startLocalPosition;
        }

        public void Fire() 
        {
            isFired = true;
            currentTime = 0;
        }

        public void Update() 
        {
            if (isFired == false) return;

            currentTime += Time.deltaTime;

            if (currentTime >= lifeTime)
            {
                gameObject.SetActive(false);
                return;
            }

            // 아래는 운동로직
            transform.Translate(moveVector * (speed * Time.deltaTime), Space.Self);
            // Translate(이동량, Space.Self) 내 기준으로 이동
            // Translate(이동량, Space.World) 월드를 기준으로 이동
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // 전투 이벤트 처리로직이 들어가야하나 진도상 생략
                gameObject.SetActive(false);
            }
        }


    }
}