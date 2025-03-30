using UnityEngine;
using EmeraldAI;

public class EnemyItemDrop : MonoBehaviour
{
    [Header("Item Drops")]
    public GameObject[] dropItemPrefabs; // Array of item prefabs
    public float scatterRadius = 2f; // How far items can scatter from the enemy's position

    private EmeraldHealth enemyHealth;

    void Start()
    {
        enemyHealth = GetComponent<EmeraldHealth>(); // Get EmeraldHealth component
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += DropItems; // Subscribe to the OnDeath event
        }
    }

    void DropItems()
    {
        if (dropItemPrefabs.Length > 0)
        {
            foreach (GameObject itemPrefab in dropItemPrefabs)
            {
                if (itemPrefab != null)
                {
                    // Random offset for scattering
                    Vector3 randomOffset = new Vector3(
                        Random.Range(-scatterRadius, scatterRadius), 
                        0, 
                        Random.Range(-scatterRadius, scatterRadius)
                    );

                    Vector3 dropPosition = transform.position + randomOffset;
                    Instantiate(itemPrefab, dropPosition, Quaternion.identity);
                }
            }
        }
    }

    void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath -= DropItems; // Unsubscribe to prevent memory leaks
        }
    }
}