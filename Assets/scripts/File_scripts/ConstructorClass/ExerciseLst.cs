using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseLst
{
    public List<Exercise_Pose> exercise_poses; //initialize list of exercise poses

    public ExerciseLst()
    {
        exercise_poses = new List<Exercise_Pose>();
    }

    public List<Exercise_Pose> GetExercisePoses()
    {
        return exercise_poses;
    }

}

public class Pose
{
    public Pose()
    {
        _pose_name = "";
        _pose_description = "";
        _body_parts = new List<Body_part>();
        _pose_animation = new Animation();
        _poseInstuction = new PoseInstuction();
    }
    public string _pose_name;
    public string _pose_description;
    public List<Body_part> _body_parts;
    public Animation _pose_animation;
    public PoseInstuction _poseInstuction;
}

public class PoseInstuction
{
    public string instruction;
    public string image;
    public string video;
    public string audio;
    public string text;
    public string gif;
    public string pose_name;
    public string pose_description;
    public List<Body_part> body_parts;
    public Animation pose_animation;
    public PoseInstuction()
    {
        instruction = "";
        image = "";
        video = "";
        audio = "";
        text = "";
        gif = "";
        pose_name = "";
        pose_description = "";
        body_parts = new List<Body_part>();
        pose_animation = new Animation();
    }
}

public enum Body_part
{
    BackTrap,
    MiddleTrap,
    LowerBack,
    RearShoulder,
    Tricep,
    BackForearm,
    Lat,
    Glute,
    Hamstring,
    BacCalves,
    FrontTrap,
    FrontShoulder,
    Chest,
    Biceps,
    FrontForearms,
    Obliques,
    Quads,
    FrontCalves,
}

