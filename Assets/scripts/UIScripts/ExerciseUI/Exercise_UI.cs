using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Exercise_UI : MonoBehaviour
{
    [SerializeField] FileHandler _fileHandler = new();
    [SerializeField] Button ButtonPrefab;
    [SerializeField] Transform ButtonParent;

    [SerializeField] public ExerciseLst exerciseLst = new ExerciseLst();

    // Start is called before the first frame update
    void Start()
    {
        string pose_df = _fileHandler.LoadExercisePoseData("Exercises_info", FileHandler.FileFormat.Json);
        //exerciseLst = JsonConverter.DeserializeObject<ExerciseLst>(pose_df);
        GereratePoseList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GereratePoseList()
    {
        foreach (Pose _pose in exerciseLst.exercise_poses_Lst)
        {
            Debug.Log("start");
            Button newButton = Instantiate(ButtonPrefab, ButtonParent);
            newButton.gameObject.SetActive(true);

            //set label of button to pose name
            //newButton.GetComponentInChildren<Text>().text = pose.pose_name;
        }
    }
}
