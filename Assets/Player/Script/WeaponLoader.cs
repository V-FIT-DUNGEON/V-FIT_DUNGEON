using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLoader : MonoBehaviour
{
    public GameObject[] weaponPrefabs; // Assign in Inspector
    private GameObject spawnedWeapon;

    private void Start()
    {
        SpawnWeapon();
    }

    private void SpawnWeapon()
    {
        // Find the player
        GameObject player = GameObject.Find("PlayerController");
        if (player == null)
        {
            Debug.LogError("PlayerController not found!");
            return;
        }

        // Get the current weapon index
        int weaponIndex = InventoryManager.instance.currentWeapon;
        if (weaponIndex < 0 || weaponIndex >= weaponPrefabs.Length)
        {
            Debug.LogError("Invalid weapon index!");
            return;
        }

        // Ensure only one weapon is spawned
        if (spawnedWeapon != null)
        {
            Destroy(spawnedWeapon);
        }

        // Spawn the weapon at the player's position
        Vector3 spawnPosition = player.transform.position + player.transform.forward * 1.5f + Vector3.up * 0.5f;
        spawnedWeapon = Instantiate(weaponPrefabs[weaponIndex], spawnPosition, Quaternion.identity);

        Debug.Log($"Spawned weapon: {spawnedWeapon.name}");
    }
}
