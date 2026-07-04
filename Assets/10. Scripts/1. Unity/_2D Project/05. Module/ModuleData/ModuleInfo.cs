using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "ModuleInfo", menuName = "Study SAO/ModuleInfo", order = 1)]
    public abstract class ModuleInfo : ScriptableObject
    {
        public int Key;
        public string Name;
        public string Description;
        public Sprite Icon;

        public abstract IBulletModule CreateModule();
        public abstract void ResetToDefault();
    }
}