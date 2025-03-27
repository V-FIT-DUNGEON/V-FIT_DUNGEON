using UnityEngine;
using EmeraldAI;

public class EnemyItemDrop : MonoBehaviour
{
    public GameObject dropItemPrefab; // Assign this in the Inspector

    private EmeraldHealth enemyHealth;

    void Start()
    {
        enemyHealth = GetComponent<EmeraldHealth>(); // Get EmeraldHealth component
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += DropItem; // Subscribe to the OnDeath event
        }
    }

    void DropItem()
    {
        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        }
    }

    void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath -= DropItem; // Unsubscribe from the event to prevent memory leaks
        }
    }
}
