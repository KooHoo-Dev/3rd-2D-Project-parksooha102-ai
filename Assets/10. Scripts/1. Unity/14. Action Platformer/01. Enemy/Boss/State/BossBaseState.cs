using UnityEngine;

namespace Study_ActionPlatformer
{
    public abstract class BossBaseState : MonoBehaviour
    {
        protected BossController BossController { get; set; }
        public virtual void Initialize(BossController controller)
        {
            BossController = controller;
        }

        // abstract로 선언하여 자식클래스들이 반드시 구현하도록 강제
        // 단, OnEnable, OnDisable등은 유니티가 직접 호출하는 메시지 함수라서 absract로 강제하는건 트릭에 가깝다

        // 정석은 public absract void EnterState();처럼 직접 호출하는 메서드를 강제하는 것이 정석

        protected abstract void OnEnable();
        protected abstract void OnDisable();
    }

    public abstract class BossPatternState : BossBaseState
    {
        // 묶어둘 수 있는 용도로 선언한 추상 클래스
        // 패턴 상태 검색용으로 사용
    }

}