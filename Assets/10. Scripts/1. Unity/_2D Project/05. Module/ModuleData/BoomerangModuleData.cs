using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "BoomerangModuleData", menuName = "Study SAO/ModuleData/Boomerang")]
    public class BoomerangModuleData : ModuleInfo
    {
        private const float DEFAULT_RETURN_DISTANCE = 10.0f;
        private const float DEFAULT_KILL_DISTANCE = 0.5f;

        [Header("Values")]
        [SerializeField] public float ReturnDistance = DEFAULT_RETURN_DISTANCE;
        [SerializeField] public float KillDistance = DEFAULT_KILL_DISTANCE;

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