using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "HomingModuleData", menuName = "Study SAO/ModuleData/Homing")]
    public class HomingModuleData : ModuleInfo
    {
        [Header("Default Values")]
        private const float DEFAULT_TRACKING_RANGE = 10f;
        private const float MIN_TRACKING_RANGE = 1f;
        private const float MAX_TRACKING_RANGE = 20f;

        private const float DEFAULT_TRACKING_ANGLE = 45f;
        private const float MIN_TRACKING_ANGLE = 0f;
        private const float MAX_TRACKING_ANGLE = 180f;

        private const float DEFAULT_TRACKING_SPEED = 4f;
        private const float MIN_TRACKING_SPEED = 0.5f;
        private const float MAX_TRACKING_SPEED = 15f;

        [field: SerializeField] public float TrackingRange { get; private set; } = 10f;
        [field: SerializeField] public float TrackingAngle { get; private set; } = 45f;
        [field: SerializeField] public float TrackingSpeed { get; private set; } = 4f;

        public void SetTrackingRange(float value)
        {
            TrackingRange = Mathf.Clamp(value, MIN_TRACKING_RANGE, MAX_TRACKING_RANGE);
        }

        public void SetTrackingAngle(float value)
        {
            TrackingAngle = Mathf.Clamp(value, MIN_TRACKING_ANGLE, MAX_TRACKING_ANGLE);
        }

        public void SetTrackingSpeed(float value)
        {
            TrackingSpeed = Mathf.Clamp(value, MIN_TRACKING_SPEED, MAX_TRACKING_SPEED);
        }

        public override void ResetToDefault()
        {
            SetTrackingRange(DEFAULT_TRACKING_RANGE);
            SetTrackingAngle(DEFAULT_TRACKING_ANGLE);
            SetTrackingSpeed(DEFAULT_TRACKING_SPEED);
        }

        public override IBulletModule CreateModule()
        {
            return new HomingModule(this);
        }

        public override List<ModuleFieldInfo> GetEditableFields()
        {
            List<ModuleFieldInfo> list = new List<ModuleFieldInfo>();

            ModuleFieldInfo rangeField = new ModuleFieldInfo();
            rangeField.Label = "TrackingRange";
            rangeField.MinValue = MIN_TRACKING_RANGE;
            rangeField.MaxValue = MAX_TRACKING_RANGE;
            rangeField.CurrentValue = TrackingRange;
            rangeField.IsInteger = false;
            list.Add(rangeField);

            ModuleFieldInfo angleField = new ModuleFieldInfo();
            angleField.Label = "TrackingAngle";
            angleField.MinValue = MIN_TRACKING_ANGLE;
            angleField.MaxValue = MAX_TRACKING_ANGLE;
            angleField.CurrentValue = TrackingAngle;
            angleField.IsInteger = false;
            list.Add(angleField);

            ModuleFieldInfo speedField = new ModuleFieldInfo();
            speedField.Label = "TrackingSpeed";
            speedField.MinValue = MIN_TRACKING_SPEED;
            speedField.MaxValue = MAX_TRACKING_SPEED;
            speedField.CurrentValue = TrackingSpeed;
            speedField.IsInteger = false;
            list.Add(speedField);

            return list;
        }

        public override void SetFieldValue(string label, float value)
        {
            if (label == "TrackingRange")
            {
                SetTrackingRange(value);
            }
            else if (label == "TrackingAngle")
            {
                SetTrackingAngle(value);
            }
            else if (label == "TrackingSpeed")
            {
                SetTrackingSpeed(value);
            }
        }
    }
}