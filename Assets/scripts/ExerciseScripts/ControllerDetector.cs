using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using BNG;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum ControllerState {IsMove, IsStill, IsNull};

public class ControllerDetector : MonoBehaviour
{
    [Space]
    [Header("Set up device")]
    [SerializeField] Transform Controller_R;
    [SerializeField] Grabber Grabber_R;

    [SerializeField] Transform Controller_L;
    [SerializeField] Grabber Grabber_L;

    [SerializeField] Transform Headset;
    [SerializeField] float JoyVibration_Amplitude = 0.5f;
    [SerializeField] float JoyVibration_Frequency = 0.1f;
    [SerializeField] float JoyVibration_Duration = 0.1f;

    [Space]
    [Header("Select Hand")]
    [SerializeField] Transform SelectedJoy;
    [SerializeField] Grabber Grabbering;

    [Space]
    [Header("Setting")]
    [SerializeField] float Border = 10f;
    [SerializeField] int scailer = 1000;
    [SerializeField] float StillnessFactor = 0.6f;
    [SerializeField] float PosUpdateDelay = 0.2f;
    [SerializeField] ControllerState CState = ControllerState.IsNull;
    
    [Space]
    [Header("Measurer things")]
    [SerializeField] Vector3 previousJoyPos;
    [SerializeField] Vector3 CurrentJoyPos;
    [SerializeField] Vector3 CurrentHeadsetPos;
    [SerializeField] Transform DistantMeterPos;
    [SerializeField] float M_MaxDist;
    [SerializeField] float M_MinDist;
    [SerializeField] Vector3 MaxDistPoint;
    [SerializeField] float C_Max_JoyDist;
    [SerializeField] float P_Max_JoyDist;
    [SerializeField] Vector3 MinDistPoint;
    [SerializeField] float C_Min_JoyDist;
    [SerializeField] float P_Min_JoyDist;
    [SerializeField] int count_threshold = 3;
    [SerializeField] int count_Away = 0;
    [SerializeField] int count_Closer = 0;
    [SerializeField] int count_Still = 0;

    [Space]
    [Header("Debug thing")]

    [SerializeField] private UnityEvent<int> OnEventInt_InOut; //1(in),0(hold),-1(out), 2(null)
    [SerializeField] float diff;
    public float countTime = 0;
    private Coroutine ControllerDetectionCoroutine;


    public static ControllerDetector Instance {get; private set;}

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SwitchDominantHand(true);
        if (SelectedJoy == null)
        {
            Debug.LogError("SelectedJoy is not set");
        }
    }

    void OnDisable()
    {
        DeactivateControllerDetection();
        diff = 0;
        count_Still = 0;
        count_Away = 0;
        count_Closer = 0;
        OnEventInt_InOut?.Invoke(0);
    }

    void Update()
    {

    }

    public void SwitchDominantHand(bool L_R)
    {
        SelectedJoy = L_R ? Controller_L : Controller_R;
        Grabbering = L_R ? Grabber_L : Grabber_R;
    }

    //set meter position
    void setMeterPos()
    {
        //set meter position to headset position
        DistantMeterPos.localPosition = new Vector3(DistantMeterPos.localPosition.x, Headset.localPosition.y, DistantMeterPos.localPosition.z);
        InputBridge.Instance.VibrateController(JoyVibration_Frequency, JoyVibration_Amplitude, JoyVibration_Duration, Grabber_L.HandSide);
        InputBridge.Instance.VibrateController(JoyVibration_Frequency, JoyVibration_Amplitude, JoyVibration_Duration, Grabber_R.HandSide);
    }

    //Breath Detection Logic
    IEnumerator ExerciseDetection()
    {
        //Debug.Log("Activated");
        yield return null;
        setMeterPos();

        previousJoyPos = new Vector3(SelectedJoy.localPosition.x, SelectedJoy.localPosition.y, SelectedJoy.localPosition.z);
        MaxDistPoint = previousJoyPos;
        MinDistPoint = previousJoyPos;
        M_MinDist = Vector3.Distance(DistantMeterPos.position, MinDistPoint);
        M_MaxDist = Vector3.Distance(DistantMeterPos.position, MaxDistPoint);

        while(true)
        {
            yield return new WaitForSeconds(PosUpdateDelay);
            CurrentJoyPos = new Vector3(SelectedJoy.localPosition.x, SelectedJoy.localPosition.y, SelectedJoy.localPosition.z);
            CurrentHeadsetPos = new Vector3(SelectedJoy.localPosition.x, SelectedJoy.localPosition.y, SelectedJoy.localPosition.z);

            diff = Vector3.Distance(CurrentJoyPos, previousJoyPos) * scailer;
            //Debug.Log(diff);
            
            //change min-max joy location
            if(Vector3.Distance(DistantMeterPos.position, CurrentJoyPos) > M_MaxDist)
            {
                MaxDistPoint = CurrentJoyPos;
                M_MaxDist = Vector3.Distance(DistantMeterPos.position, MaxDistPoint);
            }
            if(Vector3.Distance(DistantMeterPos.position, CurrentJoyPos) < M_MinDist)
            {
                MinDistPoint = CurrentJoyPos;
                M_MinDist = Vector3.Distance(DistantMeterPos.position, MinDistPoint);
            }

            //breath detection
            C_Max_JoyDist = Vector3.Distance(MaxDistPoint, CurrentJoyPos);
            P_Max_JoyDist = Vector3.Distance(MaxDistPoint, previousJoyPos);
            C_Min_JoyDist = Vector3.Distance(MinDistPoint, CurrentJoyPos);
            P_Min_JoyDist = Vector3.Distance(MinDistPoint, previousJoyPos);

            if(CState != ControllerState.IsNull && diff <= Border)
            {
                //Hold Breath
                if(diff <= StillnessFactor)
                {
                    count_Still++;
                    if(count_Still > count_threshold)
                    {
                        //Debug.Log("Hold Breath");
                        count_Away = 0;
                        count_Closer = 0;
                        CState = ControllerState.IsStill;
                        OnEventInt_InOut?.Invoke(0);
                    }
                    else
                    {
                        Invoke_previous(count_Away, count_Closer, count_Still);
                    }
                }
                //Breath in
                else if (C_Min_JoyDist > P_Min_JoyDist && C_Max_JoyDist < P_Max_JoyDist)
                {
                    count_Away++;
                    if(count_Away > count_threshold)
                    {
                        Debug.Log("move away");
                        count_Still = 0;
                        count_Closer = 0;
                        CState = ControllerState.IsMove;
                        InputBridge.Instance.VibrateController(JoyVibration_Frequency, JoyVibration_Amplitude, JoyVibration_Duration, Grabbering.HandSide);
                        //OnEventInt_InOut?.Invoke(1);
                    }
                    
                    else
                    {
                        Invoke_previous(count_Away, count_Closer, count_Still);
                    }
                }
                //Breath out
                else if (C_Min_JoyDist < P_Min_JoyDist && C_Max_JoyDist > P_Max_JoyDist)
                {
                    count_Closer++;
                    if(count_Closer > count_threshold)
                    {
                        Debug.Log("move closerr");
                        count_Still = 0;
                        count_Away = 0;
                        CState = ControllerState.IsMove;
                        InputBridge.Instance.VibrateController(JoyVibration_Frequency, JoyVibration_Amplitude, JoyVibration_Duration, Grabbering.HandSide);
                        OnEventInt_InOut?.Invoke(-1);
                    }
                    else
                    {
                        Invoke_previous(count_Away, count_Closer, count_Still);
                    }
                }
            }
            else
            {
                countTime = 0;
                //Debug.Log("end");
                OnEventInt_InOut?.Invoke(2);
                ControllerDetectionCoroutine = null;
                break;
            }
            
            previousJoyPos = CurrentJoyPos;
        }
    }

    public void ActivateControllerDetection()
    {
        //Debug.Log(breathDetectionCoroutine);
        // Debug.Log(BState);
        if (CState == ControllerState.IsNull && ControllerDetectionCoroutine == null)
        {
            M_MaxDist = 0;
            M_MinDist = 0;
            MaxDistPoint = Vector3.zero;
            MinDistPoint = Vector3.zero;
            Debug.Log("Starting BreathDetection Coroutine");
            ControllerDetectionCoroutine = StartCoroutine(ExerciseDetection());  // Start the coroutine
            CState = ControllerState.IsStill;
        }
        else{
            Debug.Log("Breath detection already Started");
            ControllerDetectionCoroutine = null;
            ControllerDetectionCoroutine = StartCoroutine(ExerciseDetection());  // Start the coroutine
            CState = ControllerState.IsStill;
            Debug.Log("Breath detection Restarted");
        }
        
    }

    public void DeactivateControllerDetection()
    {
        //do something
        //Debug.Log(breathDetectionCoroutine);
        //Debug.Log(BState);
        if (ControllerDetectionCoroutine != null)
        {
            Debug.Log("Stopping BreathDetection Coroutine");
            StopCoroutine(ControllerDetectionCoroutine);  // Stop the coroutine
            ControllerDetectionCoroutine = null;
            diff = 0;
            CState = ControllerState.IsNull;
        }
        else{
            Debug.Log("Breath detection already Stopped");
        }
    }

    void Invoke_previous(int Cin, int Cout, int Chold)
    {
        //This function use for make swing value become previous staet instead of passing it
        if(Cin >= count_threshold)
        {
            InputBridge.Instance.VibrateController(JoyVibration_Frequency, JoyVibration_Amplitude, JoyVibration_Duration, Grabbering.HandSide);
            //BState = BreathState.StateIn;
            OnEventInt_InOut?.Invoke(1);
        }
        else if(Cout >= count_threshold)
        {
            InputBridge.Instance.VibrateController(JoyVibration_Frequency, JoyVibration_Amplitude, JoyVibration_Duration, Grabbering.HandSide);
            //BState = BreathState.StateOut;
            OnEventInt_InOut?.Invoke(-1);
        }
        else if(Chold >= count_threshold)
        {
            InputBridge.Instance.VibrateController(JoyVibration_Frequency, JoyVibration_Amplitude, JoyVibration_Duration, Grabbering.HandSide);
            //BState = BreathState.StateOut;
            OnEventInt_InOut?.Invoke(0);
        }
    }

    public float GetDiff()
    {
        return diff;
    }

    //get set diff
    public void SetDiff(float _diff)
    {
        diff = _diff;
    }

    // public BreathState GetState()
    // {
    //     return BState;
    // }

}
