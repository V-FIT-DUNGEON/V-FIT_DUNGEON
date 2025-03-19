using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats.Examples; // Ensure this matches your Character's namespace

namespace EmeraldAI.Example
{
    /// <summary>
    /// A script that damages AI based on collisions. Can be used for dynamic damaging objects such as rocks, logs, 
    /// and other falling objects or collision-based weapons.
    /// </summary>
    public class DamageAIByCollision : MonoBehaviour
    {
        public bool IsTrigger = false;
        public int BaseDamage = 10;
        public int RagdollForceAmount = 50;

        private int CalculateDamage(Collider collider)
        {
            Character character = collider.GetComponent<Character>(); // Get the Character component from the colliding object

            int strengthBonus = (character != null) ? (int)character.Strength.Value : 0; // Explicitly cast float to int
            return BaseDamage + strengthBonus;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (!IsTrigger) return;

            int totalDamage = CalculateDamage(collision);

            if (collision.gameObject.GetComponent<IDamageable>() != null)
            {
                collision.gameObject.GetComponent<IDamageable>().Damage(totalDamage, transform, RagdollForceAmount);
            }
            else if (collision.gameObject.GetComponent<LocationBasedDamageArea>() != null)
            {
                LocationBasedDamageArea LBDArea = collision.gameObject.GetComponent<LocationBasedDamageArea>();
                LBDArea.DamageArea(totalDamage, transform, RagdollForceAmount);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsTrigger) return;

            int totalDamage = CalculateDamage(collision.collider);

            if (collision.gameObject.GetComponent<IDamageable>() != null)
            {
                collision.gameObject.GetComponent<IDamageable>().Damage(totalDamage, transform, RagdollForceAmount);
            }
            else if (collision.gameObject.GetComponent<LocationBasedDamageArea>() != null)
            {
                LocationBasedDamageArea LBDArea = collision.gameObject.GetComponent<LocationBasedDamageArea>();
                LBDArea.DamageArea(totalDamage, transform, RagdollForceAmount);
            }
        }
    }
}
