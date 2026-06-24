using UnityEngine;
using UnityEngine.Rendering;

namespace Study.Utilities
{
    // 타겟을 부드럽게 따라가는 범용 추적 카메라
    public class SimpleFollowCamera : MonoBehaviour
    {
        public enum States
        {
            Stopped,
            Holding,
            Following,
        }

        [field: SerializeField] public Transform Target { get; set; }
        [SerializeField] private float lerpSpeed;
        [SerializeField] private Vector3 offSet;

        private Camera Cam { get; set; }
        private States State { get; set; }

        private void Start()
        {
            Cam = Camera.main;
        }

        public void Update()
        {
            switch (State)
            {
                case States.Stopped:
                    Target = null;
                    break;
                case States.Holding:
                    break;
                    // 
                case States.Following:
                    Vector3 lerpPos = Vector3.Lerp(transform.position, Target.position + offSet, lerpSpeed * Time.deltaTime);
                    lerpPos.z = transform.position.z;
                    transform.position = lerpPos;
                    break;
            }

        }

        public void ChangeState(States state)
        {
            if(state == States.Following && Target == null)
            {
                Debug.LogWarning("");
            }

            State = state;
        }

    }
}