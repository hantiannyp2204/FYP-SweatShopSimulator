using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MacineFab : MonoBehaviour, Iinteractable
{
    public Power power;
    public GameObject _Wheel;
    public GameObject _TextHolder;
    public GameObject _StartButton;
    public GameObject _NextButton;
  

    public GameObject _WinORLose;
    public NewController newController; // Reference to the NewController script


    private void Awake()
    {
        
    }
    void Start()
    {
        // Find the persistent GameObject by its name
        GameObject persistentManager = GameObject.Find("MonkeyNuts");

        // Get the Power script attached to the persistent GameObject
        if (persistentManager != null)
        {
            power = persistentManager.GetComponent<Power>();

            // Load the saved power level from PlayerPrefs after finding the persistent GameObject
            power.finalPower = PlayerPrefs.GetFloat("FinalPower", power.finalPower);
            Debug.Log(power.finalPower);
        }
        else
        {
            Debug.LogError("Game not found!");
        }
    }

    public bool CanInteract()
    {
        return true;
    }

    public float GetInteractingLast()
    {
        throw new System.NotImplementedException();
    }

    public string GetInteractName() => "Use " + name;

    public void Interact(KeyboardGameManager player)
    {
        
        Item currentItem = player.playerInventory.GetCurrentItem();

        if (currentItem == null /*|| power.finalPower <= 0*/)
        {
            return;
        }
        else
        {
           _Wheel.SetActive(true);
           _TextHolder.SetActive(true);
            //newController.enabled = true; // Enable the NewController script
            //SceneManager.LoadScene("Minigame");
            Debug.Log("Interacting " + name + " with " + player.playerInventory.GetCurrentItem().Data.name);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int randomValue = Random.Range(350, 1001); // Generates a random integer between 350 and 1000 (inclusive)
            power.finalPower += randomValue;

            // Log the finalPower variable to the console
            Debug.Log("Final Power: " + power.finalPower);

            // Save the updated finalPower value to PlayerPrefs
            PlayerPrefs.SetFloat("FinalPower", power.finalPower);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            // Retrieve the finalPower value from PlayerPrefs
            power.finalPower = PlayerPrefs.GetFloat("FinalPower", power.finalPower);
            // Log the finalPower variable to the console
            Debug.Log("Final Power: " + power.finalPower);
        }

    }

    public void RunActive()
    {
        Debug.Log("Machine Active");
    }

    public void RunDective()
    { 
        Debug.Log("Machine NOt Active");
    }

    public void ToggleOn()
    {

        newController.hold = true;
        _Wheel.SetActive(true);
        _TextHolder.SetActive(true);
        _NextButton.SetActive(true);
        _StartButton.SetActive(true);
    }

    public void ToggleOFF()
    {
        newController.hold = false;
        _Wheel.SetActive(false);
        _TextHolder.SetActive(false);
    }
    /// <summary>
    /// 
    /// </summary>
    public void StartButtonToggle()
    {
       
        newController.hold = true;
        
        Debug.Log("Machine Active");
    }

    public void StartButtonToggleOFF()
    {
       
        newController.hold = false;
        Debug.Log("Machine Not Active");
    }

    /// <summary>
    /// 
    /// </summary
    /// 


}
