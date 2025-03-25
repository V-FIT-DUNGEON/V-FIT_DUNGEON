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
    [SerializeField] TextMeshProUGUI repsNumber;

    //make CustomWorkoutUI singleton
    public static CustomWorkoutUI Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    


    // Start is called before the first frame update
    void Start()
    {
        increaseButton.onClick.AddListener(IncreaseReps);
        decreaseButton.onClick.AddListener(DecreaseReps);
        StartWorkoutButton.onClick.AddListener(SetReps);
        FinishWorkoutButton.onClick.AddListener(FinishWorkout);
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

    public void SetReps()
    {
        if (int.TryParse(repsNumber.text, out int repsLimit))
        {
            Debug.Log("Reps limit set to: " + repsLimit);
        }
        else
        {
            Debug.LogError("Invalid input: " + repsNumber.text);
        }
        _exerciseManager.SetReps(repsLimit);
        repsNumber.text = "0";
        _exerciseManager.StartExercise();
    }

    public void SetCountReps(int reps)
    {
        repsNumber.text = reps.ToString();
    }

    public void FinishWorkout()
    {
        _exerciseManager.SetReps(0);
        _exerciseManager.FinishExercise();
        //save workout data
    }
}
