using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

public class RefillFuelManager : MonoBehaviour, Iinteractable
{
    [Range(0.1f, 0.5f)] public float fuelIncrease;
    [HideInInspector] public UnityEvent AddFuelEvent;
    [HideInInspector] public bool activateRefill;

    public MachineShredder shredder;
    [SerializeField] private Item refillCan;

    //private TMP_Text _textAboveStation;

    [Header("FEEDBACK")]
    public FeedbackEventData e_refillFuel;

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
        if (AddFuelEvent == null)
        {
            AddFuelEvent = new UnityEvent();
        }

        AddFuelEvent.AddListener(ActivateFuelRefill);
    }

    // Update is called once per frame
    void Update()
    {
        if (activateRefill)
        {
            if (shredder.AlreadyFull())
            {
                shredder.SetWheelStatus(true);
                shredder.ResetWheelValue(); // after done reset so the health will not go down
                shredder.initShredding = true;
                activateRefill = false;
                return;
            }
 
           


            // old refill code
            {
                //if (shredder.secretHealth <= fulfilledCriteria)
                //{
                //    shredder.secretHealth += amt * Time.deltaTime;

                //    if (shredder.AlreadyFull())
                //    {
                //        shredder.ResetWheelValue(); // after done reset so the health will not go down
                //        shredder.SetWheelStatus(true);
                //        shredder.initShredding = true;
                //        return;
                //    }
                //}
                //else
                //{
                //    shredder.secretHealth = fulfilledCriteria;
                //    fulfilledCriteria += incrementPostCriteria;
                //    activateRefill = false;
                //}
            }
        }
    }

    private void ActivateFuelRefill()
    {
        activateRefill = true;

        float amt = shredder.maxHealth * fuelIncrease;
        shredder.secretHealth += amt * Time.deltaTime;
    }
}
