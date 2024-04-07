using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public void GoToMainGame()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void GoToTestChamber()
    {
        SceneManager.LoadScene("Test chamber");
    }
}
