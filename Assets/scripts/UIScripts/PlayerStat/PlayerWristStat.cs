using System.Collections;
using EmeraldAI;
using Kryz.CharacterStats.Examples;
using TMPro;
using UnityEngine;

public class PlayerWristStat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject playerControllerObject;
    [SerializeField] private GameObject wristStatPanel;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private TextMeshProUGUI currencyText;

    private EmeraldGeneralTargetBridge healthSystem;
    private StaminaSystem staminaSystem;
    private Character character;

    private float previousHealth;
    private float previousStamina;
    private float previousCurrency;

    private void Start()
    {
        // Cache components once
        healthSystem = playerControllerObject.GetComponent<EmeraldGeneralTargetBridge>();
        staminaSystem = playerControllerObject.GetComponent<StaminaSystem>();
        character = playerControllerObject.GetComponent<Character>();

        UpdateAllStats(); // Initial UI update
    }

    private void Update()
    {
        // Update only when values actually change
        if (HasHealthChanged() || HasStaminaChanged() || HasCurrencyChanged())
        {
            UpdateAllStats();
        }
    }

    private void UpdateAllStats()
    {
        UpdateHealthUI();
        UpdateStaminaUI();
        UpdateCurrencyUI();
    }

    private void UpdateHealthUI()
    {
        float currentHealth = healthSystem.Health;
        float maxHealth = healthSystem.StartHealth;
        float percentage = (currentHealth / maxHealth) * 100f;

        healthText.text = $"{percentage:F2}%";
        previousHealth = currentHealth;
    }

    private void UpdateStaminaUI()
    {
        float currentStamina = staminaSystem.CurrentStamina;
        float maxStamina = staminaSystem.MaxStamina;
        float percentage = (currentStamina / maxStamina) * 100f;

        staminaText.text = $"{percentage:F2}%";
        previousStamina = currentStamina;
    }

    private void UpdateCurrencyUI()
    {
        float currentCurrency = character.Currency;
        currencyText.text = $"{currentCurrency}";
        previousCurrency = currentCurrency;
    }

    private bool HasHealthChanged() => previousHealth != healthSystem.Health;
    private bool HasStaminaChanged() => previousStamina != staminaSystem.CurrentStamina;
    private bool HasCurrencyChanged() => previousCurrency != character.Currency;
}
