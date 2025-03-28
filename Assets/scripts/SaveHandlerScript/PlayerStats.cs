using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class User
{
    public Dictionary<string, UserData> userData;
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
    public int strength;
    public int endurance;
    public int agility;
    public int vituality;
    public int currency;
}

[Serializable]
public class OverallExercise
{
    [SerializeField] public int Pushup;
    [SerializeField] public int Squat;
}
