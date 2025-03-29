using UnityEngine;
using TMPro;

public class ForceUpdateText : MonoBehaviour
{
    [Header("Assign all TextMeshPro objects here")]
    public GameObject[] textObjects;

    private TextMeshPro[] textMeshes;

    void Start()
    {
        textMeshes = new TextMeshPro[textObjects.Length];

        for (int i = 0; i < textObjects.Length; i++)
        {
            if (textObjects[i] != null)
            {
                textMeshes[i] = textObjects[i].GetComponent<TextMeshPro>();

                if (textMeshes[i] == null)
                {
                    Debug.LogError("No TextMeshPro found on " + textObjects[i].name);
                }
            }
        }

        foreach (TextMeshPro textMesh in textMeshes)
        {
            if (textMesh != null)
            {
                textMesh.fontMaterial.renderQueue = 4000; // Ensures it's always rendered
            }
        }

    }

    void Update()
    {
        foreach (TextMeshPro textMesh in textMeshes)
        {
            if (textMesh != null)
            {
                textMesh.ForceMeshUpdate(); // Force update each text mesh
            }
        }
    }
}
