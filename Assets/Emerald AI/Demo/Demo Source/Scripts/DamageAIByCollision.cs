using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats.Examples; // Import the Character Stats system

namespace EmeraldAI.Example
{
    /// <summary>
    /// A script that damages AI based on collisions. Can be used for dynamic damaging objects such as rocks, logs, 
    /// and other falling objects or collision-based weapons.
    /// </summary>
    public class DamageAIByCollision : MonoBehaviour
    {
        public bool IsTrigger = false;
        public int BaseDamage = 10; // Base damage before Strength multiplier
        public int RagdollForceAmount = 50;
        public GameObject PlayerObject; // Reference to the Player Object

        private Character playerCharacter; // Store Player's Character Component

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
            ApplyDamage(collision.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsTrigger) return;
            ApplyDamage(collision.gameObject);
        }

        private void ApplyDamage(GameObject target)
        {
            if (playerCharacter == null) return; // Ensure playerCharacter is valid

            int StrengthBonus = (int)playerCharacter.Strength.Value; // Get Strength from Player
            int TotalDamage = BaseDamage + StrengthBonus; // Apply Strength to Damage

            Debug.Log("Total Damage Dealt: " + TotalDamage);

            // Damages an AI to the collided object
            if (target.GetComponent<IDamageable>() != null)
            {
                target.GetComponent<IDamageable>().Damage(TotalDamage, transform, RagdollForceAmount);
            }
            // Damages an AI's location-based damage component
            else if (target.GetComponent<LocationBasedDamageArea>() != null)
            {
                LocationBasedDamageArea LBDArea = target.GetComponent<LocationBasedDamageArea>();
                LBDArea.DamageArea(TotalDamage, transform, RagdollForceAmount);
            }
        }
    }
}
