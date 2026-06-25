using Study.Utilities;
using UnityEngine;

namespace _2DProject
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField] private Transform AimPoint;
        [field: SerializeField] private Transform PlayerPosition;
        [field: SerializeField] public Bullet bullet { get; private set; }

        static public ComponentPool<Bullet> bulletPool { get; set; }

        private TopDownCharactorController Controller { get; set; }
        public Transform PoolRoot;

        private void Awake()
        {
            Controller = GetComponent<TopDownCharactorController>();
            bulletPool = new ComponentPool<Bullet>(bullet, PoolRoot);

        }

        private void Update()
        {
            UpdatePosition();
            UpdateShotInput();
            
        }

        private void UpdatePosition()
        {
            Vector3 goalPosition = AimPoint.position;

            Vector3 myPosition = PlayerPosition.position;

            Vector3 dir = (goalPosition - myPosition).normalized;
            PlayerPosition.up = dir;

            Vector2 inputVector = SimpleInput.GetMoveAxisRaw();
            Controller.SetMoveInput(inputVector);
        }

        private void UpdateShotInput()
        {
            if (SimpleInput.GetMouseButtonDown(0))
            {
                Debug.Log("fire");
                Vector3 direction = PlayerPosition.up;
                Bullet bullet = bulletPool.Get();
                bullet.transform.up = direction;
                bullet.Set(50);
                bullet.Fire();
            }
        }

    }
}