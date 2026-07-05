using System.Collections;
using TMPro;
using UnityEngine;


namespace _2DProject
{
    public class Dummy : CombatEntity
    {
        private static readonly int HIT_EFFECT_BLEND = Shader.PropertyToID("_HitEffectBlend");

        [SerializeField] private float hitEffectDuration = 0.2f;
        [SerializeField] private float resetDelay = 5.0f;
        [SerializeField] private TMP_Text totalDamageText;
        [SerializeField] private TMP_Text DPSText;

        public Animator Animator { get; private set; }

        public static readonly int Attacked = Animator.StringToHash("Attacked");
        public static readonly int Normal = Animator.StringToHash("Normal");

        private const float HIT_EFFECT_VALUE = 0.05f;

        private int accumulatedDamage = 0;
        private float DPS = 0;
        private float firstHitTime;
        private Coroutine resetTimerCoroutine;

        protected SpriteRenderer SpriteRenderer { get; private set; }
        protected Material instanceMaterial { get; set; }

        protected virtual void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();

            instanceMaterial = SpriteRenderer.material;
            Animator.SetBool(Normal, true); 
            UpdateDamageText();
        }

        protected virtual void OnDestroy()
        {
            if (instanceMaterial != null) Destroy(instanceMaterial);
        }

        public void FixedUpdate()
        {
        }

        public override void TakeDamage(int damage)
        {
            if (accumulatedDamage == 0) firstHitTime = Time.time;

            accumulatedDamage += damage;
            Animator.SetTrigger(Attacked);
            StartCoroutine(HitEffectCoroutine());

            float duration = Mathf.Max(Time.time - firstHitTime, 0.01f);
            DPS = accumulatedDamage / duration;

            UpdateDamageText();

            if (resetTimerCoroutine != null) StopCoroutine(resetTimerCoroutine);
            resetTimerCoroutine = StartCoroutine(ResetDamageTimer());
        }


        public override void TakeHeal(int heal)
        {
        }

        protected IEnumerator HitEffectCoroutine()
        {
            float waitTime = 0.0f;

            while (true)
            {
                if (waitTime > hitEffectDuration) break;

                float blendValue = Mathf.Lerp(HIT_EFFECT_VALUE, 0.0f, waitTime / hitEffectDuration);

                instanceMaterial.SetFloat(HIT_EFFECT_BLEND, blendValue);

                yield return null;
                waitTime += Time.deltaTime;
            }

            instanceMaterial.SetFloat(HIT_EFFECT_BLEND, 0.0f);
        }

        private IEnumerator ResetDamageTimer()
        {
            yield return new WaitForSeconds(resetDelay);
            accumulatedDamage = 0;
            DPS = 0;
            resetTimerCoroutine = null;
            UpdateDamageText();
            Debug.Log("누적 데미지 초기화됨");
        }

        private void UpdateDamageText()
        {
            totalDamageText.text = $"Total Damage: {accumulatedDamage}";
            DPSText.text = $"DPS: {DPS:F1}";
        }

    }

}