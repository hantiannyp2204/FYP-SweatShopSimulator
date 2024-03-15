using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Timers;
using UnityEditor.Rendering;


public class RequestBox : MonoBehaviour,Iinteractable
{
    [SerializeField] private float pointsMax;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text pointText;
    bool boxOpened = false;
    ItemData requestedItem;

    private float _timer;
    private float _points;
    private float _tracker;

    public delegate void OrderInteractionHandler();
    public static event OrderInteractionHandler OnOrderProcessed;
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
        return "open request";
    }

    public void Interact(GameManager player)
    {
        if(!boxOpened)
        {
            //get the request
            player.playerObjective.UpdateObjetcive(requestedItem.GetName());
            boxOpened = true;
        }
        else
        {
            //check player's current equip
            Item playerCurrentEquip = player.playerInventory.GetCurrentItem();
            //see if its blank or not correct item
            if (playerCurrentEquip == null || playerCurrentEquip.Data != requestedItem)
            {
                return;
            }
            //if correct, remove it and set into the box as parent
            player.playerInventory.RemoveAtCurrentSlot(true,false);

            // Change parent
            playerCurrentEquip.transform.SetParent(this.transform);
            playerCurrentEquip.transform.position = this.transform.position;
            //and send object over
            OnOrderProcessed?.Invoke();

            //reset objective
            player.playerObjective.ResetObjective();

            boxOpened = false;

        }
    }
    private void Start()
    {
        _points = pointsMax;
    }
    private void Update()
    {
        if (boxOpened)
        {
            pointText.text = "Points :  " + _points;

            // Start timer when receive requests
            _timer += Time.deltaTime;
            _tracker += Time.deltaTime;

            if (_tracker >= 5)
            {
                _points -= 100;
                if (_points <= 0)
                {
                    Debug.Log("Jerald islesbian");
                    _points = 0;
                    pointText.text = "Points :  " + _points;
                    return;
                }
                pointText.text = "Points :  " + _points;
                _tracker = 0;
            }
            timerText.text = "Time Being Taken: " + (int)_timer;
        }
    }
    public void SetRequestedItem(ItemData newRequestedItem) => requestedItem = newRequestedItem;
}
