using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class ExerciseLogUI : MonoBehaviour
{

    private FileHandler _fileHandler =new FileHandler();

    [Header("Exercise Log UI")]
    public ExerciseLog exerciseLog; // Assign via Inspector or script
    public GameObject entryPrefab; // The UI prefab for a single log entry
    public Transform contentParent; // The Content object under Scroll View

    [SerializeField] string exerciseLogJsonData;
    [SerializeField] string exerciseLogFilePath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        void OnEnable()
    {
        exerciseLogFilePath = _fileHandler.GetFilePath("ExerciseLogs");
        if(File.Exists(exerciseLogFilePath))
        {
            exerciseLogJsonData = _fileHandler.LoadData("ExerciseLogs");
            exerciseLog = JsonConvert.DeserializeObject<ExerciseLog>(exerciseLogJsonData);

            foreach (var entry in exerciseLog.ExerciseLogList)
            {
                GameObject item = Instantiate(entryPrefab, contentParent);
                item.transform.Find("ExerciseNameText").GetComponent<TMP_Text>().text = entry.ExerciseName;
                item.transform.Find("RepsText").GetComponent<TMP_Text>().text = "Reps: " + entry.Reps.ToString();
                item.transform.Find("DateText").GetComponent<TMP_Text>().text = entry.Date;
                item.transform.Find("TimeText").GetComponent<TMP_Text>().text = entry.Time;
            }

        }
        
    }
}
