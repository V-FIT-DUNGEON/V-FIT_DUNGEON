using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Kryz.CharacterStats.Examples;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

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
    [SerializeField] private User _user; // user
    [SerializeField] private UserData _userData; // user data
    [SerializeField] private UserStat _userStat; // user stat
    [SerializeField] private OverallExercise _overallExercise; // overall exercise
    [SerializeField] private int AllSquat = 0; // Player currency
    [SerializeField] private int AllPushUp = 0; // Player level


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

    private ExerciseLog _exerciseLog; // Exercise log for saving data
    private ExerciseEntry _exerciseEntry; // Exercise entry for saving data


    [Header("Events")]
    [SerializeField] private UnityEvent<bool> OnCalibrationEvent; // UI feedback for calibration
    [SerializeField] private UnityEvent<int> OnSquatRepsCountEvent; // UI feedback for Squat reps
    [SerializeField] private UnityEvent<int> OnPushUpRepsCountEvent; // UI feedback for Push-Up reps
    [SerializeField] private UnityEvent<int> OnPlankCountEvent; // UI feedback for Plank
    [SerializeField] private UnityEvent<bool> OnFinishExerciseEvent; // UI feedback for exercise completion

    private FileHandler _fileHandler; // File handler for saving data
    string userFilePath; // File path for user data
    string exerciseLogFilePath;
    [SerializeField] string jsonData; // JSON data for saving

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

        _fileHandler = new FileHandler();
        userFilePath = _fileHandler.GetFilePath("PlayerStats");
        exerciseLogFilePath = _fileHandler.GetFilePath("ExerciseLogs");

        // Load user data
        if(!File.Exists(userFilePath))
        {
            Debug.Log("New User Created");
            // Create a new user and initialize data
            _user = new User();
            _user.UserDatas = new Dictionary<string, UserData>();
            _userData = new UserData();
            _userStat = new UserStat();
            _overallExercise = new OverallExercise();
            _userStat.Strength = 0;
            _userStat.Endurance = 0;
            _userStat.Agility = 0;
            _userStat.Vitality = 0;
            _userStat.Currency = 0;
            _overallExercise.Pushup = 0;
            _overallExercise.Squat = 0;
            _userData.UserStat = _userStat;
            _userData.OverallExercise = _overallExercise;
            _user.UserDatas.Add("User", _userData); // Add user data to the dictionary

            // Serialize the data using Newtonsoft.Json
            jsonData = JsonConvert.SerializeObject(_user, Formatting.Indented);
            _fileHandler.SaveData("PlayerStats", jsonData);

            // set stats
            _userData = _user.UserDatas["User"];
            _userStat = _userData.UserStat;
            _overallExercise = _userData.OverallExercise;
            _Strength = _userStat.Strength;
            _Vitality = _userStat.Vitality;
            _Agility = _userStat.Agility;
            _Endurance = _userStat.Endurance;

            
            
        }
        else{
            Debug.Log("User List already exists");
            // Load existing user data
            jsonData = _fileHandler.LoadData("PlayerStats");
            _user = JsonConvert.DeserializeObject<User>(jsonData);
            _userData = _user.UserDatas["User"];
            _userStat = _userData.UserStat;
            _overallExercise = _userData.OverallExercise;
            _Strength = _userStat.Strength;
            _Vitality = _userStat.Vitality;
            _Agility = _userStat.Agility;
            _Endurance = _userStat.Endurance;
            AllPushUp = _overallExercise.Pushup;
            AllSquat = _overallExercise.Squat;
            
        }

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
        //Debug.Log($"Head Y: {headY}");
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
                    AllSquat++;
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
                    AllPushUp++;
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

            SavePlayerStats();
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
            standingHeight = Headset.position.y;
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

            SavePlayerStats();
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
                _PlayerCharacter.Strength.BaseValue = _Strength;
                _PlayerCharacter.Vitality.BaseValue = _Vitality;
                _PlayerCharacter.Agility.BaseValue = _Agility;
                _PlayerCharacter.Endurance.BaseValue = _Endurance;

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
    public void SaveExerciseData()
    {
        _fileHandler = new FileHandler();
        string fileName = "ExerciseData.json";
        string filePath = _fileHandler.GetFilePath(fileName);

        // Create a dictionary to hold the exercise data
        Dictionary<string, object> exerciseData = new Dictionary<string, object>
        {
            { "Strength", _Strength },
            { "Vitality", _Vitality },
            { "Agility", _Agility },
            { "Endurance", _Endurance },
            { "RepsCount", repsCount }
        };

        // Convert the dictionary to JSON format
        string jsonData = JsonUtility.ToJson(exerciseData);

        // Save the JSON data to a file
        _fileHandler.SaveData(fileName, jsonData);
    }

    public void SavePlayerStats()
    {
        _userData = _user.UserDatas["User"];
        _userStat = _userData.UserStat;
        _overallExercise = _userData.OverallExercise;
        _userStat.Strength = _Strength;
        _userStat.Vitality = _Vitality;
        _userStat.Agility = _Agility;
        _userStat.Endurance = _Endurance;
        
        _overallExercise.Pushup = AllPushUp;
        _overallExercise.Squat = AllSquat;

        jsonData = JsonConvert.SerializeObject(_user, Formatting.Indented);
        _fileHandler.SaveData("PlayerStats", jsonData);
    }
}
