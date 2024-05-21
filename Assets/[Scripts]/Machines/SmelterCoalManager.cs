using Oculus.Interaction;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SmelterCoalManager : MonoBehaviour
{
    public MachineSmelter smelter;
    [SerializeField] float maxPurityValue;
    [SerializeField] float minPurityValue;
    [SerializeField] FeedbackEventData fuelAddedSound;

    private bool _isAddedCoalQuestCheck = false;
    private int _counter = 0;
    public bool GetAddedCoalCheck()
    {
        return _isAddedCoalQuestCheck;
    }

    public int GetCoalInputCounter()
    {
        return _counter;
    }
    private void OnTriggerEnter(Collider other)
    {
        //only accept if its coal and is not being grabbed
        GeneralItem item = other.transform.GetComponent<GeneralItem>();
        if (item == null)
        {
            return;
        }
        ItemData itemData = item.Data;
        if (itemData != null && itemData.itemName == "Coal fuel" && !other.GetComponent<XRBaseInteractable>().isSelected && other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            _counter += 1;
            _isAddedCoalQuestCheck = true;
            //play fuel enter sound
            fuelAddedSound?.InvokeEvent(transform.position, Quaternion.identity, transform);
            Debug.Log("Entered");
            // Here you can also call a method on the smelter to refill fuel
            // Assuming such a method exists
            smelter.AddFuel(Random.Range(minPurityValue, maxPurityValue));
            Destroy(other.gameObject);
        }
    }
}