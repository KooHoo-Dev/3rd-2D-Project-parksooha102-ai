using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "ExplosionModuleData", menuName = "Study SAO/ModuleData/Explosion")]
    public class ExplosionModuleData : ModuleInfo
    {
        [Header("Default Values")]
        [SerializeField] private const float DEFAULT_EXPLOSION_RADIUS = 5f;
        [SerializeField] private const float DEFAULT_EXPLOSION_MULTIPLIER = 0.5f;

        [field: SerializeField] public float ExplosionRadius { get; private set; } = 5f;
        [field: SerializeField] public float ExplosionMultiplier { get; private set; } = 0.5f;

        public override IBulletModule CreateModule()
        {
            return new ExplosionModule(this);
        }

        public override void ResetToDefault()
        {
            ExplosionRadius = DEFAULT_EXPLOSION_RADIUS;
            ExplosionMultiplier = DEFAULT_EXPLOSION_MULTIPLIER;
        }
    }
}