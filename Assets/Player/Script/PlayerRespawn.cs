using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerRespawnHandler : MonoBehaviour
{
    public EmeraldAI.EmeraldGeneralTargetBridge playerHealth;
    public float respawnDelay = 5f; // Time before respawning

    void Update()
    {
        if (playerHealth != null && playerHealth.Health <= 0)
        {
            SceneManager.LoadScene(0); // Load the main menu scene (index 0)
            // HandleRespawn();
        }
    }

    private void HandleRespawn()
    {
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(respawnDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
