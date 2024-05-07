using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuVR : MonoBehaviour
{
    public GameObject _PauseMenu;

    public bool _ActivePauseMenu = true;
    void Start()
    {
        Display_PauseMenu();
    }
    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Display_PauseMenu();
        }
    }
    public void Display_PauseMenu()
    {
        if (_ActivePauseMenu)
        {
            _PauseMenu.SetActive(false);
            _ActivePauseMenu = false;
            Time.timeScale = 1.0f;
        }
        else if (!_ActivePauseMenu)
        {
            _PauseMenu.SetActive(true);
            _ActivePauseMenu = true;
            Time.timeScale = 0f;
        }
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
