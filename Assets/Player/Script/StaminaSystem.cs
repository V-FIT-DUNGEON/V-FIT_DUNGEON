using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats.Examples;

public class StaminaSystem : MonoBehaviour
{
    private Character character;

    public float BaseStamina = 100;
    public float CurrentStamina;
    public float StaminaRegenRate = 5f; // Base stamina regen per second
    public float SprintStaminaDrain = 10f; // Stamina drain per second while sprinting
    public float JumpStaminaCost = 15f; // Stamina cost per jump
    public float MaxStamina;

    private bool isSprinting;
    private bool isJumping;
    private bool forceStoppedSprint; // Prevents stamina usage after forced stop

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
            // Only drain stamina if it wasn't forcefully stopped
            if (!forceStoppedSprint)
            {
                CurrentStamina -= SprintStaminaDrain * Time.deltaTime;
            }

            // Stop sprinting if stamina is depleted
            if (CurrentStamina <= 0)
            {
                CurrentStamina = 0;
                isSprinting = false; // Force stop sprinting
                forceStoppedSprint = true; // Prevent additional stamina drain
            }
        }

        if (isJumping)
        {
            CurrentStamina -= JumpStaminaCost;
            isJumping = false; // Reset after jumping
        }

        // Prevent stamina from going below 0
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);

        // **Regenerate stamina only when NOT sprinting or jumping**
        if (!isSprinting && !isJumping)
        {
            float regenBoost = (character != null) ? (0.01f * character.Endurance.Value) : 0f;
            CurrentStamina += (StaminaRegenRate + regenBoost) * Time.deltaTime;
            forceStoppedSprint = false; // Reset sprinting restriction after regen
        }

        // Clamp to max stamina
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);
    }

    public bool CanSprint()
    {
        return CurrentStamina > SprintStaminaDrain * Time.deltaTime; // Prevents instant stop when reaching zero
    }

    public bool CanJump()
    {
        return CurrentStamina >= JumpStaminaCost;
    }

    public void StartSprint()
    {
        if (CanSprint())
        {
            isSprinting = true;
            forceStoppedSprint = false; // Reset forced stop flag when player can sprint again
        }
    }

    public void StopSprint()
    {
        isSprinting = false;
    }

    public void Jump()
    {
        if (CanJump())
        {
            isJumping = true;
        }
    }
}
