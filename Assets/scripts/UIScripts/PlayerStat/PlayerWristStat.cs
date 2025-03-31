using System.Collections;
using System.Collections.Generic;
using Kryz.CharacterStats.Examples;
using TMPro;
using UnityEngine;

public class PlayerWristStat : MonoBehaviour
{

    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private GameObject WristStatPanel;
    [SerializeField] private TextMeshProUGUI HealthText;
    [SerializeField] private TextMeshProUGUI StaminaText;
    [SerializeField] private TextMeshProUGUI CurrencyText;

    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentStamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float currencyAmount;
    // Start is called before the first frame update
    void Start()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        //maxHealth = PlayerObject.GetComponent<HealthSystem>().MaxHealth;
        maxStamina = PlayerObject.GetComponent<StaminaSystem>().MaxStamina;
        currencyAmount = PlayerObject.GetComponent<Character>().Currency;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
