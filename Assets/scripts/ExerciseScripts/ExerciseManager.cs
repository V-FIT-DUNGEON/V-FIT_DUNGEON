using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Kryz.CharacterStats.Examples;
using System.Collections.Generic;

public class ExerciseManager : MonoBehaviour
{
    [Header("Set up device")]
    [SerializeField] private Transform Headset; // Assign the VR headset GameObject

    private enum ExerciseSelected { None, Squat, PushUp, Plank }
    private enum ExerciseState { Idle, Up, Down }

    [Header("Player Settings")]
    [SerializeField] private GameObject _PlayerObject; // Assign the Player GameObject
    [SerializeField] private Character _PlayerCharacter; // Assign the Player Character
    [SerializeField] private float _Strength = 0f; // Player Strength
    [SerializeField] private float _Vitality = 0f; // Player Vitality
    [SerializeField] private float _Agility = 0f; // Player Agility
    [SerializeField] private float _Endurance = 0f; // Player Endurance

    [Header("Exercise Settings")]
    [SerializeField] private GameObject detectionSystem; // Assign the DetectionSystem GameObject

    [SerializeField] private ExerciseSelected currentExercise = ExerciseSelected.None;
    [SerializeField] private ExerciseState currentExerciseState = ExerciseState.Idle;
    
    [SerializeField] private bool isExerciseActive = false;
    [SerializeField] private bool calibrated = false;
    [SerializeField] private float standingHeight;
    [SerializeField] private Vector3 lastPosition;

    [SerializeField] private int repsLimit = 12;
    [SerializeField] private int repsCount = 0;

    [Header("Events")]
    [SerializeField] private UnityEvent<bool> OnCalibrationEvent; // UI feedback for calibration
    [SerializeField] private UnityEvent<int> OnSquatRepsCountEvent; // UI feedback for Squat reps
    [SerializeField] private UnityEvent<int> OnPushUpRepsCountEvent; // UI feedback for Push-Up reps
    [SerializeField] private UnityEvent<int> OnPlankCountEvent; // UI feedback for Plank
    [SerializeField] private UnityEvent<bool> OnFinishExerciseEvent; // UI feedback for exercise completion

    public static ExerciseManager Instance { get; private set; }

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

        // Reassign value
        ReassignValue();

    }

    private void OnEnable()
    {
        standingHeight = Headset.position.y;
        lastPosition = Headset.position;
    }

    private void Update()
    {
        if (!calibrated) return; // Ensure exercise only runs if calibrated

        switch (currentExercise)
        {
            case ExerciseSelected.Squat:
                DetectSquat();
                break;
            case ExerciseSelected.PushUp:
                DetectPushUp();
                break;
            case ExerciseSelected.Plank:
                DetectPlank();
                break;
        }
    }

    // ---- Exercise Detection Methods ----

    private void DetectSquat()
    {
        float headY = Headset.position.y;

        switch (currentExerciseState)
        {
            case ExerciseState.Idle:
                if (headY > standingHeight * 0.95f)
                {
                    currentExerciseState = ExerciseState.Down;
                    Debug.Log("Start Squat");
                }
                break;
            case ExerciseState.Up:
                if (headY > standingHeight * 0.95f)
                {
                    currentExerciseState = ExerciseState.Down;
                    repsCount++;
                    OnSquatRepsCountEvent.Invoke(repsCount);
                    Debug.Log($"Squat Rep {repsCount}");
                }
                break;
            case ExerciseState.Down:
                if (headY < standingHeight * 0.7f)
                {
                    currentExerciseState = ExerciseState.Up;
                    Debug.Log("Squat Down");
                }
                break;
        }

        CheckExerciseCompletion();
    }

    private void DetectPushUp()
    {
        float headY = Headset.position.y;
        float pushUpThreshold = standingHeight * 0.5f;

        switch (currentExerciseState)
        {
            case ExerciseState.Idle:
                if (headY > standingHeight * 0.7f) // Ready position
                {
                    currentExerciseState = ExerciseState.Down;
                }
                break;
            case ExerciseState.Up:
                if (headY > standingHeight * 0.7f)
                {
                    currentExerciseState = ExerciseState.Down;
                    repsCount++;
                    OnPushUpRepsCountEvent.Invoke(repsCount);
                    Debug.Log($"Push-Up Rep {repsCount}");
                }
                break;
            case ExerciseState.Down:
                if (headY < pushUpThreshold)
                {
                    currentExerciseState = ExerciseState.Up;
                    Debug.Log("Push-Up Down");
                }
                break;
        }

        CheckExerciseCompletion();
    }

    private void DetectPlank()
    {
        float headY = Headset.position.y;
        float plankHeight = standingHeight * 0.4f;

        if (headY < plankHeight)
        {
            OnPlankCountEvent.Invoke(repsCount);
            Debug.Log("Plank Hold Started");
        }
    }

    private void CheckExerciseCompletion()
    {
        if (repsCount >= repsLimit)
        {
            Debug.Log("Exercise Completed!");
            OnFinishExerciseEvent.Invoke(true);
            switch (currentExercise)
            {
                case ExerciseSelected.Squat:
                    //add end agility
                    _Agility += 0.1f * repsCount;
                    _Endurance += 1f * repsCount;

                    _PlayerCharacter.Agility.BaseValue = _Agility;
                    _PlayerCharacter.Endurance.BaseValue = _Endurance;
                    break;
                case ExerciseSelected.PushUp:
                    //add str vit
                    _Strength += 1f * repsCount;
                    _Vitality += 1f * repsCount;
                    _PlayerCharacter.Strength.BaseValue = _Strength;
                    _PlayerCharacter.Vitality.BaseValue = _Vitality;
                    break;
                case ExerciseSelected.Plank:
                    
                    break;
            }
            ResetExercise();
        }
    }

    private void ResetExercise()
    {
        repsCount = 0;
        isExerciseActive = false;
        calibrated = false;
        detectionSystem.SetActive(false);
    }

    // ---- Exercise Selection Methods ----
    public void SelectExercise(int exerciseIndex)
    {
        currentExercise = (ExerciseSelected)exerciseIndex;
        Debug.Log($"Exercise Selected: {currentExercise}");
    }

    public void StartExercise()
    {
        if (calibrated)
        {
            isExerciseActive = true;
            detectionSystem.SetActive(true);
            Debug.Log("Exercise Started");
        }
        else
        {
            Debug.Log("Calibration Required Before Exercise!");
            OnCalibrationEvent.Invoke(false);
        }
    }

    public void FinishExerciseEarly()
    {
        if (isExerciseActive == true)
        {
            Debug.Log("Exercise Finished Early!");
            switch (currentExercise)
            {
                case ExerciseSelected.Squat:
                    //add end agility
                    _Agility += 0.1f * repsCount;
                    _Endurance += 1f * repsCount;

                    _PlayerCharacter.Agility.BaseValue = _Agility;
                    _PlayerCharacter.Endurance.BaseValue = _Endurance;
                    break;
                case ExerciseSelected.PushUp:
                    //add str vit
                    _Strength += 1f * repsCount;
                    _Vitality += 1f * repsCount;
                    _PlayerCharacter.Strength.BaseValue = _Strength;
                    _PlayerCharacter.Vitality.BaseValue = _Vitality;
                    break;
                case ExerciseSelected.Plank:
                    
                    break;
            }
            ResetExercise();
            Debug.Log("Exercise already finished!");
        }
        else
        {
            ResetExercise();
            Debug.Log("No exercise in progress to stop.");
        }
        
    }

    // ---- Set reps ----
    public void SetReps(int reps)
    {
        repsLimit = reps;
    }

    // ---- Calibration ----
    public void SetCalibrated(bool isCalibrated)
    {
        calibrated = isCalibrated;
        OnCalibrationEvent.Invoke(calibrated);
        isExerciseActive = true; // Set exercise active when calibrated
        Debug.Log("Calibration Status Updated: " + calibrated);
    }

    // ---- Reassign value func ----
    public void ReassignValue()
    {
        // ----- Player Object ----
        _PlayerObject = GameObject.Find("PlayerController");
        if (_PlayerObject == null)
        {
            Debug.LogError("PlayerObject not found! Ensure it has the 'Player' tag.");
        }
        else
        {     
            _PlayerCharacter = _PlayerObject.GetComponent<Character>();
            if (_PlayerCharacter == null)
            {
                Debug.LogError("PlayerCharacter not found! Ensure it has the Character component.");
            }
            else
            {
                _Strength = _PlayerCharacter.Strength.BaseValue;
                _Vitality = _PlayerCharacter.Vitality.BaseValue;
                _Agility = _PlayerCharacter.Agility.BaseValue;
                _Endurance = _PlayerCharacter.Endurance.BaseValue;
            }
            
        }

        // ----- Device ----
        Headset = GameObject.Find("CenterEyeAnchor").transform;
    }

    // ---- Save and Load ----

}
