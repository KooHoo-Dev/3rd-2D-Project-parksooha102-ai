using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "BoomerangModuleData", menuName = "Study SAO/ModuleData/Boomerang")]
    public class BoomerangModuleData : ModuleInfo
    {
        [Header("Default Values")]
        [SerializeField] private const float DEFAULT_RETURN_DISTANCE = 10.0f;
        [SerializeField] private const float DEFAULT_KILL_DISTANCE = 0.5f;

        [field: SerializeField] public float ReturnDistance { get; private set; } = 10.0f;
        [field: SerializeField] public float KillDistance { get; private set; } = 0.5f;

        public override IBulletModule CreateModule()
        {
            return new BoomerangModule(this);
        }

        public override void ResetToDefault()
        {
            ReturnDistance = DEFAULT_RETURN_DISTANCE;
            KillDistance = DEFAULT_KILL_DISTANCE;
        }
    }
}