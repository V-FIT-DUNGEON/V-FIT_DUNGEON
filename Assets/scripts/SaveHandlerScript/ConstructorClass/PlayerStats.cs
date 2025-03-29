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
}

[Serializable]
public class UserStat
{
    public int Strength;
    public int Endurance;
    public int Agility;
    public int Vitality;
    public int Currency;
}

[Serializable]
public class OverallExercise
{
    [SerializeField] public int Pushup;
    [SerializeField] public int Squat;
}
