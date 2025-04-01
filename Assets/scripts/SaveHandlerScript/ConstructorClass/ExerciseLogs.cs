using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExerciseLog
{
    public Dictionary<string, List<ExerciseEntry>> ExerciseLogList = new Dictionary<string, List<ExerciseEntry>>();

    public void AddExerciseLog(string exerciseName, int reps, string date, string time)
    {
        ExerciseEntry newEntry = new ExerciseEntry
        {
            ExerciseName = exerciseName,
            Reps = reps,
            Time = time
        };

        // If this date doesn't exist in the dictionary, add it
        if (!ExerciseLogList.ContainsKey(date))
        {
            ExerciseLogList[date] = new List<ExerciseEntry>();
        }

        // Insert at the top of the list for that date
        ExerciseLogList[date].Insert(0, newEntry);
    }
}

[Serializable]
public class ExerciseEntry
{
    public string ExerciseName;
    public int Reps;
    public string Time;
}


