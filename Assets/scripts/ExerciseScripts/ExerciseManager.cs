using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExerciseManager : MonoBehaviour
{

    [Header("Set up device")]  
    [SerializeField] public Transform headset; // Assign the VR headset GameObject

    private enum ExerciseSelected { None, Squat, PushUp, Plank }
    private enum ExerciseState { idle, up, down, left, right }
    [SerializeField] private ExerciseSelected currentExercise = ExerciseSelected.None;
    [SerializeField] private ExerciseState currentExerciseState = ExerciseState.idle;
    [SerializeField] private bool isExerciseActive = false;
    [SerializeField] private bool calibrated = false;

    [SerializeField] private GameObject DetectionSystem; // Assign the DetectionSystem GameObject

    [SerializeField] private float standingHeight;
    [SerializeField] private Vector3 lastPosition;

    [SerializeField] private int repsLimit = 12;
    [SerializeField] private int repsCount = 0;

    [SerializeField] private UnityEvent<bool> OnEventCalibration; // Event to send calibration status to UI
    [SerializeField] private UnityEvent<int> OnEventSquatRepsCount; // Event to send reps count to UI
    [SerializeField] private UnityEvent<int> OnEventPushUpRepsCount; // Event to send reps count to UI
    [SerializeField] private UnityEvent<int> OnEventplankCount; // Event to send reps count to UI

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

    void OnEnable()
    {
        // Initialize player height and movement tracking
        standingHeight = headset.position.y;
        lastPosition = headset.position;
    }

    void Update()
    {
        CaseHandler();
    }

    public void CaseHandler()
    {
        float headY = headset.position.y;
        float headX = headset.position.x;
        float headZ = headset.position.z;

        switch (currentExercise)
        {
            case ExerciseSelected.Squat:
                DetectSquat(headY);
                break;
            case ExerciseSelected.PushUp:
                DetectPushUp(headY);
                break;
            case ExerciseSelected.Plank:
                DetectPlank(headY);
                break;
        }
    }

    // ---- Exercise Detection Methods ----

    private void DetectSquat(float headY)
    {
        Debug.Log("Calibrated: " + calibrated);
        if (calibrated)
        {
            Debug.Log("Calibrated");
            if (isExerciseActive && repsCount < repsLimit)
            {
                switch (currentExerciseState)
                {
                    case ExerciseState.idle:
                        if (headY > standingHeight * 0.95f)
                        {
                            currentExerciseState = ExerciseState.down;
                            Debug.Log("Start");
                        }
                        break;
                    case ExerciseState.up:
                        if (headY > standingHeight * 0.95f)
                        {
                            currentExerciseState = ExerciseState.down;
                            Debug.Log("Squat up");
                            repsCount++;
                            OnEventSquatRepsCount.Invoke(repsCount);
                        }
                        break;
                    case ExerciseState.down:
                        if (headY < standingHeight * 0.7f)
                        {
                            currentExerciseState = ExerciseState.up;
                            Debug.Log("Squat Down");
                        }
                        break;

                }

            }
            else{
                Debug.Log("Exercise Finished");
                OnEventSquatRepsCount.Invoke(repsCount);
                repsCount = 0;
                isExerciseActive = false;
                DetectionSystem.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Calibration Required");
            OnEventCalibration.Invoke(calibrated);
        }
    }

    private void DetectPushUp(float headY)
    {
        if (isExerciseActive)
        {
            float pushUpThreshold = standingHeight * 0.5f; // Push-Up at 50% of standing height
            if (headY < pushUpThreshold)
                Debug.Log("Push-Up Detected");
        }
    }

    private void DetectSideLunge(float headX, float headZ)
    {
        if (isExerciseActive)
        {
            float lateralMovement = Vector3.Distance(new Vector3(headX, 0, headZ), new Vector3(lastPosition.x, 0, lastPosition.z));
            if (lateralMovement > 0.3f) // Adjust based on lunge width
                Debug.Log("Side Lunge Detected");

            lastPosition = headset.position;
        }
    }

    private void DetectPlank(float headY)
    {
        if (isExerciseActive)
        {
            float plankHeight = standingHeight * 0.4f; // Plank is around 40% of standing height
            if (headY < plankHeight)
                Debug.Log("Plank Hold Started");
        }
    }

    // ---- Exercise Selection Methods ----
    public void SelectSquat() => currentExercise = ExerciseSelected.Squat;
    public void SelectPushUp() => currentExercise = ExerciseSelected.PushUp;
    public void SelectPlank() => currentExercise = ExerciseSelected.Plank;
    public void DeselectExercise() => currentExercise = ExerciseSelected.None;
    public void StartExercise() => isExerciseActive = true;
    public void FinishExercise() => isExerciseActive = false;

    // ---- Set reps ----
    public void SetReps(int reps)
    {
        repsLimit = reps;
    }

    // ---- Calibration ----
    public void SetCalibrated(bool isCalibrated)
    {
        calibrated = isCalibrated;
    }
}

