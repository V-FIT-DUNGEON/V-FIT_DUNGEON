using UnityEngine;
using TMPro;
using Kryz.CharacterStats.Examples;

public class MoneyCounter : MonoBehaviour
{
    private TMP_Text txt;
    private Character playerCharacter;

    private void Awake()
    {
        txt = GetComponent<TMP_Text>();
        FindPlayerCharacter();
    }

    private void FindPlayerCharacter()
    {
        GameObject playerObject = GameObject.Find("PlayerController");
        if (playerObject != null)
        {
            playerCharacter = playerObject.GetComponent<Character>();
            if (playerCharacter != null)
            {
                playerCharacter.OnCurrencyChanged += UpdateMoneyText;
                UpdateMoneyText(playerCharacter.Currency); // Initial update
            }
        }
    }

    private void UpdateMoneyText(float newAmount)
    {
        if (txt != null)
        {
            txt.text = newAmount + " Coins";
        }
    }
}
