using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class PreExerciseCalibration : MonoBehaviour
{
    [Header("Set up device")]
    [SerializeField] Transform Controller_R;
    [SerializeField] Grabber Grabber_R;

    [SerializeField] Transform Controller_L;
    [SerializeField] Grabber Grabber_L;

    [SerializeField] Transform Headset;

    [SerializeField] ExerciseManager _ExerciseManager;

    public enum ExercisePose{Null, Squat, PushUp, Plank}
    [SerializeField] private ExercisePose exercisePose = ExercisePose.Null;
    [SerializeField] float PosChangeThreshold = 0.7f; // Threshold to trigger recalibration
    [SerializeField] float CalibrationTime = 3f; // Time to hold still for recalibration
    [SerializeField] bool isCalibrating = false;
    [SerializeField] float countTime;
    [SerializeField] Vector3 calibPos;
    private Coroutine _onlyHeadCalibration;

        void OnEnable()
    {
        calibPos = Headset.localPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CaseHandler(bool Calibrated)
    {
        isCalibrating = !Calibrated;
        if(isCalibrating)
        {
            switch(exercisePose)
                {
                    case ExercisePose.Squat:
                        OnlyHeadCalribration();
                        Debug.Log("Squat Pose Detected");
                        break;
                    case ExercisePose.PushUp:
                        Debug.Log("PushUp Pose Detected");
                        break;
                    case ExercisePose.Plank:
                        Debug.Log("Plank Pose Detected");
                        break;
                }
        }
    }

    public void OnlyHeadCalribration()
    {
        while(countTime < CalibrationTime)
        {
            
            if(Vector3.Distance(calibPos, Headset.localPosition) > PosChangeThreshold)
            {
                Debug.Log("Calibration reset.");
                countTime = 0;
                calibPos = Headset.localPosition;
            }
            else
            {
                Debug.Log("Calibrating...");
                countTime += Time.deltaTime;
            }
        }

        InputBridge.Instance.VibrateController(1f, 10f, 1f, Grabber_L.HandSide);
        InputBridge.Instance.VibrateController(1f, 10f, 1f, Grabber_R.HandSide);
        Debug.Log("Calibration complete.");
        countTime = 0;
        isCalibrating = false;
        _ExerciseManager.SetCalibrated(true);


    }

    public void SetExercisePoseSquat() => exercisePose = ExercisePose.Squat;
    public void SetExercisePosePushUp() => exercisePose = ExercisePose.PushUp;

}
