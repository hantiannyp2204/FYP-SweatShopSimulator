using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] TMP_Text timerTxt;
    private float timer;

    public void SetTimer(float newTime)=>timer = newTime;

    public bool UpdateTimeLeft()
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
    public void UpdateElapsedTime()
    {
        //keep adding the time for unlimited rounds (test chamber)
        timer -= Time.deltaTime;
        // Format the time as a 24-hour clock and display it
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        timerTxt.text = "Time left: " + timeSpan.ToString(@"mm\:ss");
    }
    public void NoTime()
    {
        timerTxt.text = "Time left: Null";
    }
}
