using System.Collections;
using UnityEngine;
using BNG;

public class CalibrationSys : MonoBehaviour
{
    [Space]
    [Header("Set up device")]
    [SerializeField] Transform Controller_R;
    [SerializeField] Grabber Grabber_R;

    [SerializeField] Transform Controller_L;
    [SerializeField] Grabber Grabber_L;

    [SerializeField] Transform Headset;

    [Header("References")]
    [SerializeField] ControllerDetector ControllerDetector;

    [Space]
    [Header("Select Hand")]
    [SerializeField] Transform SelectedJoy;
    [SerializeField] Grabber Grabbering;

    [Header("Settings")]
    [SerializeField] float CalibrationThreshold = 0.7f; // Threshold to trigger recalibration
    [SerializeField] float CalibrationTime = 2f; // Time to hold still for recalibration
    [SerializeField] float MovingBorder = 10f;
    [SerializeField] int scailer = 1000;

    public enum CalibratingState { Active, Calibrating, Idle}

    // Internal variables
    [SerializeField] Vector3 calibPos;
    [SerializeField] private float changedDist;
    [SerializeField] private float countTime;
    [SerializeField] CalibratingState currentState = CalibratingState.Calibrating;
    private Coroutine calibrationCoroutine;

    void OnEnable()
    {
        SwitchDominantHand(true);
        calibPos = SelectedJoy.localPosition;
    }

    void OnDisable()
    {
        calibrationCoroutine = null;
        countTime = 0;
        changedDist = 0;
        currentState = CalibratingState.Calibrating;
    }

    void Update()
    {
        //Debug.Log("CalibrationSys Update");
        MovingCheck();
        switch (currentState)
        {
            case CalibratingState.Active:
                HandleActiveState();
                break;
            case CalibratingState.Calibrating:
                // Calibration is handled by the coroutine
                // Start the calibration coroutine
                HandleCalibratingState();
                break;
            case CalibratingState.Idle:
                //HandleIdleState();
                break;
        }
    }

    public void SwitchDominantHand(bool L_R)
    {
        SelectedJoy = L_R ? Controller_L : Controller_R;
        Grabbering = L_R ? Grabber_L : Grabber_R;
    }

    void HandleCalibratingState()
    {
        // Set calibration position when transitioning to idle state
        calibPos = SelectedJoy.localPosition;
        calibrationCoroutine ??= StartCoroutine(CalibrationRoutine());
    }

    void HandleActiveState()
    {
        ControllerDetector.ActivateControllerDetection();
        currentState = CalibratingState.Idle;
    }

    IEnumerator CalibrationRoutine()
    {
        countTime = 0;

        // Wait for player to stay still for the required calibration time
        while (countTime < CalibrationTime)
        {
            if (changedDist < CalibrationThreshold)
            {
                countTime += Time.deltaTime;  // Player is holding still, progress the calibration
            }
            else
            {
                countTime = 0;  // Reset timer if player moves
                calibPos = SelectedJoy.localPosition;
                //Debug.Log("Player moved during calibration. Resetting...");
            }

            yield return null;
        }

        InputBridge.Instance.VibrateController(0.5f, 0.1f, 1f, Grabbering.HandSide);
        //Debug.Log("Calibration complete.");
        currentState = CalibratingState.Active;  // Switch to active state after successful calibration
        calibrationCoroutine = null;
    }

    void MovingCheck()
    {
        //Debug.Log("MovingCheck");
        changedDist = Vector3.Distance(calibPos, SelectedJoy.localPosition) * scailer;

        //Check if player moved too much and restart calibration if necessary
        if (ControllerDetector.GetDiff() > MovingBorder )
        {
            ControllerDetector.SetDiff(0);
            //Debug.Log("Player moved too much. Starting recalibration...");
            InputBridge.Instance.VibrateController(0.5f, 0.1f, 1f, Grabbering.HandSide);
            calibPos = SelectedJoy.localPosition;
            currentState = CalibratingState.Calibrating;
            ControllerDetector.DeactivateControllerDetection();
        }
    }
}
