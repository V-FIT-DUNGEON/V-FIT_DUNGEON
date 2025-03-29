using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesManager : MonoBehaviour
{
    // Start is called before the first frame update
    void OnSceneLoaded()
    {
        //Debug.Log("Scene Loaded: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void LoadSceneinteraction()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void LoadSceneUITesting()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void LoadSceneDungeonTest()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

}
