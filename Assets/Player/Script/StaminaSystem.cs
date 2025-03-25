using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats.Examples;

public class StaminaSystem : MonoBehaviour
{
    private Character character;

    public float BaseStamina = 100;
    public float CurrentStamina;
    public float StaminaRegenRate = 5f; // Stamina regen per second
    public float SprintStaminaDrain = 10f; // Stamina drain per second while sprinting
    public float JumpStaminaCost = 15f; // Stamina cost per jump
    public float MaxStamina;

    private bool isSprinting;
    private bool isJumping;

    void Start()
    {
        character = GetComponent<Character>();

        if (character != null)
        {
            MaxStamina = BaseStamina + character.Endurance.Value;
            CurrentStamina = MaxStamina;
        }
    }

    void Update()
    {
        HandleStamina();
    }

    void HandleStamina()
    {
        if (isSprinting)
        {
            CurrentStamina -= SprintStaminaDrain * Time.deltaTime;

            // Stop sprinting if stamina is depleted
            if (CurrentStamina <= 0)
            {
                CurrentStamina = 0;
                isSprinting = false; // Force stop sprinting
            }
        }

        if (isJumping)
        {
            CurrentStamina -= JumpStaminaCost;
            isJumping = false; // Reset after jumping
        }

        // Prevent stamina from going below 0
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);

        // **Only regenerate stamina when NOT sprinting or jumping**
        if (!isSprinting && !isJumping)
        {
            CurrentStamina += StaminaRegenRate * Time.deltaTime;
        }

        // Clamp to max stamina
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);
    }

    public bool CanSprint()
    {
        return CurrentStamina > 0;
    }

    public bool CanJump()
    {
        return CurrentStamina >= JumpStaminaCost;
    }

    public void StartSprint()
    {
        // Only start sprinting if there is enough stamina to sustain at least one frame
        if (CanSprint() && CurrentStamina > SprintStaminaDrain * Time.deltaTime)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    public void StopSprint()
    {
        isSprinting = false;
    }

    public void Jump()
    {
        if (CanJump())
            isJumping = true;
    }
}

