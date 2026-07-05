using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "BoomerangModuleData", menuName = "Study SAO/ModuleData/Boomerang")]
    public class BoomerangModuleData : ModuleInfo
    {
        [Header("Default Values")]
        private const float DEFAULT_RETURN_DISTANCE = 10.0f;
        private const float MIN_RETURN_DISTANCE = 1.0f;
        private const float MAX_RETURN_DISTANCE = 30.0f;

        private const float DEFAULT_KILL_DISTANCE = 0.5f;
        private const float MIN_KILL_DISTANCE = 0.1f;
        private const float MAX_KILL_DISTANCE = 3.0f;

        [field: SerializeField] public float ReturnDistance { get; private set; } = 10.0f;
        [field: SerializeField] public float KillDistance { get; private set; } = 0.5f;

        public void SetReturnDistance(float value)
        {
            ReturnDistance = Mathf.Clamp(value, MIN_RETURN_DISTANCE, MAX_RETURN_DISTANCE);
        }

        public void SetKillDistance(float value)
        {
            KillDistance = Mathf.Clamp(value, MIN_KILL_DISTANCE, MAX_KILL_DISTANCE);
        }

        public override void ResetToDefault()
        {
            SetReturnDistance(DEFAULT_RETURN_DISTANCE);
            SetKillDistance(DEFAULT_KILL_DISTANCE);
        }

        public override IBulletModule CreateModule()
        {
            return new BoomerangModule(this);
        }

        public override List<ModuleFieldInfo> GetEditableFields()
        {
            List<ModuleFieldInfo> list = new List<ModuleFieldInfo>();

            ModuleFieldInfo returnField = new ModuleFieldInfo();
            returnField.Label = "ReturnDistance";
            returnField.MinValue = MIN_RETURN_DISTANCE;
            returnField.MaxValue = MAX_RETURN_DISTANCE;
            returnField.CurrentValue = ReturnDistance;
            returnField.IsInteger = false;
            list.Add(returnField);

            ModuleFieldInfo killField = new ModuleFieldInfo();
            killField.Label = "KillDistance";
            killField.MinValue = MIN_KILL_DISTANCE;
            killField.MaxValue = MAX_KILL_DISTANCE;
            killField.CurrentValue = KillDistance;
            killField.IsInteger = false;
            list.Add(killField);

            return list;
        }

        public override void SetFieldValue(string label, float value)
        {
            if (label == "ReturnDistance")
            {
                SetReturnDistance(value);
            }
            else if (label == "KillDistance")
            {
                SetKillDistance(value);
            }
        }
    }
}