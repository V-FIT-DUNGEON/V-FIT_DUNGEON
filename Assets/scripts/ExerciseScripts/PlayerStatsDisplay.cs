using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats.Examples;

public class PlayerStatsDisplay : MonoBehaviour
{
    public GameObject PlayerObject;
    private Character playerCharacter;

    private void Start()
    {
        if (PlayerObject != null)
         {
            playerCharacter = PlayerObject.GetComponent<Character>();
             if (playerCharacter == null)
            {
                 Debug.LogError("Character component missing on PlayerObject!");
            }
             else
            {
                DisplayStats();
            }
        }
        else
        {
             Debug.LogError("PlayerObject is not assigned!");
        }
     }

    private void DisplayStats()
    {
        Debug.Log("=== Player Stats ===");
        Debug.Log("Strength: " + playerCharacter.Strength.Value);
        Debug.Log("Agility: " + playerCharacter.Agility.Value);
        Debug.Log("Vitality: " + playerCharacter.Vitality.Value);
        Debug.Log("Endurance: " + playerCharacter.Endurance.Value);
        Debug.Log("====================");
    }

    public Dictionary<string, float> GetPlayerStats()
    {
        if (playerCharacter == null)
        {
            Debug.LogError("PlayerCharacter is not set!");
            return null;
        }

        Dictionary<string, float> stats = new Dictionary<string, float>
        {
            { "Strength", playerCharacter.Strength.Value },
            { "Agility", playerCharacter.Agility.Value },
            { "Vitality", playerCharacter.Vitality.Value },
            { "Endurance", playerCharacter.Endurance.Value },
        };

        return stats;
      }
}