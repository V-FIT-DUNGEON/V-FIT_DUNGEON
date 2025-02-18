using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSceneinteraction()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void LoadSceneUITesting()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void LoadSceneEnemyTest()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

}
