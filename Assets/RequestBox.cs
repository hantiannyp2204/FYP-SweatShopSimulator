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

    bool boxOpened = false;
    ItemData requestedItem;

    private float _timer;
    private float _pointsToReward;
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
            ResetPointTracker();
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

            //lastly, give points accordingly
            GameManager.AddScore(_pointsToReward);

            //reset objective
            player.playerObjective.ResetObjective();

            boxOpened = false;

        }
    }
    private void Update()
    {
        if (boxOpened)
        {

            // Start timer when receive requests
            _timer += Time.deltaTime;
            _tracker += Time.deltaTime;

            if (_tracker >= 5)
            {
                _pointsToReward -= 100;
                if (_pointsToReward <= 0)
                {
                    Debug.Log("Jerald islesbian");
                    _pointsToReward = 0;
                    return;
                }
                _tracker = 0;
            }
            timerText.text = "Time Being Taken: " + (int)_timer;
        }
    }
    void ResetPointTracker()
    {
        _tracker = 0;
        _timer = 0;
        _pointsToReward = pointsMax;
    }
    public void SetRequestedItem(ItemData newRequestedItem) => requestedItem = newRequestedItem;
}
