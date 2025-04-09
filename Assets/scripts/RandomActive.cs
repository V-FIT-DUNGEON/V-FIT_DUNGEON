using UnityEngine;

public class RandomActive : MonoBehaviour
{
    public GameObject[] objectsToActivate;

    void Start()
    {
        // Disable all first
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(false);
        }

        // Choose and activate one at random
        if (objectsToActivate.Length > 0)
        {
            int randomIndex = Random.Range(0, objectsToActivate.Length);
            objectsToActivate[randomIndex].SetActive(true);
        }
    }
}
