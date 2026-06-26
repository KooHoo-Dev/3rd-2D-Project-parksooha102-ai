using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
        private int wallLayer;

        public bool isModuleA = false;
        public bool isModuleB = false;
        public bool isModuleC = false;
        public bool isModuleD = false;
        public bool isModuleE = false;
        public bool isModuleF = false;

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

        void Start()
        {
            wallLayer = LayerMask.NameToLayer("Wall");
        }

        private void Update()
        {
            if (isFired == false) return;

            if (isModuleB)
            {
                float searchRadius = 20;
                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, searchRadius);
                Collider2D closestTarget = null;
                float minDistance = Mathf.Infinity;

                foreach (var target in targets)
                {
                    if (target.CompareTag(opponentTag))
                    {
                        float distance = (target.transform.position - transform.position).sqrMagnitude;
                        Debug.Log($"{distance} 만큼 떨어져있는 적 발견!");
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestTarget = target;
                        }
                    }
                }
            }

            transform.Translate(moveVector * (speed * Time.deltaTime), Space.Self);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($"{collision.gameObject}에 충돌!");

            if (collision.gameObject.CompareTag(opponentTag))
            {
                if (isModuleC)
                {
                    // 대충 폭발
                }
                if (isModuleD)
                {
                    // 대충 관통
                }
                if (isModuleE)
                {
                    // 대충 분열
                }

                gameObject.SetActive(false);
            }

            if (collision.gameObject.layer == wallLayer)
            {
                if (isModuleA)
                {
                    // 대충 반사
                    Debug.Log($"벽에 충돌!");
                }
                if(isModuleC)
                {
                    // 대충 폭발
                }

                gameObject.SetActive(false);
            }
        }

    }
}