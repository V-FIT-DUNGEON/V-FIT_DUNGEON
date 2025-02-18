using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EmeraldAI.Utility;

namespace EmeraldAI
{
    public class RespawnEnemy : MonoBehaviour
    {

        //The seconds needed before respawning
        public int RespawnSeconds = 10;

        //The reference to the Emerald AI system
        EmeraldSystem EmeraldAIReference;

        //The respawn timer that will track the seconds passed
        float RespawnTimer;

        void Start()
        {
            //Get the reference to the Emerald AI system
            EmeraldAIReference = GetComponent<EmeraldSystem>();
        }

        void Update()
        {
            //Increase the RespawnTimer, but only when the AI has been killed
            if (EmeraldAIReference.HealthComponent.CurrentHealth <= 0)
            {
                RespawnTimer += Time.deltaTime;

                if (RespawnTimer >= RespawnSeconds)
                {
                    //Reset the AI and set the timer back to 0 to be used again.
                    EmeraldAIReference.ResetAI();
                    RespawnTimer = 0;
                }
            }
        }
    }
}