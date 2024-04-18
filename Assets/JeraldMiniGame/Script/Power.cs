using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Power : MonoBehaviour
{
    public Controller controller;
    public MacineFab macineFab;
    
    [SerializeField] private float _StartingPower;
    [SerializeField] private float _PowerToMinus;
    public float currentPower;
    public float finalPower;
    public float newfinalPower;
    public bool canDecreasePower = true;
    int currentlevel;
    void Start()
    {
        // Get the current level number
        //currentlevel = PlayerPrefs.GetInt("Level num", controller.Lnum);

        if (SceneManager.GetActiveScene().name == "Minigame")
        {
            // Load newfinalPower from PlayerPrefs based on the current level
            for (int i = 1; i <= 5; i++)
            {
                if (controller.Lnum == i)
                {
                    newfinalPower = PlayerPrefs.GetFloat("FinalPower", newfinalPower);
                    break; // Exit loop once the correct level is found
                }
            }

            Debug.Log(newfinalPower);

            // Assign newfinalPower to currentPower
            currentPower = newfinalPower;
        }
        else
        {
            // If the scene is not "Minigame", initialize currentPower with _StartingPower
            currentPower = _StartingPower;
        }

        Debug.Log(currentPower);
    }



    // Update is called once per frame
    void Update()
    {
        //currentlevel = PlayerPrefs.GetInt("Level num", controller.Lnum);
        

        if (SceneManager.GetActiveScene().name == "Minigame" && canDecreasePower)
        {
            currentPower -= _PowerToMinus * Time.deltaTime;
        }

        if (currentPower <= 0 )
        {
            finalPower = currentPower;
            NoPower();
        }

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    Debug.Log("Level");
        //    Debug.Log(currentlevel);
        //}


    }

    private void NoPower()
    {
        // Save the current power level to PlayerPrefs
        PlayerPrefs.SetFloat("CurrentPower", finalPower);
    }

  

}
