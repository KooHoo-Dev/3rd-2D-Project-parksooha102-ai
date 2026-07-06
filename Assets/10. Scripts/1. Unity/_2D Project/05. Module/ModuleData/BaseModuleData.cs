using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    [CreateAssetMenu(fileName = "BaseModuleData", menuName = "Study SAO/ModuleData/Base")]
    public class BaseModuleData : ModuleInfo
    {
        [Header("Default Values")]
        private const int DEFAULT_DAMAGE = 50;
        private const int MIN_DAMAGE = 1;
        private const int MAX_DAMAGE = 200;

        private const float DEFAULT_BULLET_SPEED = 15f;
        private const float MIN_BULLET_SPEED = 1f;
        private const float MAX_BULLET_SPEED = 40f;

        [field: SerializeField] public int Damage { get; private set; } = 50;
        [field: SerializeField] public float BulletSpeed { get; private set; } = 15f;

        public void SetDamage(int value)
        {
            Damage = Mathf.Clamp(value, MIN_DAMAGE, MAX_DAMAGE);
        }

        public void SetBulletSpeed(float value)
        {
            BulletSpeed = Mathf.Clamp(value, MIN_BULLET_SPEED, MAX_BULLET_SPEED);
        }

        public override void ResetToDefault()
        {
            SetDamage(DEFAULT_DAMAGE);
            SetBulletSpeed(DEFAULT_BULLET_SPEED);
        }

        public override IBulletModule CreateModule()
        {
            return new BaseModule(this);
        }

        public override List<ModuleFieldInfo> GetEditableFields()
        {
            List<ModuleFieldInfo> list = new List<ModuleFieldInfo>();

            ModuleFieldInfo damageField = new ModuleFieldInfo();
            damageField.Label = "Damage";
            damageField.MinValue = MIN_DAMAGE;
            damageField.MaxValue = MAX_DAMAGE;
            damageField.CurrentValue = Damage;
            damageField.IsInteger = true;
            list.Add(damageField);

            ModuleFieldInfo speedField = new ModuleFieldInfo();
            speedField.Label = "BulletSpeed";
            speedField.MinValue = MIN_BULLET_SPEED;
            speedField.MaxValue = MAX_BULLET_SPEED;
            speedField.CurrentValue = BulletSpeed;
            speedField.IsInteger = false;
            list.Add(speedField);

            return list;
        }

        public override void SetFieldValue(string label, float value)
        {
            if (label == "Damage")
            {
                SetDamage(Mathf.RoundToInt(value));
            }
            else if (label == "BulletSpeed")
            {
                SetBulletSpeed(value);
            }
        }
    }
}