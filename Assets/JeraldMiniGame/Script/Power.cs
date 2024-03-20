using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Power : MonoBehaviour
{
    public Controller controller;
    [SerializeField] private float _StartingPower;
    [SerializeField] private float _PowerToMinus;
    public float currentPower;
    public float finalPower;
    // Start is called before the first frame update
    void Start()
    {
         currentPower = _StartingPower;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Minigame")
        {
            currentPower -= _PowerToMinus * Time.deltaTime;
        }

       

        if (currentPower <= 0)
        {
            finalPower = currentPower;
            NoPower();
        }
    }

    private void NoPower()
    {
        // Save the current power level to PlayerPrefs
        PlayerPrefs.SetFloat("CurrentPower", finalPower);
    }
}
