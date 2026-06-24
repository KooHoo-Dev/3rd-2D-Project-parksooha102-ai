using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Study.Utilities
{
    // PC 플랫폼에서 사용하는 키보드, 마우스 인풋을 묶어놓은 유틸 클래스
    // 목표는 쉽고 직관적으로 사용할 수 있도록 기능들을 묶어놓는 것
    // 아래처럼 작업하는 디자인 패턴을 Wrapper 패턴이라고 함

    // SimpleInput 클래스는 어디서든 호출할 수 있는 static 키워드를 사용하여 선언

    public static class SimpleInput
    {

        #region 키보드 입력

        // GetKeyDown, GetKey, GetKeyUp 메서드를 하나로 묶어서 사용하기 위한 메서드
        public static bool GetKeyDown(Key key)
        {
            return Keyboard.current[key].wasPressedThisFrame;
        }
        public static bool GetKeyUp(Key key)
        {
            return Keyboard.current[key].wasReleasedThisFrame;
        }
        public static bool GetKey(Key key)
        {
            return Keyboard.current[key].isPressed;
        }
        public static bool AnyKeyDown()
        {
            return Keyboard.current.anyKey.wasPressedThisFrame;
        }

        #endregion
        #region 마우스 입력

        public enum MouseButton
        {
            Left = 0,
            Right = 1,
            Middle = 2
        }

        private static ButtonControl GetButtonControl(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return Mouse.current.leftButton;
                case MouseButton.Right:
                    return Mouse.current.rightButton;
                case MouseButton.Middle:
                    return Mouse.current.middleButton;
                default:
                    return null;
            }
        }

        public static bool GetMouseButtonDown(MouseButton button)
        {
            return GetButtonControl(button).wasPressedThisFrame;
        }
        public static bool GetMouseButtonUp(MouseButton button)
        {
            return GetButtonControl(button).wasReleasedThisFrame;
        }
        public static bool GetMouseButton(MouseButton button)
        {
            return GetButtonControl(button).isPressed;
        }

        public static Vector2 GetMousePosition()
        {
            return Mouse.current.position.ReadValue();
        }

        /// <summary>
        ///  마우스가 1프레임만에 변한 벡터값을 반환
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetMouseDelta()
        {
            return Mouse.current.delta.ReadValue();
        }

        public static float GetMouseScrollDeltaRaw()
        {
            return Mouse.current.scroll.ReadValue().y;
        }

        // 마우스 스크롤 휠의 방향을 -1, 0, 1로 반환
        public static float GetMouseScrollDelta()
        {
            float raw = Mouse.current.scroll.ReadValue().y;
            return Mathf.Approximately(raw, 0f) ? 0f : Mathf.Sign(raw);
            // Mathf.Approximately 0이랑 비교를 하면 근사치로 일정한 범위 내에 raw가 있으면 0으로 간주
        }

        #endregion

        #region 축 입력 (WASD, 방향키)

        // 수평축 원시 입력값을 반환(-1, 0, 1)
        public static float GetAxisHorizontalRaw()
        {
            float axis = 0f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) axis += 1.0f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) axis -= 1.0f;
            return axis;
        }

        public static float GetAxisVerticalRaw()
        {
            float axis = 0f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) axis += 1.0f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) axis -= 1.0f;
            return axis;
        }

        public static Vector2 GetMoveAxisRaw()
        {
            Vector2 axis = new Vector2();
            axis.x = GetAxisHorizontalRaw();
            axis.y = GetAxisVerticalRaw();

            if (axis.sqrMagnitude > 1.0f) return axis.normalized;
            return axis;
        }

        #endregion
    }
}