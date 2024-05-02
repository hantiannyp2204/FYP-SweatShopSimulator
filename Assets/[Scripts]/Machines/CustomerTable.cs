using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomerTable : MonoBehaviour
{
    //box things
    [SerializeField] RequestBox requestBox;
    [SerializeField] float boxRequestYPosition;
    [SerializeField] float boxSendYPosition;

    //requests
    bool isRequest = true;
    [SerializeField] List<ItemData> posibleRequests;
    [SerializeField] TMP_Text orderText;
    Coroutine moveBoxCoroutineHandler;
    //Timer
    float elapsedTime = 0;
    float timeToNextRequest;

    void RandomiseNextRequestTimer()
    {
        timeToNextRequest = Random.Range(2, 5);
    }
    void Start()
    {
        ResetBox();
        RandomiseNextRequestTimer();
        requestBox.ResetPointTracker();
    }

    public void UpdateTimer()
    {
        //if time exceed return
        if(elapsedTime >= timeToNextRequest)
        {
            ToggleOrder();
            return;
        }
        elapsedTime += Time.deltaTime;
    }

    public void ToggleOrder()
    {
        if (moveBoxCoroutineHandler != null) return;
        //request
        if(isRequest)
        {
            //randomise what to order
            int randomRequest = Random.Range(0, posibleRequests.Count);
            requestBox.SetRequestedItem(posibleRequests[randomRequest]);
            orderText.text = "Product needed: "+posibleRequests[randomRequest].itemName;
            //animate box upwards
            moveBoxCoroutineHandler = StartCoroutine(MoveBoxCoroutine());
            isRequest = false;
        }
        //send
        else
        {
            //if wrong item, ignore
            if (requestBox.GetRequestedItem() != requestBox.GetInsertedItem()) return;

            //correct item
            //animate box downwards
            moveBoxCoroutineHandler = StartCoroutine(MoveBoxCoroutine());
            //check item quality (how long it takes to complete)

            //reset all variable
            isRequest = true;
            elapsedTime = 0;
            orderText.text = $"Time taken: {requestBox.ShowTimerResult()}\nScore awarded: {requestBox.ShowScoreResult()}";
            RandomiseNextRequestTimer();
        }
        
    }
    public IEnumerator MoveBoxCoroutine()
    {
        float elapsedTime = 0f;
        float duration = 0.3f;
        Vector3 startPosition;
        Vector3 targetPosition;
        if (isRequest)
        {
            startPosition = new Vector3(requestBox.transform.localPosition.x, boxSendYPosition, requestBox.transform.localPosition.z);
            targetPosition = new Vector3(requestBox.transform.localPosition.x, boxRequestYPosition, requestBox.transform.localPosition.z);
        }
        else
        {

            startPosition = new Vector3(requestBox.transform.localPosition.x, boxRequestYPosition, requestBox.transform.localPosition.z);
            targetPosition = new Vector3(requestBox.transform.localPosition.x, boxSendYPosition, requestBox.transform.localPosition.z);
        }

        while (elapsedTime < duration)
        {
            float fraction = elapsedTime / duration;

            requestBox.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, fraction);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the RequestBox is exactly at the target position after the loop completes
        requestBox.transform.localPosition = targetPosition;
        //reset the items
        requestBox.ResetBox();
        requestBox.ResetPointTracker();
        moveBoxCoroutineHandler = null;
    }

    void ResetBox()
    {
        //reset box back
        Vector3 currentPosition = requestBox.transform.localPosition;
        currentPosition.y = boxSendYPosition;
        requestBox.transform.localPosition = currentPosition;
    }
    public void SubcribeEvents() => RequestBox.OnOrderProcessed += ToggleOrder;
    public void UnsubcribeEvents() => RequestBox.OnOrderProcessed -= ToggleOrder;
}
