using UnityEngine;

public class WeaponChangeAnimation : MonoBehaviour
{
    [SerializeField] private Transform rootObject; // Assign the root object in the Inspector
    private Vector3 initialPosition;
    private Vector3 finalPosition;

    private void Awake()
    {
        initialPosition = transform.position;

        // Auto-assign finalPosition based on the root object's position
        if (rootObject != null)
        {
            finalPosition = rootObject.position + new Vector3(0f, 0f, -0.325f);
        }
        else
        {
            Debug.LogError("Root object is not assigned in WeaponChangeAnimation!");
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, finalPosition, 1f);
    }

    private void OnDisable()
    {
        transform.position = initialPosition;
    }
}
