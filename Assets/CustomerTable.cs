using System.Collections;
using System.Collections.Generic;
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
    }

    public void UpdateTimer()
    {
        //if time exceed return
        if(elapsedTime >= timeToNextRequest)
        {
            RequestOrder();
            return;
        }
        elapsedTime += Time.deltaTime;
    }

    public void RequestOrder()
    {
        if(!isRequest)
        {
            return;
        }
        //randomise what to order
        int randomRequest = Random.Range(0, posibleRequests.Count);
        requestBox.SetRequestedItem(posibleRequests[randomRequest]);
        //animate box upwards
        StartCoroutine(MoveBoxCoroutine());
        isRequest = false;
    }
    public void SendOrder()
    {
        //animate box downwards
        StartCoroutine(MoveBoxCoroutine());
        //check item quality (how long it takes to complete)

        //reset all variable
        isRequest= true;
        elapsedTime = 0;
        RandomiseNextRequestTimer();
    }
    IEnumerator MoveBoxCoroutine()
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
    }

    void ResetBox()
    {
        //reset box back
        Vector3 currentPosition = requestBox.transform.localPosition;
        currentPosition.y = boxSendYPosition;
        requestBox.transform.localPosition = currentPosition;
    }
    public void SubcribeEvents() => RequestBox.OnOrderProcessed += SendOrder;
    public void UnsubcribeEvents() => RequestBox.OnOrderProcessed -= SendOrder;
}
