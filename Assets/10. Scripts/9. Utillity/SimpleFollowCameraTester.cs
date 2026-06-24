using Study_ActionPlatformer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Study.Utilities
{
    [RequireComponent(typeof(SimpleFollowCamera))]
    public class SimpleFollowCameraTester : MonoBehaviour
    {
        private SimpleFollowCamera followCam;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            followCam = GetComponent<SimpleFollowCamera>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (SimpleInput.GetKeyDown(Key.F1))
            {
                Debug.Log("Set Main Camera Stopped");
                followCam.ChangeState(SimpleFollowCamera.States.Stopped);
            }
            else if (SimpleInput.GetKeyDown(Key.F2))
            {
                Debug.Log("Set Main Camera Holding");
                followCam.ChangeState(SimpleFollowCamera.States.Holding);
            }
            else if (SimpleInput.GetKeyDown(Key.F3))
            {
                followCam.Target = Player.LocalPlayer.transform;
                Debug.Log("Set Main Camera Following");
                followCam.ChangeState(SimpleFollowCamera.States.Following);
            }
            else if (SimpleInput.GetKeyDown(Key.F4))
            {
                Debug.Log("Set Main Camera Following");
                followCam.ChangeState(SimpleFollowCamera.States.Following);
            }
        }
    }
}