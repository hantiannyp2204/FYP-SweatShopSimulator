using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VrSceneManager : MonoBehaviour
{

    public void NextLevel()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
