using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats.Examples;

namespace EmeraldAI.Example
{
    public class DamageAIByCollision : MonoBehaviour
    {
        public bool IsTrigger = false;
        public int BaseDamage = 10;
        public int RagdollForceAmount = 50;
        public GameObject PlayerObject;

        private Character playerCharacter;
        private HashSet<GameObject> damagedTargets = new HashSet<GameObject>(); // Store already damaged targets
        private float damageCooldown = 1.5f; // Cooldown time in seconds
        private Coroutine resetCooldownCoroutine;

        private void Start()
        {
            if (PlayerObject != null)
            {
                playerCharacter = PlayerObject.GetComponent<Character>();
                if (playerCharacter == null)
                {
                    Debug.LogError("Character component missing on PlayerObject!");
                }
            }
            else
            {
                Debug.LogError("PlayerObject is not assigned!");
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (!IsTrigger) return;
            TryApplyDamage(collision.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsTrigger) return;
            TryApplyDamage(collision.gameObject);
        }

        private void TryApplyDamage(GameObject target)
        {
            if (playerCharacter == null) return; 

            if (!damagedTargets.Contains(target)) // Check if this target was already damaged
            {
                damagedTargets.Add(target);
                ApplyDamage(target);

                // Start cooldown reset coroutine if not already running
                if (resetCooldownCoroutine == null)
                {
                    resetCooldownCoroutine = StartCoroutine(ResetCooldown());
                }
            }
        }

        private void ApplyDamage(GameObject target)
        {
            int StrengthBonus = (int)playerCharacter.Strength.Value;
            int TotalDamage = BaseDamage + StrengthBonus;

            Debug.Log("Total Damage Dealt: " + TotalDamage);

            if (target.GetComponent<IDamageable>() != null)
            {
                target.GetComponent<IDamageable>().Damage(TotalDamage, transform, RagdollForceAmount);
            }
            else if (target.GetComponent<LocationBasedDamageArea>() != null)
            {
                target.GetComponent<LocationBasedDamageArea>().DamageArea(TotalDamage, transform, RagdollForceAmount);
            }
        }

        private IEnumerator ResetCooldown()
        {
            yield return new WaitForSeconds(damageCooldown);
            damagedTargets.Clear(); // Allow new collisions after cooldown
            resetCooldownCoroutine = null;
        }
    }
}
