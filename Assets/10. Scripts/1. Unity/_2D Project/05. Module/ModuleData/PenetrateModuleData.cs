using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "PenetrateModuleData", menuName = "Study SAO/ModuleData/Penetrate")]
    public class PenetrateModuleData : ModuleInfo
    {
        [Header("Default Values")]
        [SerializeField] private const int DEFAULT_MAX_PENETRATE_COUNT = 3;

        [field: SerializeField] public int MaxPenetrateCount { get; private set; } = 3;

        public override IBulletModule CreateModule()
        {
            return new PenetrateModule(this);
        }

        public override void ResetToDefault()
        {
            MaxPenetrateCount = DEFAULT_MAX_PENETRATE_COUNT;
        }
    }
}