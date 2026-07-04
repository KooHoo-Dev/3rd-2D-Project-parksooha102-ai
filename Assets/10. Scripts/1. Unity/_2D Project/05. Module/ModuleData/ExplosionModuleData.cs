using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "ExplosionModuleData", menuName = "Study SAO/ModuleData/Explosion")]
    public class ExplosionModuleData : ModuleInfo
    {
        [Header("Default Values")]
        [SerializeField] private const float DEFAULT_EXPLOSION_RADIUS = 5f;
        [SerializeField] private const int DEFAULT_EXPLOSION_DAMAGE = 10;

        [field: SerializeField] public float ExplosionRadius { get; private set; } = 5f;
        [field: SerializeField] public int ExplosionDamage { get; private set; } = 10;

        public override IBulletModule CreateModule()
        {
            return new ExplosionModule(this);
        }

        public override void ResetToDefault()
        {
            ExplosionRadius = DEFAULT_EXPLOSION_RADIUS;
            ExplosionDamage = DEFAULT_EXPLOSION_DAMAGE;
        }
    }
}