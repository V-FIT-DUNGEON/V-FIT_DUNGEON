using UnityEngine;
using System;
using Kryz.CharacterStats.Examples;

public class Collectible : MonoBehaviour
{
    public static event Action OnCollected;
    public float currencyValue = 100f; // Value this collectible gives
    public AudioClip collectSound; // Sound to play on collection

    private AudioSource audioSource;
    private Collider itemCollider; // Reference to the collider
    private bool isCollected = false; // Prevent multiple collections

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        itemCollider = GetComponent<Collider>(); // Get the collider reference

        if (collectSound != null)
        {
            audioSource.clip = collectSound;
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        transform.localRotation = Quaternion.Euler(0f, Time.time * 100f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return; // Prevent multiple collections

        if (other.CompareTag("Player"))
        {
            isCollected = true; // Mark as collected
            itemCollider.enabled = false; // Disable collider to prevent further triggers

            Character playerCharacter = other.GetComponent<Character>();

            if (playerCharacter != null)
            {
                playerCharacter.Currency += currencyValue; // Increase currency
                Debug.Log("Currency Increased! New Value: " + playerCharacter.Currency);
            }

            if (collectSound != null)
            {
                audioSource.Play(); // Play sound
                GetComponent<MeshRenderer>().enabled = false; // Hide the object while sound plays
                Destroy(gameObject, collectSound.length); // Destroy after sound finishes
            }
            else
            {
                Destroy(gameObject);
            }

            OnCollected?.Invoke();
        }
    }
}
