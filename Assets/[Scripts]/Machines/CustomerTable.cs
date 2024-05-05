using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static VRGameManager;

public class CustomerTable : MonoBehaviour
{
    public GameMode gameMode;
    //box things
    [SerializeField] RequestBox requestBox;
    [SerializeField] float boxRequestYPosition;
    [SerializeField] float boxSendYPosition;

    //requests
    bool isRequest = true;
    [SerializeField] List<ItemData> posibleRequests;
    [SerializeField] TMP_Text orderText;
    Coroutine moveBoxCoroutineHandler;
    bool gameStart = false;
    float gameTimeLeft = 0;
    //Timer
    float elapsedTimeToNextRequest = 0;
    float timeToNextRequest;

    //Hand text refrences
    [SerializeField] TMP_Text LeftHandTimerTxt;
    [SerializeField] TMP_Text RightHandRequestTxt;
    int totalScore = 0;
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
        if(!gameStart)
        {
            return;
        }
        if(!isRequest)
        {
            float timeLeftForOrder = gameTimeLeft - requestBox.GetTimer();
            //Debug.Log(timeLeftForOrder);
            //game ends if time ran out
            if (gameTimeLeft <= 0)
            {
                EndGame();
            }
            else
            {
                // Calculate minutes and seconds
                int minutes = Mathf.FloorToInt(timeLeftForOrder / 60);
                int seconds = Mathf.FloorToInt(timeLeftForOrder % 60);

                // Format the time as a string
                string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
                LeftHandTimerTxt.text = "Time left:\n" + timeString;
            }
           
        }
        else if(isRequest)
        {
            elapsedTimeToNextRequest += Time.deltaTime;
            //if time exceed return
            if (elapsedTimeToNextRequest >= timeToNextRequest)
            {
                ToggleOrder(false);
                return;
            }
        }

     
    }
    private void Update()
    {
        UpdateTimer();


    }

    public void ToggleOrder(bool toggledByButton)
    {
        if (moveBoxCoroutineHandler != null) return;
        //request
        if(isRequest)
        {
            //start game if first time pressing button
            if(toggledByButton && !gameStart)
            {
                gameStart = true;
            } 
            //dont not get request manually if game already started
            else if(toggledByButton && gameStart)
            {
                return;
            }
            //randomise what to order
            int randomRequest = Random.Range(0, posibleRequests.Count);
            requestBox.SetRequestedItem(posibleRequests[randomRequest]);
            orderText.text = "Product needed: "+posibleRequests[randomRequest].itemName;
            RightHandRequestTxt.text = posibleRequests[randomRequest].itemName;
            gameTimeLeft += posibleRequests[randomRequest].GetTimeGiven();
            LeftHandTimerTxt.text = "Time left:\n" + gameTimeLeft.ToString();
            //animate box upwards
            moveBoxCoroutineHandler = StartCoroutine(MoveBoxCoroutine());
  
         
        }
        //send
        else
        {
            //if no item or wrong item, ignore
            if (requestBox.GetInsertedItem() == null || requestBox.GetRequestedItem() != requestBox.GetInsertedItemData()) return;

            //correct item
            //animate box downwards
            moveBoxCoroutineHandler = StartCoroutine(MoveBoxCoroutine());
            //check item quality (how long it takes to complete)

            //reset all variable
            elapsedTimeToNextRequest = 0;
            orderText.text = $"Time taken: {(int)requestBox.GetTimer()}\n\nScore awarded:  {requestBox.ShowScoreResult()}";
            totalScore += requestBox.ShowScoreResult();
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
        if(!isRequest)
        {
            LeftHandTimerTxt.text = "Awaiting order";
            RightHandRequestTxt.text = "Awaiting order";
            //reset the items
            requestBox.ResetBox();
            requestBox.ResetPointTracker();
        }
        isRequest = !isRequest;
        moveBoxCoroutineHandler = null;
    }

    void ResetBox()
    {
        //reset box back
        Vector3 currentPosition = requestBox.transform.localPosition;
        currentPosition.y = boxSendYPosition;
        requestBox.transform.localPosition = currentPosition;

        if(!gameStart)
        {
            LeftHandTimerTxt.text = "Awaiting game\nto start";
            RightHandRequestTxt.text = "Awaiting game\nto start";
        }
        totalScore = 0;
    }
    void EndGame()
    {
        isRequest = true;
        if (moveBoxCoroutineHandler != null)
        {
            StopCoroutine(moveBoxCoroutineHandler);
            moveBoxCoroutineHandler = null;
        }
        gameStart = false;
        gameTimeLeft = 0;
        elapsedTimeToNextRequest = 0;
        orderText.text = $"Total Score: {totalScore}";
       ResetBox();
    }
}
