using System;
using System.Collections.Generic;
using UnityEngine;

namespace Study_ActionPlatformer
{
    public class BossController : MonoBehaviour
    {
        // MonoBehaviour를 상속받은 FSM을 만들어보자
        // 게임 오브젝트, 상태클래스 구조를 가지고 있음
        private Dictionary<Type, BossBaseState> StateDic { get; set; } = new();
        private BossBaseState CurrentState { get; set; }
        private SpriteRenderer[] allSpriteRenderrer;

        private void Awake()
        {
            // 비활성화된 SpriteRenderrer도 넣어주자
            allSpriteRenderrer = GetComponentsInChildren<SpriteRenderer>(true);

            // Bose GameObject 하위의 모든 상태 클래스들을 찾아주기
            BossBaseState[] allStates = GetComponentsInChildren<BossBaseState>(true);

            for (int i = 0; i < allStates.Length; i++)
            {
                BossBaseState state = allStates[i];
                StateDic.Add(state.GetType(), state);   // state에 정의된 타입을 가져와서 저장하기
                // .GetType
                // 현재 객체의 타입을 반환 현재 인스턴스의 정확한 런타임 타입을 반환
                // BossBaseState를 상속받은 Idle이 있다면 Idle타입을 반환할 수 있다
                state.Initialize(this);
                state.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            ChangeState(typeof(BossPatternState));
        }

        public void ChangeState(Type stateType)
        {
            if (StateDic.TryGetValue(stateType, out BossBaseState nextState) == false)
            {
                Debug.LogError($"등록되지 않은 상태입니다: {stateType.Name}");
                return;
            }

            if (CurrentState != null)
            {
                CurrentState.gameObject.SetActive(false);
            }
            CurrentState = nextState;
            CurrentState.gameObject.SetActive(true);
        }

        public void ChangeAlpha(float alphaValue)
        {
            for (int i = 0; i < allSpriteRenderrer.Length; i++)
            {
                // Color는 struct(value) 타입이기 때문에 하나만 수정하더라도 Vector처럼 모든 값을 대입해야함
                Color color = allSpriteRenderrer[i].color;
                color.a = alphaValue;
                allSpriteRenderrer[i].color = color;
            }
        }

        public void Dead()
        {
            ChangeState(typeof(DeadState));
        }


    }
}