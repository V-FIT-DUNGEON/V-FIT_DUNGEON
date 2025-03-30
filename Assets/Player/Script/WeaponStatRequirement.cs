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

    [Header("Hand Tracking")]
    public Transform grabberTransform; // Assign the hand/grabber transform here
    public float maxGrabDistance = 1f; // Max distance before it counts as released

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

    private void Update()
    {
        if (isHoldingWeapon && grabberTransform != null)
        {
            float distance = Vector3.Distance(grabberTransform.position, transform.position);
            if (distance > maxGrabDistance)
            {
                Debug.Log($"Weapon too far from hand ({distance}m). Releasing.");
                OnReleaseWeapon();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Grabber")
        {
            Transform playerTransform = GetParentPlayerObject(other.transform);
            if (playerTransform != null && playerTransform.gameObject == PlayerObject)
            {
                grabberTransform = other.transform; // Store grabber reference
                Debug.Log("Correct Player detected, grabbing weapon.");
                OnGrabWeapon();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Grabber")
        {
            Transform playerTransform = GetParentPlayerObject(other.transform);
            if (playerTransform != null && playerTransform.gameObject == PlayerObject)
            {
                Debug.Log("Hand exited trigger, but checking distance first...");
                // We don't release immediately; we wait for Update() to verify distance.
            }
        }
    }

    private Transform GetParentPlayerObject(Transform child)
    {
        Transform parent = child;
        for (int i = 0; i < 6; i++)
        {
            if (parent.parent != null)
                parent = parent.parent;
            else
                break;
        }
        return parent;
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
            playerCharacter.Agility.RemoveAllModifiersFromSource(this);
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

        playerCharacter.Agility.RemoveAllModifiersFromSource(this);
        agilityPenaltyModifier = null;
        isHoldingWeapon = false;
        grabberTransform = null;

        Debug.Log($"Agility restored to {playerCharacter.Agility.Value}");
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