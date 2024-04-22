using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

public class RefillFuelManager : MonoBehaviour, Iinteractable
{
    [HideInInspector] public UnityEvent AddFuelEvent;
    [HideInInspector] public bool activateRefill;

    [SerializeField] private MachineShredder shredder;
    [SerializeField] private Item refillCan;

    private TMP_Text _textAboveStation;

    [Header("S")]
    [SerializeField] private int fulfilledCriteria;

    [Header("D")]
    [SerializeField] private int incrementPostCriteria;
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

        if (AddFuelEvent == null)
        {
            AddFuelEvent = new UnityEvent();
        }

        AddFuelEvent.AddListener(ActivateFuelRefill);
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
            if (shredder.AlreadyFull())
            {
                shredder.SetWheelStatus(true);
                shredder.initShredding = true;
                return;
            }

            if (shredder.secretHealth <= fulfilledCriteria)
            {
                shredder.secretHealth += 1 * Time.deltaTime;

                if (shredder.AlreadyFull())
                {
                    shredder.SetWheelStatus(true);
                    shredder.initShredding = true;
                    return;
                }
            }
            else
            {
                if (shredder.AlreadyFull())
                {
                    shredder.initShredding = true;
                    return;
                }
                shredder.secretHealth = fulfilledCriteria;
                fulfilledCriteria += incrementPostCriteria;
                activateRefill = false;
            }
            {
                //else
                //{
                //    _textAboveStation.gameObject.SetActive(false);
                //    activateRefill = false;
                //    return;
                //}

                //// Logic for if out of fuel
                //if (shredder.secretHealth <= shredder.maxHealth)
                //{
                //    shredder.secretHealth += 1  * Time.deltaTime;
                //}
                //else // Reached Max Health
                //{
                //    _textAboveStation.gameObject.SetActive(false);
                //    activateRefill = false;
                //    return;
                //}

                //Debug.Log(shredder.secretHealth);
            }
        }
    }

    private void ActivateFuelRefill()
    {
        activateRefill = true;
    }
}
