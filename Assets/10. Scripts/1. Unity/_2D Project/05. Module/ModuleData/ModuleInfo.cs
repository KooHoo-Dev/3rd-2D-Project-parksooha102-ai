using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    public class ModuleFieldInfo
    {
        public string Label;
        public float MinValue;
        public float MaxValue;
        public float CurrentValue;
        public bool IsInteger;
    }

    [CreateAssetMenu(fileName = "ModuleInfo", menuName = "Study SAO/ModuleInfo", order = 1)]
    public abstract class ModuleInfo : ScriptableObject
    {
        public int Key;
        public string Name;
        public string Description;
        public Sprite Icon;

        [Header("Slot Behavior")]
        public bool IsFixed = false;

        public abstract IBulletModule CreateModule();
        public abstract void ResetToDefault();

        public abstract List<ModuleFieldInfo> GetEditableFields();

        public abstract void SetFieldValue(string label, float value);
    }
}