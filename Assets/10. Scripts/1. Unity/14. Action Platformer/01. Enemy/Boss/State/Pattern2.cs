using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Study_ActionPlatformer
{
    public class Pattern2 : BossPatternState
    {
        // 오른 손은 플레이어의 x값 따라감 y값 고정
        // 왼쪽 손은 x값 고정 플레이어의 y값 따라감

        [System.Serializable] // 인스펙터에 노출이 가능하게 해주는 키워드
        public class Hand
        {
            [Header("Settings")]
            public bool isLeft = false;
            public Vector3 offset = Vector3.zero;

            [Header("Ref")]
            public Transform handTransform;
            public GameObject normal;
            public GameObject attack;
            public BossBullet bullet;
            public Transform axis;

            public void SetNormal(Vector3 originpos)
            {
                normal.SetActive(true);
                attack.SetActive(false);
                bullet.gameObject.SetActive(false);

                handTransform.position = originpos;
            }

            public void SetAttack(int pattern2Damage)
            {

                normal.SetActive(false);
                attack.SetActive(true);
                bullet.gameObject.SetActive(true);

                bullet.Set(pattern2Damage);
            }

        }

        [SerializeField] private Hand leftHand;
        [SerializeField] private Hand rightHand;


        [SerializeField] private float castingTime = 5.0f;
        [SerializeField] private int damage = 10;

        private Vector3 leftHandOrigin;
        private Vector3 rightHandOrigin;

        public override void Initialize(BossController controller)
        {
            base.Initialize(controller);
            leftHandOrigin = leftHand.handTransform.position;
            rightHandOrigin = rightHand.handTransform.position;
        }

        protected override void OnDisable()
        {
            leftHand.SetNormal(leftHandOrigin);
            rightHand.SetNormal(rightHandOrigin);
        }

        protected override void OnEnable()
        {
            leftHand.SetAttack(damage);
            rightHand.SetAttack(damage);
            StartCoroutine(Coroutine());
        }

        private IEnumerator Coroutine()
        {
            Transform target = Player.LocalPlayer.transform;
            float waitTime = 0.0f;

            while(waitTime < castingTime)
            {
                UpdateHand(leftHand, target);
                UpdateHand(rightHand, target);

                yield return null;
                waitTime += Time.deltaTime;
            }

            rightHand.bullet.Fire();
            leftHand.bullet.Fire();

            BossController.ChangeState(typeof(IdleState));
        }

        private void UpdateHand(Hand hand, Transform target)
        {
            Vector3 handPosition = hand.handTransform.position;

            if (hand.isLeft)
            {
                handPosition.x = hand.axis.position.x;
                handPosition.y = target.transform.position.y;
            }
            else
            {
                handPosition.y = hand.axis.position.y;
                handPosition.x = target.transform.position.x;
            }

            hand.handTransform.position = handPosition;
        }

    }
}