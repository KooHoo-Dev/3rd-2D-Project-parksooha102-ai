using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "SplitModuleData", menuName = "Study SAO/ModuleData/Split")]
    public class SplitModuleData : ModuleInfo
    {
        [Header("Default Values")]
        [SerializeField] private const int DEFAULT_MAX_GENERATION = 3;
        [SerializeField] private const int DEFAULT_SPLIT_AMOUNT = 3;
        [SerializeField] private const float DEFAULT_SPLIT_ANGLE = 45.0f;
        [SerializeField] private const float DEFAULT_MULTIPLIER = 0.5f;

        [field: SerializeField] public int MaxGeneration { get; private set; } = 3;
        [field: SerializeField] public int SplitAmount { get; private set; } = 3;
        [field: SerializeField] public float SplitAngle { get; private set; } = 45.0f;
        [field: SerializeField] public float Multiplier { get; private set; } = 0.5f;

        public override IBulletModule CreateModule()
        {
            return new SplitModule(this);
        }

        public override void ResetToDefault()
        {
            MaxGeneration = DEFAULT_MAX_GENERATION;
            SplitAmount = DEFAULT_SPLIT_AMOUNT;
            SplitAngle = DEFAULT_SPLIT_ANGLE;
            Multiplier = DEFAULT_MULTIPLIER;
        }
    }
}