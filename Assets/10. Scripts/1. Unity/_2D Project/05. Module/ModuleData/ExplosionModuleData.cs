using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "ExplosionModuleData", menuName = "Study SAO/ModuleData/Explosion")]
    public class ExplosionModuleData : ModuleInfo
    {
        [Header("Default Values")]
        private const float DEFAULT_EXPLOSION_RADIUS = 5f;
        private const float MIN_EXPLOSION_RADIUS = 1f;
        private const float MAX_EXPLOSION_RADIUS = 15f;

        private const int DEFAULT_EXPLOSION_DAMAGE = 10;
        private const int MIN_EXPLOSION_DAMAGE = 0;
        private const int MAX_EXPLOSION_DAMAGE = 100;

        [field: SerializeField] public float ExplosionRadius { get; private set; } = 5f;
        [field: SerializeField] public int ExplosionDamage { get; private set; } = 10;

        public void SetExplosionRadius(float value)
        {
            ExplosionRadius = Mathf.Clamp(value, MIN_EXPLOSION_RADIUS, MAX_EXPLOSION_RADIUS);
        }

        public void SetExplosionDamage(int value)
        {
            ExplosionDamage = Mathf.Clamp(value, MIN_EXPLOSION_DAMAGE, MAX_EXPLOSION_DAMAGE);
        }

        public override void ResetToDefault()
        {
            SetExplosionRadius(DEFAULT_EXPLOSION_RADIUS);
            SetExplosionDamage(DEFAULT_EXPLOSION_DAMAGE);
        }

        public override IBulletModule CreateModule()
        {
            return new ExplosionModule(this);
        }

        public override List<ModuleFieldInfo> GetEditableFields()
        {
            List<ModuleFieldInfo> list = new List<ModuleFieldInfo>();

            ModuleFieldInfo radiusField = new ModuleFieldInfo();
            radiusField.Label = "ExplosionRadius";
            radiusField.MinValue = MIN_EXPLOSION_RADIUS;
            radiusField.MaxValue = MAX_EXPLOSION_RADIUS;
            radiusField.CurrentValue = ExplosionRadius;
            radiusField.IsInteger = false;
            list.Add(radiusField);

            ModuleFieldInfo damageField = new ModuleFieldInfo();
            damageField.Label = "ExplosionDamage";
            damageField.MinValue = MIN_EXPLOSION_DAMAGE;
            damageField.MaxValue = MAX_EXPLOSION_DAMAGE;
            damageField.CurrentValue = ExplosionDamage;
            damageField.IsInteger = true;
            list.Add(damageField);

            return list;
        }

        public override void SetFieldValue(string label, float value)
        {
            if (label == "ExplosionRadius")
            {
                SetExplosionRadius(value);
            }
            else if (label == "ExplosionDamage")
            {
                SetExplosionDamage(Mathf.RoundToInt(value));
            }
        }
    }
}