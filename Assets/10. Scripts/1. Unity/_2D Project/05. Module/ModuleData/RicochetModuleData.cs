using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "RicochetModuleData", menuName = "Study SAO/ModuleData/Ricochet")]
    public class RicochetModuleData : ModuleInfo
    {
        [Header("Default Values")]
        private const int DEFAULT_MAX_RICOCHET_COUNT = 3;
        private const int MIN_RICOCHET_COUNT = 0;
        private const int MAX_RICOCHET_COUNT = 10;

        [field: SerializeField] public int MaxRicochetCount { get; private set; } = 3;

        public void SetMaxRicochetCount(int value)
        {
            MaxRicochetCount = Mathf.Clamp(value, MIN_RICOCHET_COUNT, MAX_RICOCHET_COUNT);
        }

        public override void ResetToDefault()
        {
            SetMaxRicochetCount(DEFAULT_MAX_RICOCHET_COUNT);
        }

        public override IBulletModule CreateModule()
        {
            return new RicochetModule(this);
        }

        public override List<ModuleFieldInfo> GetEditableFields()
        {
            List<ModuleFieldInfo> list = new List<ModuleFieldInfo>();
            ModuleFieldInfo countField = new ModuleFieldInfo();

            countField.Label = "MaxRicochetCount";
            countField.MinValue = MIN_RICOCHET_COUNT;
            countField.MaxValue = MAX_RICOCHET_COUNT;
            countField.CurrentValue = MaxRicochetCount;
            countField.IsInteger = true;

            list.Add(countField);

            return list;
        }

        public override void SetFieldValue(string label, float value)
        {
            if (label == "MaxRicochetCount")
            {
                SetMaxRicochetCount(Mathf.RoundToInt(value));
            }
        }
    }
}