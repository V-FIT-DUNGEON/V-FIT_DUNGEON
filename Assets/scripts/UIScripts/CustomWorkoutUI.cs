using System.Collections;
using System.Collections.Generic;
using BNG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class CustomWorkoutUI : MonoBehaviour
{
    [SerializeField] Button increaseButton;
    [SerializeField] Button decreaseButton;
    [SerializeField] TextMeshProUGUI repsNumber;
    [SerializeField] int reps = 0;
    // Start is called before the first frame update
    void Start()
    {
        increaseButton.onClick.AddListener(IncreaseReps);
        decreaseButton.onClick.AddListener(DecreaseReps);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseReps()
    {
        int reps = int.Parse(repsNumber.text);
        reps++;
        repsNumber.text = reps.ToString();
    }

    public void DecreaseReps()
    {
        int reps = int.Parse(repsNumber.text);
        if (reps > 0)
        {
            reps--;
        }
        repsNumber.text = reps.ToString();
    }
}
