using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "PenetrateModuleData", menuName = "Study SAO/ModuleData/Penetrate")]
    public class PenetrateModuleData : ModuleInfo
    {
        [Header("Default Values")]
        private const int DEFAULT_MAX_PENETRATE_LIMIT = 3;
        private const int MIN_PENETRATE_LIMIT = 0;
        private const int MAX_PENETRATE_LIMIT = 10;

        [field: SerializeField] public int MaxPenetrateLimit { get; private set; } = 3;

        // 외부(UI)에서는 이 메서드를 통해서만 값을 바꿀 수 있음 (범위 검증 포함)
        public void SetMaxPenetrateCount(int value)
        {
            MaxPenetrateLimit = Mathf.Clamp(value, MIN_PENETRATE_LIMIT, MAX_PENETRATE_LIMIT);
        }

        public override void ResetToDefault()
        {
            SetMaxPenetrateCount(DEFAULT_MAX_PENETRATE_LIMIT);
        }

        public override IBulletModule CreateModule()
        {
            return new PenetrateModule(this);
        }

        public override List<ModuleFieldInfo> GetEditableFields()
        {
            List<ModuleFieldInfo> list = new List<ModuleFieldInfo>();
            ModuleFieldInfo limitField = new ModuleFieldInfo();

            limitField.Label = "MaxPenetrateLimit";
            limitField.MinValue = MIN_PENETRATE_LIMIT;
            limitField.MaxValue = MAX_PENETRATE_LIMIT;
            limitField.CurrentValue = MaxPenetrateLimit;
            limitField.IsInteger = true;

            list.Add(limitField);

            return list;
        }

        public override void SetFieldValue(string label, float value)
        {
            if (label == "MaxPenetrateLimit")
            {
                SetMaxPenetrateCount(Mathf.RoundToInt(value));
            }
        }
    }
}