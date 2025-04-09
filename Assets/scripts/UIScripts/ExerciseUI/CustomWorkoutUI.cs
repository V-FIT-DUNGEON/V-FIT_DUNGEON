using System.Collections;
using System.Collections.Generic;
using BNG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class CustomWorkoutUI : MonoBehaviour
{
    [SerializeField] private ExerciseManager _exerciseManager;
    [SerializeField] Button increaseButton;
    [SerializeField] Button decreaseButton;
    [SerializeField] Button StartWorkoutButton;
    [SerializeField] Button FinishWorkoutButton;
    [SerializeField] TextMeshProUGUI repsText;
    [SerializeField] TextMeshProUGUI repsNumber;

    //make CustomWorkoutUI singleton
    public static CustomWorkoutUI Instance { get; private set; }
    


    // Start is called before the first frame update
    void Start()
    {
        increaseButton.onClick.AddListener(IncreaseReps);
        decreaseButton.onClick.AddListener(DecreaseReps);
        StartWorkoutButton.onClick.AddListener(StartWorkout);
        FinishWorkoutButton.onClick.AddListener(FinishWorkoutBtn);
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
        if (reps > 1)
        {
            reps--;
        }
        repsNumber.text = reps.ToString();
    }

    public void StartWorkout()
    {
        //set reps limit
        if (int.TryParse(repsNumber.text, out int repsLimit))
        {
            Debug.Log("Reps limit set to: " + repsLimit);
        }
        else
        {
            Debug.LogError("Invalid input: " + repsNumber.text);
        }
        _exerciseManager.SetReps(repsLimit);
        _exerciseManager.StartExercise();
        
        repsNumber.text = "0";


    }

    public void SetCountReps(int reps)
    {
        repsNumber.text = reps.ToString();
    }

    public void FinishWorkoutBtn()
    {
        _exerciseManager.FinishExerciseEarly();
        _exerciseManager.SetReps(1);
        repsNumber.text = "1";
        repsText.text = "reps";
        //save workout data
    }

    public void FinishWorkout(bool isFinished)
    {
        if (isFinished)
        {
            repsText.text = "Finish Workout";
        }
    }

    public void CalibratingUI(float countTime, float calibrationTime)
    {
        if (countTime == 0)//calibration Complete
        {
            Debug.Log("Calibraton complete!");
            repsText.text = "Calibration Complete!";
        }
        else if (countTime == -1) //reset UI to start workout
        {
            Debug.Log("workout started!");
            repsText.text = "Reps";
            repsNumber.text = "0";
        }
        else
        {
            Debug.Log("Calibrating UI: " + countTime);
            repsText.text = "Calibration in progress please stand still  ";
            repsNumber.text = Mathf.CeilToInt(calibrationTime - countTime).ToString() + "s";
        }
        
    }
}
