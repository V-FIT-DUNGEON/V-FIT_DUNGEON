using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ExerciseManager : MonoBehaviour
{
    [Header("Set up device")]
    [SerializeField] private Transform headset; // Assign the VR headset GameObject

    private enum ExerciseSelected { None, Squat, PushUp, Plank }
    private enum ExerciseState { Idle, Up, Down }

    [SerializeField] private ExerciseSelected currentExercise = ExerciseSelected.None;
    [SerializeField] private ExerciseState currentExerciseState = ExerciseState.Idle;
    
    [SerializeField] private bool isExerciseActive = false;
    [SerializeField] private bool calibrated = false;

    [SerializeField] private GameObject detectionSystem; // Assign the DetectionSystem GameObject

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
    }

    private void OnEnable()
    {
        standingHeight = headset.position.y;
        lastPosition = headset.position;
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
        float headY = headset.position.y;

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
        float headY = headset.position.y;
        float pushUpThreshold = standingHeight * 0.5f;

        switch (currentExerciseState)
        {
            case ExerciseState.Idle:
                if (headY > standingHeight * 0.6f) // Ready position
                {
                    currentExerciseState = ExerciseState.Down;
                }
                break;
            case ExerciseState.Up:
                if (headY > standingHeight * 0.6f)
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
        float headY = headset.position.y;
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

    public void FinishExercise()
    {
        ResetExercise();
        Debug.Log("Exercise Stopped");
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
        Debug.Log("Calibration Status Updated: " + calibrated);
    }
}
