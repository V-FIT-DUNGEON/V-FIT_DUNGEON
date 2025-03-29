using UnityEngine;
using Kryz.CharacterStats.Examples;
using Kryz.CharacterStats;

public class WeaponStatRequirement : MonoBehaviour
{
    [Header("Required Stats")]
    public float RequiredStrength;
    public float RequiredAgility;
    public float RequiredVitality;
    public float RequiredEndurance;

    public GameObject PlayerObject;
    private Character playerCharacter;
    private bool isHoldingWeapon = false;
    private StatModifier agilityPenaltyModifier;

    private void Awake()
    {
        ReassignPlayerObject();
    }

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

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is named "Grabber"
        if (other.gameObject.name == "Grabber")
        {
            // Move up six levels in the hierarchy
            Transform playerTransform = other.transform;
            for (int i = 0; i < 6; i++)
            {
                if (playerTransform.parent != null)
                    playerTransform = playerTransform.parent;
                else
                    break; // Stop if there's no more parent
            }

            // Check if we reached the correct PlayerObject
            if (playerTransform.gameObject == PlayerObject)
            {
                Debug.Log("Correct Player detected, grabbing weapon.");
                OnGrabWeapon();
            }
            else
            {
                Debug.Log("PlayerController not found after moving up.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collided object is named "Grabber"
        if (other.gameObject.name == "Grabber")
        {
            // Move up six levels in the hierarchy
            Transform playerTransform = other.transform;
            for (int i = 0; i < 6; i++)
            {
                if (playerTransform.parent != null)
                    playerTransform = playerTransform.parent;
                else
                    break; // Stop if there's no more parent
            }

            // Check if we reached the correct PlayerObject
            if (playerTransform.gameObject == PlayerObject)
            {
                Debug.Log("Correct Player detected, releasing weapon.");
                OnReleaseWeapon();
            }
            else
            {
                Debug.Log("PlayerController not found after moving up.");
            }
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
            // Remove any previous penalty to prevent stacking
            playerCharacter.Agility.RemoveAllModifiersFromSource(this);

            // Apply a flat penalty so Agility is reduced to -9
            float penaltyValue = -9 - playerCharacter.Agility.BaseValue;
            agilityPenaltyModifier = new StatModifier(penaltyValue, StatModType.Flat, this);
            playerCharacter.Agility.AddModifier(agilityPenaltyModifier);

            Debug.Log($"Agility reduced by {penaltyValue}, new Agility: {playerCharacter.Agility.Value}");
        }

        isHoldingWeapon = true;
    }

    public void OnReleaseWeapon()
    {
        if (playerCharacter == null || !isHoldingWeapon) return;

        // Remove the agility penalty when releasing the weapon
        playerCharacter.Agility.RemoveAllModifiersFromSource(this);
        agilityPenaltyModifier = null;

        Debug.Log($"Agility restored to {playerCharacter.Agility.Value}");

        isHoldingWeapon = false;
    }

    public void ReassignPlayerObject()
    {
        PlayerObject = GameObject.Find("PlayerController");
        if (PlayerObject == null)
        {
            Debug.LogError("PlayerObject not found! Ensure it has the 'Player' tag.");
        }
        else
        {     
            playerCharacter = PlayerObject.GetComponent<Character>();
            if (playerCharacter == null)
            {
                Debug.LogError("PlayerCharacter not found! Ensure it has the Character component.");
            }
        }
    }

    public Character GetPlayerCharacter()
    {
        return playerCharacter;
    }
}
