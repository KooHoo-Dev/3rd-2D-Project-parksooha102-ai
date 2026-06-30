using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace _2DProject
{
    public class Dummy : CombatEntity
    {
        private static readonly int HIT_EFFECT_BLEND = Shader.PropertyToID("_HitEffectBlend");

        [SerializeField] private float hitEffectDuration = 0.2f;
        private const float HIT_EFFECT_VALUE = 0.05f;

        public Animator Animator { get; private set; }

        public static readonly int Attacked = Animator.StringToHash("Attacked");
        public static readonly int Normal = Animator.StringToHash("Normal");

        protected SpriteRenderer SpriteRenderer { get; private set; }
        protected Material instanceMaterial { get; set; }

        protected virtual void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();

            instanceMaterial = SpriteRenderer.material;
            Animator.SetBool(Normal, true);
        }

        protected virtual void OnDestory()
        {
            if (instanceMaterial != null) Destroy(instanceMaterial);
        }

        public override void TakeDamage(int damage)
        {
            Debug.Log("더미 피격!");
            Animator.SetTrigger(Attacked);
            StartCoroutine(HitEffectCoroutine());
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

    }
}