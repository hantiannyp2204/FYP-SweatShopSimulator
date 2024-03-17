using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    float timer;
    [SerializeField] TMP_Text timerTxt;
    public void SetTimer(float newTime)=>timer = newTime;

    public bool UpdateTime()
    {
        //reduce time by one if not reached
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            // Format the time as a 24-hour clock and display it
            TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
            timerTxt.text = "Time left: "+timeSpan.ToString(@"mm\:ss");
            return false;
        }
        else
        {
            //check win/lose condition
            timer = 0;
            return true;
        }
    }    
}
