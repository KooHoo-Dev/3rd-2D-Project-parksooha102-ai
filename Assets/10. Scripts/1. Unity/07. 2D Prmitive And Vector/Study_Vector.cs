using UnityEngine;
using UnityEngine.InputSystem;

namespace Study.PrimitiveAndVector
{
    public class Study_Vector : MonoBehaviour
    {

        private void Start()
        {
            Vector();
        }

        public void Vector()
        {
            // Vector의 함연산
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                transform.position += Vector3.one;
            }
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                Vector3 angle = new Vector3(0, 0, 30);
                transform.rotation *= Quaternion.Euler(angle);
            }
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                transform.localScale += Vector3.one;
            }
        }



    }
}