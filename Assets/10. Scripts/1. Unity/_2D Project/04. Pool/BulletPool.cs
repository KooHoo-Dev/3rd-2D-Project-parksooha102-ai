using Study.Utilities;
using UnityEngine;

namespace _2DProject
{
    public class BulletPool : MonoBehaviour
    {
        [field: SerializeField] public Bullet bullet { get; private set; }

        static public ComponentPool<Bullet> bulletPool { get; set; }

        private void Awake()
        {
            bulletPool = new ComponentPool<Bullet>(bullet, transform);
        }

        public Bullet Get()
        {
            Bullet bullet = bulletPool.Get();
            return bullet;
        }

    }
}