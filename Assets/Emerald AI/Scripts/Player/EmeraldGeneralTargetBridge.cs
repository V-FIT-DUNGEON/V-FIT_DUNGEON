using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EmeraldAI.Utility;
using Kryz.CharacterStats.Examples;

namespace EmeraldAI
{
    [RequireComponent(typeof(TargetPositionModifier))]
    [RequireComponent(typeof(FactionExtension))]
    [HelpURL("https://black-horizon-studios.gitbook.io/emerald-ai-wiki/getting-started/setting-up-a-player-with-emerald-ai")]
    public class EmeraldGeneralTargetBridge : MonoBehaviour, IDamageable, ICombat
    {
        public int StartingHealth = 50;
        public bool Immortal = false;
        public UnityEvent OnTakeDamage;
        public UnityEvent OnDeath;
        
        public bool DebugLogDeath = true;
        public bool HideSettingsFoldout;
        public bool HealthSettingsFoldout = true;

        public int StartHealth { get => StartingHealth; set => StartingHealth = value; }
        [field: SerializeField] public int Health { get; set; }
        [field: SerializeField] public List<string> ActiveEffects { get; set; }

        private Character character;
        private float healRate;
        private Coroutine regenCoroutine;
        private float lastDamageTime;
        private int previousvitality;

        TargetPositionModifier m_TargetPositionModifier;
        Collider m_Collider;

        void Start()
        {
            character = GetComponent<Character>();
            previousvitality = (int)character.Vitality.Value;
            StartingHealth += previousvitality;
            Health = StartingHealth; // Initialize health with vitality bonus
            healRate = 1 + (0.01f * character.Vitality.Value); // Heal rate formula

            m_TargetPositionModifier = GetComponent<TargetPositionModifier>();
            m_Collider = GetComponent<Collider>();
        }

        void Update()
        {
            if (previousvitality != (int)character.Vitality.Value && character.Vitality.Value >= 0)
            {
                StartingHealth = 200; // Reset base health
                previousvitality = (int)character.Vitality.Value;
                StartingHealth += previousvitality; // Update health with new vitality
                Health = StartingHealth; // Reset health to max
            }
        }

        public void Damage(int DamageAmount, Transform AttackerTransform = null, int RagdollForce = 100, bool CriticalHit = false)
        {
            DefaultDamage(DamageAmount, AttackerTransform);

            if (CombatTextSystem.Instance != null)
                CombatTextSystem.Instance.CreateCombatText(DamageAmount, DamagePosition(), CriticalHit, false, false);
        }

        void DefaultDamage(int DamageAmount, Transform Target)
        {
            if (Immortal) return;

            Health -= DamageAmount;
            OnTakeDamage.Invoke();
            lastDamageTime = Time.time; // Reset damage timer

            if (regenCoroutine != null)
                StopCoroutine(regenCoroutine); // Stop regen if hit

            regenCoroutine = StartCoroutine(RegenerateHealth());

            if (Health <= 0)
            {
                if (DebugLogDeath)
                    Debug.Log("The Non-AI Target has died.");

                if (m_Collider != null) m_Collider.enabled = false;
                gameObject.layer = 0;
                gameObject.tag = "Untagged";
                OnDeath.Invoke();
            }
        }

        IEnumerator RegenerateHealth()
        {
            yield return new WaitForSeconds(10); // Wait 10 seconds after last damage

            while (Health < StartingHealth)
            {
                if (Time.time - lastDamageTime < 10) yield break; // Stop if damaged again
                
                Health = Mathf.Min(Health + Mathf.CeilToInt(healRate), StartingHealth); // Apply healing
                yield return new WaitForSeconds(1); // Heal every second
            }
        }

        public void ResetTarget()
        {
            Health = StartingHealth;
            if (m_Collider != null) m_Collider.enabled = true;
        }

        public Vector3 DamagePosition()
        {
            if (m_TargetPositionModifier != null)
                return new Vector3(m_TargetPositionModifier.TransformSource.position.x, 
                                   m_TargetPositionModifier.TransformSource.position.y + m_TargetPositionModifier.PositionModifier, 
                                   m_TargetPositionModifier.TransformSource.position.z);
            else
                return transform.position + new Vector3(0, transform.localScale.y / 2, 0);
        }

        public Transform TargetTransform() => transform;

        public bool IsAttacking() => false;
        public bool IsBlocking() => false;
        public bool IsDodging() => false;
        public void TriggerStun(float StunLength) { }
    }
}
