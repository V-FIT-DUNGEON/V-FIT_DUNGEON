using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExerciseLog
{
    public List<ExerciseEntry> ExerciseLogList;
}

[Serializable]
public class ExerciseEntry
{
    public string ExerciseName;
    public int Reps;
    public string Date;
    public string Time; // Keep as string for simplicity; use DateTime if needed
}