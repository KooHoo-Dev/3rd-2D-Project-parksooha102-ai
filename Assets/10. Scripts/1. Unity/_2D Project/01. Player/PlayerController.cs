using Study.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _2DProject
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField] private Transform AimPoint;
        [field: SerializeField] private Transform Player;

        private TopDownCharactorController Controller { get; set; }

        private void Awake()
        {
            Controller = GetComponent<TopDownCharactorController>();

        }

        private void Update()
        {
            Vector3 goalPosition = AimPoint.position;

            Vector3 myPosition = Player.position;

            Vector3 dir = (goalPosition - myPosition).normalized;
            Player.up = dir;

            Vector2 inputVector = SimpleInput.GetMoveAxisRaw();
            Controller.SetMoveInput(inputVector);
        }


    }
}
