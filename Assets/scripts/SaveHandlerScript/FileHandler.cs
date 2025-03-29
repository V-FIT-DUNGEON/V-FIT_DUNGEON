using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class FileHandler
{
    public enum FileFormat {None, csv, Save, Profile, Json}
    public enum FileType {None, ExerciseLog, PlayerStats}
    //This script is used to save the data of Breathing into a CSV file
    // Start is called before the first frame update

#if UNITY_EDITOR
    private readonly string directoryPath = $"{Application.dataPath}/SavedData/Save";
    private readonly string exerciseLogsDirectoryPath = $"{Application.dataPath}/SavedData/ExercisesLogs";
    private readonly string PlayerStatsDirectoryPath = $"{Application.dataPath}/SavedData/PlayerStats";
#else
    private readonly string directoryPath = $"{Application.persistentDataPath}/SavedData/Save";
    private readonly string exerciseLogsDirectoryPath = $"{Application.dataPath}/SavedData/ExercisesLogs";
    private readonly string PlayerStatsDirectoryPath = $"{Application.dataPath}/SavedData/PlayerStats";
#endif
    private readonly string[] FILE_EXTENSION = { "", ".csv", ".save", ".profile", ".json" };

    public string GetExercisePoseFilePath(string fileName, FileFormat format)
    {
        return Path.Combine(exerciseLogsDirectoryPath, fileName + FILE_EXTENSION[(int)format]);
    }

    // ----- Get File Path ----
    public string GetFilePath(string fileName)
    {
        switch (fileName)
        {
            case "ExerciseLogs":
                return Path.Combine(exerciseLogsDirectoryPath, fileName + FILE_EXTENSION[4]);
            case "PlayerStats":
                return Path.Combine(PlayerStatsDirectoryPath, fileName + FILE_EXTENSION[4]);
            case "ExercisePose":
                return Path.Combine(directoryPath, fileName + FILE_EXTENSION[4]);
            default:
                Debug.LogError($"<b><color=#ED1D24>[Error]</color></b> Invalid file name: {fileName}");
                return string.Empty;
        }
    }

    // ----- Save File ----
    public void SaveData(string fileName, object data)
    {
        string filePath = GetFilePath(fileName);
        string directoryPath = Path.GetDirectoryName(filePath);

        try 
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fileData = JsonUtility.ToJson(data, true); // Pretty print for better readability

            using FileStream stream = new(filePath, FileMode.Create);
            using StreamWriter writer = new(stream);
    
            try
            {
                writer.Write(fileData);
                Debug.Log($"<b><color=#00E69C>[Save]</color></b> Saved file data at: {filePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write data: {e.Message}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save file: {e.Message}");
        }
    }

    // ----- Load File ----
    public string LoadData(string fileName)
    {
        string filePath = GetFilePath(fileName);

        try
        {
            if (File.Exists(filePath))
            {
                using FileStream stream = new(filePath, FileMode.Open);
                using StreamReader reader = new(stream);
                string fileData = reader.ReadToEnd();
                Debug.Log($"<b><color=#00E69C>[Load]</color></b> Loaded file data from: {filePath}");
                return fileData;
            }
            else
            {
                Debug.LogError($"<b><color=#ED1D24>[Error]</color></b> File not found: {filePath}");
                return string.Empty;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load file: {e.Message}");
            return string.Empty;
        }
    }
    

    public void DeleteDirectory(string directoryName)
    {
        try
        {
            if (Directory.Exists(Path.Combine(directoryPath, directoryName)))
            {
                Directory.Delete(Path.Combine(directoryPath, directoryName), true);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"<b><color=#ED1D24>[Error]</color></b> Error occured when trying to delete directory: {e}");
        }
    }
}
