using Study.Utilities;
using UnityEngine;

namespace _2DProject
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField] private Transform AimPoint;
        [field: SerializeField] private Transform Muzzle;
        [field: SerializeField] public BulletPool bulletPool;

        private TopDownCharactorController Controller { get; set; }
        private bool isOnFire;

        private void Awake()
        {
            Controller = GetComponent<TopDownCharactorController>();
        }

        private void Update()
        {
            UpdatePosition();
            UpdateShotInput();
            if (SimpleInput.GetKeyDown(UnityEngine.InputSystem.Key.V))
            {
                isOnFire = !isOnFire;
            }
        }

        private void UpdatePosition()
        {
            Vector3 goalPosition = AimPoint.position;

            Vector3 myPosition = transform.position;

            Vector3 dir = (goalPosition - myPosition).normalized;
            transform.up = dir;

            Vector2 inputVector = SimpleInput.GetMoveAxisRaw();
            Controller.SetMoveInput(inputVector);
        }

        private void UpdateShotInput()
        {
            if (SimpleInput.GetMouseButtonDown(0))
            {
                Debug.Log("fire");
                Bullet bullet = bulletPool.Get();
                bullet.Set(Muzzle.position, transform.up, 50);
            }

            if (SimpleInput.GetMouseButton(0) && isOnFire)
            {
                Debug.Log("fireing");

                Bullet bullet = bulletPool.Get();
                bullet.Set(Muzzle.position, transform.up, 50);
            }
        }

    }
}