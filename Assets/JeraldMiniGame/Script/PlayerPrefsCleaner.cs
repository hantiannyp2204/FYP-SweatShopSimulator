using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PlayerPrefsCleaner
{
    static PlayerPrefsCleaner()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefs deleted as you exited play mode.");
        }
    }
}
