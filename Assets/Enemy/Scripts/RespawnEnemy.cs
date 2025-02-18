using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmeraldAI;

public class RespawnEnemy : MonoBehaviour
{
    EmeraldSystem EmeraldComponent;

    //Cache the EmeraldSystem component
    void Start ()
    {
        EmeraldComponent = GetComponent<EmeraldSystem>();
    }
    
    //Instantly kill the AI this script is assigned to after pressing the H key
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            EmeraldAPI.Combat.ResetAI(EmeraldComponent);
        }
    }
}