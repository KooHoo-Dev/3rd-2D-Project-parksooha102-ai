using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "RicochetModuleData", menuName = "Study SAO/ModuleData/Ricochet")]
    public class RicochetModuleData : ModuleInfo
    {
        [Header("Default Values")]
        [SerializeField] private const int DEFAULT_MAX_RICOCHET_COUNT = 3;

        [field: SerializeField] public int MaxRicochetCount { get; private set; } = 3;

        public override IBulletModule CreateModule()
        {
            return new RicochetModule(this);
        }

        public override void ResetToDefault()
        {
            MaxRicochetCount = DEFAULT_MAX_RICOCHET_COUNT;
        }
    }
}