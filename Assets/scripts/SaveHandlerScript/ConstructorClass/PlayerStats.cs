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
            "Currency" => UserStat.Currency,
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
    public float Currency;
}

[Serializable]
public class OverallExercise
{
    [SerializeField] public int Pushup;
    [SerializeField] public int Squat;
}
