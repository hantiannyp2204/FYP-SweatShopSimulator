using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Power : MonoBehaviour
{
    public MacineFab macineFab;
    [SerializeField] private float _StartingPower;
    [SerializeField] private float _PowerToMinus;
    public float currentPower;
    public float finalPower;
    public float newfinalPower;
    public bool canDecreasePower = true;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Minigame")
        {
            // Load newfinalPower from PlayerPrefs
            newfinalPower = PlayerPrefs.GetFloat("finalPower", macineFab.power.finalPower);
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
        if (SceneManager.GetActiveScene().name == "Minigame" && canDecreasePower)
        {
            currentPower -= _PowerToMinus * Time.deltaTime;
        }

        if (currentPower <= 0 )
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
