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
    //This script is used to save the data of Breathing into a CSV file
    // Start is called before the first frame update

#if UNITY_EDITOR
    private readonly string directoryPath = $"{Application.dataPath}/SavedData/Save";
    private readonly string exerciseDirectoryPath = $"{Application.dataPath}/SavedData/Exercises";
#else
    private readonly string directoryPath = $"{Application.persistentDataPath}/SavedData/Save";
    private readonly string exerciseDirectoryPath = $"{Application.dataPath}/SavedData/Exercises";
#endif
    private readonly string[] FILE_EXTENSION = { "", ".csv", ".save", ".profile", ".json" };

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetExercisePoseFilePath(string fileName, FileFormat format)
    {
        return Path.Combine(exerciseDirectoryPath, fileName + FILE_EXTENSION[(int)format]);
    }

    public string GetUserFilePath(string profileID, string fileName, FileFormat format)
    {
        return Path.Combine(directoryPath, profileID, fileName + FILE_EXTENSION[(int)format]);
    }

    public string GetFilePath(string fileName, FileFormat format)
    {
        return Path.Combine(directoryPath, fileName + FILE_EXTENSION[(int)format]);
    }

    public void SaveData(string profileID, string fileName, object data, FileFormat format)
    {
        //check if profileID is provided and handle as needed
        string filePath = profileID != null ? GetUserFilePath(profileID, fileName, format) : GetFilePath(fileName, format);
        string fileDirectory = Path.GetDirectoryName(filePath);
        string fileData = string.Empty;

        try
        {
            if(!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);

            using FileStream stream = new(filePath, FileMode.Create);
            using StreamWriter writer = new(stream);

            try
            {
                writer.Write(data);
                Debug.Log($"<b><color=#00E69C>[Save]</color></b> Saved file data at: {filePath}");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }   
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public string LoadExercisePoseData(string fileName, FileFormat format)
    {
        string filePath = GetExercisePoseFilePath(fileName, format);
        string fileData = string.Empty;
        string fileDirectory = Path.GetDirectoryName(filePath);

        try
        {
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);

            using FileStream stream = new(filePath, FileMode.Open);
            using StreamReader reader = new(stream);

            try
            {
                fileData = reader.ReadToEnd();
                Debug.Log($"<b><color=#00E69C>[Load]</color></b> Loaded file data at: {filePath}");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        return fileData;
    }
    public string LoadSaveData(string fileName, FileFormat format, string profileID = null)
    {
        // Check if profileID is provided and handle as needed
        string filePath = profileID != null ? GetUserFilePath(profileID, fileName, format) : GetFilePath(fileName, format);
        string fileData = string.Empty;
        string fileDirectory = Path.GetDirectoryName(filePath);

        try
        {
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);

            using FileStream stream = new(filePath, FileMode.Open);
            using StreamReader reader = new(stream);

            try
            {
                fileData = reader.ReadToEnd();
                Debug.Log($"<b><color=#00E69C>[Load]</color></b> Loaded file data at: {filePath}");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        return fileData;
    }

    public void Delete(string profileID, string fileName, FileFormat format)
    {
        if(string.IsNullOrEmpty(profileID))
            return;

        string filePath = GetUserFilePath(profileID, fileName, format);
        string fileDirectory = Path.GetDirectoryName(filePath);

        try
        {
            if(format == FileFormat.Profile)
                return;//Delete the whole profile}
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
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
