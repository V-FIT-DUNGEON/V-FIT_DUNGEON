using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class User
{
    public Dictionary<string, UserData> UserDatas;
}

[Serializable]
public class UserData
{
    public UserStat UserStat;
    public OverallExercise OverallExercise;

    public float GetStat(string statName)
    {
        return statName switch
        {
            "Strength" => UserStat.Strength,
            "Endurance" => UserStat.Endurance,
            "Agility" => UserStat.Agility,
            "Vitality" => UserStat.Vitality,
            _ => 0f
        };
    }

    public float GetOverallExercise(string exerciseName)
    {
        return exerciseName switch
        {
            "Pushup" => OverallExercise.Pushup,
            "Squat" => OverallExercise.Squat,
            _ => 0f
        };
    }
}

[Serializable]
public class UserStat
{
    public float Strength;
    public float Endurance;
    public float Agility;
    public float Vitality;
}

[Serializable]
public class OverallExercise
{
    [SerializeField] public int Pushup;
    [SerializeField] public int Squat;
}
