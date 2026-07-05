using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "SplitModuleData", menuName = "Study SAO/ModuleData/Split")]
    public class SplitModuleData : ModuleInfo
    {
        [Header("Default Values")]
        private const int DEFAULT_MAX_GENERATION = 3;
        private const int MIN_MAX_GENERATION = 0;
        private const int MAX_MAX_GENERATION = 5;

        private const int DEFAULT_SPLIT_AMOUNT = 3;
        private const int MIN_SPLIT_AMOUNT = 2;
        private const int MAX_SPLIT_AMOUNT = 8;

        private const float DEFAULT_SPLIT_ANGLE = 45.0f;
        private const float MIN_SPLIT_ANGLE = 0f;
        private const float MAX_SPLIT_ANGLE = 180f;

        private const float DEFAULT_MULTIPLIER = 0.5f;
        private const float MIN_MULTIPLIER = 0.1f;
        private const float MAX_MULTIPLIER = 1.0f;

        [field: SerializeField] public int MaxGeneration { get; private set; } = 3;
        [field: SerializeField] public int SplitAmount { get; private set; } = 3;
        [field: SerializeField] public float SplitAngle { get; private set; } = 45.0f;
        [field: SerializeField] public float Multiplier { get; private set; } = 0.5f;

        public void SetMaxGeneration(int value)
        {
            MaxGeneration = Mathf.Clamp(value, MIN_MAX_GENERATION, MAX_MAX_GENERATION);
        }

        public void SetSplitAmount(int value)
        {
            SplitAmount = Mathf.Clamp(value, MIN_SPLIT_AMOUNT, MAX_SPLIT_AMOUNT);
        }

        public void SetSplitAngle(float value)
        {
            SplitAngle = Mathf.Clamp(value, MIN_SPLIT_ANGLE, MAX_SPLIT_ANGLE);
        }

        public void SetMultiplier(float value)
        {
            Multiplier = Mathf.Clamp(value, MIN_MULTIPLIER, MAX_MULTIPLIER);
        }

        public override void ResetToDefault()
        {
            SetMaxGeneration(DEFAULT_MAX_GENERATION);
            SetSplitAmount(DEFAULT_SPLIT_AMOUNT);
            SetSplitAngle(DEFAULT_SPLIT_ANGLE);
            SetMultiplier(DEFAULT_MULTIPLIER);
        }

        public override IBulletModule CreateModule()
        {
            return new SplitModule(this);
        }

        public override List<ModuleFieldInfo> GetEditableFields()
        {
            List<ModuleFieldInfo> list = new List<ModuleFieldInfo>();

            ModuleFieldInfo generationField = new ModuleFieldInfo();
            generationField.Label = "MaxGeneration";
            generationField.MinValue = MIN_MAX_GENERATION;
            generationField.MaxValue = MAX_MAX_GENERATION;
            generationField.CurrentValue = MaxGeneration;
            generationField.IsInteger = true;
            list.Add(generationField);

            ModuleFieldInfo amountField = new ModuleFieldInfo();
            amountField.Label = "SplitAmount";
            amountField.MinValue = MIN_SPLIT_AMOUNT;
            amountField.MaxValue = MAX_SPLIT_AMOUNT;
            amountField.CurrentValue = SplitAmount;
            amountField.IsInteger = true;
            list.Add(amountField);

            ModuleFieldInfo angleField = new ModuleFieldInfo();
            angleField.Label = "SplitAngle";
            angleField.MinValue = MIN_SPLIT_ANGLE;
            angleField.MaxValue = MAX_SPLIT_ANGLE;
            angleField.CurrentValue = SplitAngle;
            angleField.IsInteger = false;
            list.Add(angleField);

            ModuleFieldInfo multiplierField = new ModuleFieldInfo();
            multiplierField.Label = "Multiplier";
            multiplierField.MinValue = MIN_MULTIPLIER;
            multiplierField.MaxValue = MAX_MULTIPLIER;
            multiplierField.CurrentValue = Multiplier;
            multiplierField.IsInteger = false;
            list.Add(multiplierField);

            return list;
        }

        public override void SetFieldValue(string label, float value)
        {
            if (label == "MaxGeneration")
            {
                SetMaxGeneration(Mathf.RoundToInt(value));
            }
            else if (label == "SplitAmount")
            {
                SetSplitAmount(Mathf.RoundToInt(value));
            }
            else if (label == "SplitAngle")
            {
                SetSplitAngle(value);
            }
            else if (label == "Multiplier")
            {
                SetMultiplier(value);
            }
        }
    }
}