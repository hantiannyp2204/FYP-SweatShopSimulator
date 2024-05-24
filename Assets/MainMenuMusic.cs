using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    [SerializeField] private FeedbackEventData e_mainMenuMusic;
    private void Start()
    {
        e_mainMenuMusic?.InvokeEvent();
    }
}
