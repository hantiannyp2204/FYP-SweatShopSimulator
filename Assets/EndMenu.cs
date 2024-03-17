using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    [SerializeField] TMP_Text FinalScoreTxt;


    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void GameEnd(float scoreNeeded, float finalScore)
    {
        //stop all game stuff and turn on cursor
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //show the score
        FinalScoreTxt.text = $"Game Over\nScore: {finalScore}/{scoreNeeded}";

        //print if player wins or lose
        if(finalScore >= scoreNeeded)
        {
            FinalScoreTxt.text += "\n\n You WON!!!";
        }
        else
        {
            FinalScoreTxt.text += "\n\n You LOST!!!!";
        }

    }
}
