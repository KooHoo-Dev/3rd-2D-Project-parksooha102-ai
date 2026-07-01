using Study.Utilities;
using UnityEngine;

namespace _2DProject
{
    public class ExplosionPool : MonoBehaviour
    {
        [field: SerializeField] public ExplosionEffect explosionEffect { get; private set; }

        static public ComponentPool<ExplosionEffect> explosionPool { get; set; }

        private void Awake()
        {
            explosionPool = new ComponentPool<ExplosionEffect>(explosionEffect, transform);
        }

        public ExplosionEffect Get()
        {
            ExplosionEffect explosion = explosionPool.Get();
            return explosion;
        }

    }
}