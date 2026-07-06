using UnityEngine;

namespace _2DProject
{
    [RequireComponent(typeof(Collider2D))]
    public class HurtBox : MonoBehaviour
    {
        [field: SerializeField] public CombatEntity Owner { get; private set; }
        private Collider2D col2D;

        private void Awake()
        {
            Owner = GetComponent<CombatEntity>();
            if (Owner == null) 
                Owner = GetComponentInParent<CombatEntity>();

            col2D = GetComponent<Collider2D>();
        }

        private void Start()
        {
            CombatSystem.Instance.AddHurtBox(col2D, this);
        }

        private void OnDestroy()
        {
            if (CombatSystem.Instance != null)
                CombatSystem.Instance.RemoveHurtBox(col2D);
        }
    }
}