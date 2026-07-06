using Study.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

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
            UpdateCursor();

            if (SimpleInput.GetKeyDown(Key.V))
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
            if (SimpleInput.GetKey(Key.Tab))
                return;

            if (SimpleInput.GetMouseButtonDown(0))
            {
                Bullet bullet = BulletFactory.Create(bulletPool, Muzzle.position, transform.up);
            }

            if (SimpleInput.GetMouseButton(0) && isOnFire)
            {
                Bullet bullet = BulletFactory.Create(bulletPool, Muzzle.position, transform.up);
            }
        }

        private void UpdateCursor()
        {
            bool isBuildMode = SimpleInput.GetKey(Key.Tab);

            Cursor.visible = isBuildMode;
        }

    }
}