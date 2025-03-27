using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats.Examples;
using Kryz.CharacterStats;

public class WeaponStatRequirement : MonoBehaviour
{
    [Header("Required Stats")]
    public int RequiredStrength;
    public int RequiredAgility;
    public int RequiredVitality;
    public int RequiredEndurance;

    public GameObject PlayerObject;
    private Character playerCharacter;
    private bool isHoldingWeapon = false;
    private StatModifier agilityModifier;

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

    public bool CanEquip()
    {
        if (playerCharacter == null)
        {
            Debug.LogError("Player character is null in WeaponStatRequirement!");
            return false;
        }
        return playerCharacter.Strength.Value >= RequiredStrength &&
            playerCharacter.Agility.Value >= RequiredAgility &&
            playerCharacter.Vitality.Value >= RequiredVitality &&
            playerCharacter.Endurance.Value >= RequiredEndurance;
    }

    public void OnGrabWeapon()
    {
        if (playerCharacter == null || isHoldingWeapon) return;

        if (!CanEquip())
        {
            // Remove existing modifier to avoid stacking issues
            playerCharacter.Agility.RemoveAllModifiersFromSource(this);

            // Apply agility penalty
            agilityModifier = new StatModifier(-playerCharacter.Agility.BaseValue - 9, StatModType.Flat, this);
            playerCharacter.Agility.AddModifier(agilityModifier);
        }

        isHoldingWeapon = true;
    }

    public void OnReleaseWeapon()
    {
        if (playerCharacter == null || !isHoldingWeapon) return;

        // Remove agility penalty modifier
        playerCharacter.Agility.RemoveAllModifiersFromSource(this);
        agilityModifier = null;

        isHoldingWeapon = false;
    }

    public Character GetPlayerCharacter()
    {
        return playerCharacter;
    }
}
