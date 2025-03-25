using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseManager : MonoBehaviour
{

    [Header("Set up device")]  
    [SerializeField] public Transform headset; // Assign the VR headset GameObject

    private enum ExerciseSelected { None, Squat, PushUp, SideLunge, Plank }
    private enum ExerciseState { idle, up, down, left, right }
    [SerializeField] private ExerciseSelected currentExercise = ExerciseSelected.None;
    [SerializeField] private ExerciseState currentExerciseState = ExerciseState.idle;
    [SerializeField] private bool isExerciseActive = false;

    [SerializeField] private float standingHeight;
    [SerializeField] private Vector3 lastPosition;

    void Start()
    {
        // Initialize player height and movement tracking
        standingHeight = headset.position.y;
        lastPosition = headset.position;
    }

    void Update()
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
            case ExerciseSelected.SideLunge:
                DetectSideLunge(headX, headZ);
                break;
            case ExerciseSelected.Plank:
                DetectPlank(headY);
                break;
        }
    }

    // ---- Exercise Detection Methods ----

    private void DetectSquat(float headY)
    {
        if (isExerciseActive)
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
    public void SelectSideLunge() => currentExercise = ExerciseSelected.SideLunge;
    public void SelectPlank() => currentExercise = ExerciseSelected.Plank;
    public void DeselectExercise() => currentExercise = ExerciseSelected.None;
    public void ChangeExerciseState() => isExerciseActive = !isExerciseActive;
}

