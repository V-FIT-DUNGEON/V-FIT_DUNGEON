using System.Collections;
using UnityEngine;
using BNG;
using TMPro;
using UnityEngine.Events;

public class PreExerciseCalibration : MonoBehaviour
{
    [Header("Set up device")]
    [SerializeField] private Transform Controller_R;
    [SerializeField] private Grabber Grabber_R;

    [SerializeField] private Transform Controller_L;
    [SerializeField] private Grabber Grabber_L;

    [SerializeField] private Transform Headset;

    [SerializeField] private ExerciseManager _ExerciseManager;

    public enum ExercisePose { Null, Squat, PushUp, Plank }
    //[SerializeField] private ExercisePose exercisePose = ExercisePose.Null;

    [SerializeField] private float scailer = 1000f;
    [SerializeField] private float PosChangeThreshold = 0.6f; // Movement threshold to restart calibration
    [SerializeField] private float CalibrationTime = 5f; // Required time to stay still
    [SerializeField] private bool isCalibrating = false;
    [SerializeField] private float countTime;
    [SerializeField] private Vector3 calibPos;
    [SerializeField] private TextMeshProUGUI RepText;
    private Coroutine calibrationCoroutine;

    [SerializeField] private UnityEvent<float, float> OnCalibrating; // UI feedback for calibration

    private void OnEnable()
    {
        ReassignValue();
        calibPos = Headset.localPosition; // Store initial headset position
    }

    public void CaseHandler(bool Calibrated)
    {
        isCalibrating = !Calibrated;
        if (isCalibrating)
        {
            if (calibrationCoroutine != null)
            {
                StopCoroutine(calibrationCoroutine); // Stop any existing calibration
            }

            calibrationCoroutine = StartCoroutine(CalibrateHeadset());
        }
    }

    private IEnumerator CalibrateHeadset()
    {
        isCalibrating = true;
        countTime = 0;
        calibPos = Headset.localPosition;

        Debug.Log("Starting Calibration... Hold still!");

        while (countTime <= CalibrationTime)
        {
            //Debug.Log("Move" + Vector3.Distance(calibPos, Headset.localPosition)* scailer);
            if (Vector3.Distance(calibPos, Headset.localPosition) * scailer > PosChangeThreshold)
            {
                Debug.Log("Movement detected! Restarting calibration...");
                countTime = 0; // Reset time
                calibPos = Headset.localPosition; // Update reference position
                InputBridge.Instance.VibrateController(1f, 10f, 1f, Grabber_L.HandSide);
                InputBridge.Instance.VibrateController(1f, 10f, 1f, Grabber_R.HandSide);
            }
            else
            {
                countTime += Time.deltaTime; // Increment time while staying still
                OnCalibrating.Invoke(countTime, CalibrationTime); // Update UI feedback
            }

            yield return null; // Wait for next frame
        }

        // Calibration successful
        OnCalibrating.Invoke(0,CalibrationTime); // Update UI feedback
        InputBridge.Instance.VibrateController(1f, 1f, 1f, Grabber_L.HandSide);
        InputBridge.Instance.VibrateController(1f, 1f, 1f, Grabber_R.HandSide);
        Debug.Log("Calibration complete.");
        // wait for 1 second before setting the calibrated state
        yield return new WaitForSeconds(1f);

        OnCalibrating.Invoke(-1,CalibrationTime);
        isCalibrating = false;
        _ExerciseManager.SetCalibrated(true);
    }

    // Methods to set exercise poses
    // public void SetExercisePoseSquat() => exercisePose = ExercisePose.Squat;
    // public void SetExercisePosePushUp() => exercisePose = ExercisePose.PushUp;

    // Reassign value
    private void ReassignValue()
    {
        if (Controller_R == null)
            Controller_R = GameObject.Find("RightControllerAnchor").transform;
        if (Grabber_R == null)
            Grabber_R = GameObject.Find("LeftMainGrabber").GetComponent<Grabber>();
        if (Controller_L == null)
            Controller_L = GameObject.Find("LeftControllerAnchor").transform;
        if (Grabber_L == null)
            Grabber_L = GameObject.Find("RightMainGrabber").GetComponent<Grabber>();
        if (Headset == null)
            Headset = GameObject.Find("CenterEyeAnchor").transform;
        if (_ExerciseManager == null)
            _ExerciseManager = FindObjectOfType<ExerciseManager>();
    }
}
