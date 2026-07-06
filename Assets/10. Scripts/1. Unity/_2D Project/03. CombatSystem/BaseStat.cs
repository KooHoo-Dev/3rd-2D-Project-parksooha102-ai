using UnityEngine;

namespace _2DProject
{
    [System.Serializable]
    public class BaseStat
    {
        [field: SerializeField] public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public bool isDead => (Hp <= 0);

        public bool ApplyDamage(int damage)
        {
            Hp = Mathf.Clamp(Hp - damage, 0, MaxHp);
            return isDead;
        }

        public void ApplyHeal(int heal)
        {
            Hp = Mathf.Clamp(Hp + heal, 0, MaxHp);
        }

        public void ResetToFull()
        {
            Hp = MaxHp;
        }
    }

}

