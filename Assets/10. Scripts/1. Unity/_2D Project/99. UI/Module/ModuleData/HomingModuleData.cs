using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "HomingModuleData", menuName = "Study SAO/ModuleData/Homing")]
    public class HomingModuleData : ModuleInfo
    {
        [Header("Default Values")]
        [SerializeField] private const float DEFAULT_TRACKING_RANGE = 10f;
        [SerializeField] private const float DEFAULT_TRACKING_ANGLE = 45f;
        [SerializeField] private const float DEFAULT_TRACKING_SPEED = 4f;

        [field: SerializeField] public float TrackingRange { get; private set; } = 10f;
        [field: SerializeField] public float TrackingAngle { get; private set; } = 45f;
        [field: SerializeField] public float TrackingSpeed { get; private set; } = 4f;

        public override IBulletModule CreateModule()
        {
            return new HomingModule(this);
        }

        public override void ResetToDefault()
        {
            TrackingRange = DEFAULT_TRACKING_RANGE;
            TrackingAngle = DEFAULT_TRACKING_ANGLE;
            TrackingSpeed = DEFAULT_TRACKING_SPEED;
        }
    }
}