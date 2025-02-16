using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Exercise_UI : MonoBehaviour
{
    [SerializeField] FileHandler _fileHandler = new();
    [SerializeField] Button ButtonPrefab;
    [SerializeField] Transform ButtonParent;

    [SerializeField] ExerciseLst exerciseLst = new ExerciseLst();

    // Start is called before the first frame update
    void Start()
    {
        string pose_df = _fileHandler.LoadExercisePoseData("Exercises_info", FileHandler.FileFormat.Json);
        exerciseLst = JsonUtility.FromJson<ExerciseLst>(pose_df);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GereratePoseList()
    {
        foreach (var pose in exerciseLst.GetExercisePoses())
        {
            Button newButton = Instantiate(ButtonPrefab, ButtonParent);
            newButton.gameObject.SetActive(true);

            //set label of button to pose name
            //newButton.GetComponentInChildren<Text>().text = pose.pose_name;   
        }
    }
}
