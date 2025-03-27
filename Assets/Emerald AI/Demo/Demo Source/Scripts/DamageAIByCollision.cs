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
        public WeaponStatRequirement WeaponRequirement; // Reference to the requirement script

        private Character playerCharacter;
        private HashSet<GameObject> damagedTargets = new HashSet<GameObject>(); // Store already damaged targets
        private float damageCooldown = 1.5f; // Cooldown time in seconds
        private Coroutine resetCooldownCoroutine;

        private void Start()
        {
            if (WeaponRequirement == null)
            {
                Debug.LogError("WeaponStatRequirement script is not assigned!");
                return;
            }

            playerCharacter = WeaponRequirement.GetPlayerCharacter(); // Get player from WeaponStatRequirement
            if (playerCharacter == null)
            {
                Debug.LogError("Character component is missing in WeaponStatRequirement!");
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
            if (playerCharacter == null || WeaponRequirement == null) return;

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
            int totalDamage;

            if (WeaponRequirement.CanEquip())
            {
                // Player meets the stat requirement → Normal damage
                totalDamage = BaseDamage + (int)playerCharacter.Strength.Value;
            }
            else
            {
                // Player does NOT meet the stat requirement → Half of BaseDamage, NO Strength bonus
                totalDamage = Mathf.FloorToInt(BaseDamage / 2);
            }

            Debug.Log($"Total Damage Dealt: {totalDamage} (Stats Met: {WeaponRequirement.CanEquip()})");

            if (target.GetComponent<IDamageable>() != null)
            {
                target.GetComponent<IDamageable>().Damage(totalDamage, transform, RagdollForceAmount);
            }
            else if (target.GetComponent<LocationBasedDamageArea>() != null)
            {
                target.GetComponent<LocationBasedDamageArea>().DamageArea(totalDamage, transform, RagdollForceAmount);
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
