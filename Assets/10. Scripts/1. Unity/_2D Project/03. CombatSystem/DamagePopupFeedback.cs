using Study.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    public class DamagePopupFeedback : MonoBehaviour, ICombatObserver
    {
        [field: SerializeField] public FloatingText PopupText { get; private set; }
        [field: SerializeField] public Color DamageColor { get; private set; }

        private Dictionary<CombatEntity, int> damageBuffer = new Dictionary<CombatEntity, int>();
        private HashSet<CombatEntity> processingEntities = new HashSet<CombatEntity>();

        private ComponentPool<FloatingText> Pool { get; set; }

        private void Awake()
        {
            Pool = new ComponentPool<FloatingText>(PopupText, transform);
        }

        private void OnEnable()
        {
            CombatSystem.Instance.Subscribe(this);
        }

        private void OnDisable()
        {
            if (CombatSystem.Instance != null)
                CombatSystem.Instance.UnSubscribe(this);
        }

        public void OnDamageTaken(CombatEntity sender, CombatEntity receiver, CombatEvent @event)
        {
            if (damageBuffer.ContainsKey(receiver))
                damageBuffer[receiver] += @event.Amount;
            else
                damageBuffer[receiver] = @event.Amount;

            if (!processingEntities.Contains(receiver))
            {
                StartCoroutine(ProcessDamageSummation(receiver));
            }
        }

        public void OnHealTaken(CombatEntity sender, CombatEntity receiver, CombatEvent @event)
        {
        }

        private IEnumerator ProcessDamageSummation(CombatEntity target)
        {
            processingEntities.Add(target);

            yield return new WaitForSeconds(0.1f);

            if (damageBuffer.TryGetValue(target, out int totalAmount))
            {
                SpawnPopup(target, totalAmount);
                damageBuffer.Remove(target);
            }

            processingEntities.Remove(target);
        }

        private void SpawnPopup(CombatEntity target, int totalAmount)
        {
            FloatingText textItem = Pool.Get();
            Color color = DamageColor;

            textItem.Show($"{totalAmount}", color, target.HeadUpPivot.position);
        }

    }
}