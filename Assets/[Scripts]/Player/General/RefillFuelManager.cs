using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class RefillFuelManager : MonoBehaviour, Iinteractable
{
    [SerializeField] private MachineShredder shredder;
    [SerializeField] private Item refillCan;
    public bool activateRefill;

    private TMP_Text _textAboveStation;
    public bool CanInteract()
    {
        return true;
    }

    public float GetInteractingLast()
    {
        throw new System.NotImplementedException();
    }

    public string GetInteractName()
    {
        return "RefillFuelMachine";
    }

    public void Interact(KeyboardGameManager player)
    {
        if (!shredder.IsOutOfFuel() || player.playerInventory.GetCurrentItem() != refillCan)
        {
            return; // Dont do anything if shredder is not completely out of fuel
        }
        else 
        {
            activateRefill = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _textAboveStation = GetComponentInChildren<TMP_Text>();
        _textAboveStation.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (shredder.IsOutOfFuel())
        {
            _textAboveStation.text = "Use me!";
            _textAboveStation.gameObject.SetActive(true);
        }

        if (activateRefill)
        {
            // Logic for if out of fuel
            if (shredder.secretHealth <= shredder.maxHealth)
            {
                shredder.secretHealth += 1  * Time.deltaTime;
            }
            else // Reached Max Health
            {
                _textAboveStation.gameObject.SetActive(false);
                activateRefill = false;
                return;
            }

            Debug.Log(shredder.secretHealth);
        }
    }
}
