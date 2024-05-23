using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static VRGameManager;

public class CustomerTable : MonoBehaviour
{
    public RobotDisplayOrder robotDisplayOrder;

    private GameMode gameMode;

    [Header("Win and Lose")]
    public GameObject _Win;
    public GameObject _EndGameUI;
    public TMP_Text _PointsText;

    [Header("Feedback")]
    [SerializeField] FeedbackEventData e_requestRecieve;
    [SerializeField] FeedbackEventData e_requestSend;

    //box settings
    [SerializeField] RequestBox requestBox;
    [SerializeField] float boxRequestYPosition;
    [SerializeField] float boxSendYPosition;
    [SerializeField] Animator boxAnimator;
    //requests
    public bool isRequest = true;
    [SerializeField] List<ItemData> posibleRequests;
    [SerializeField] TMP_Text orderText;
    Coroutine moveBoxCoroutineHandler;
    public bool gameStart = false;
    float gameTimeLeft = 0;
    //Timer
    float elapsedTimeToNextRequest = 0;
    float timeToNextRequest;
    float timeTaken = 0;
    float timeNeededToWin;

    int totalScore = 0;
    private bool _isEnteredSuccess = false;
    public bool GetEnteredProductVerdict()
    {
        return _isEnteredSuccess;
    }

    public void SetProductVerdict(bool status)
    {
        _isEnteredSuccess = status;
    }

    void RandomiseNextRequestTimer()
    {
        timeToNextRequest = Random.Range(2, 5);
    }
    
    void Start()
    {
        Time.timeScale = 1.0f;
        if (_Win!=null)
        {
            _Win?.SetActive(false);
        }
        
        _EndGameUI?.SetActive(false);
        ResetBoxPosition();
        RandomiseNextRequestTimer();
        requestBox.Init();

        if (robotDisplayOrder == null) return;
    }
    public void SetTimeNeededToWin(float timeNeeded)
    {
        timeNeededToWin = timeNeeded;
    }

    public void Init(TMP_Text leftHandTimerText, GameMode setGameMode)
    {
        leftHandTimerText.text = "Go start your shift";
        gameMode = setGameMode;
        if (gameMode == GameMode.Test_Chamber)
        {
            timeTaken = 0;
        }
    }
    public void UpdateTimer(TMP_Text lefthandTimerText)
    {
        if (!gameStart)
        {
            return;
        }

        //updates time left before "Ran out of time" end
        gameTimeLeft -= Time.deltaTime;
        //Debug.Log(timeLeftForOrder);
        //if time needed to win is 0
        //if (timeNeededToWin <= 0 && gameMode == GameMode.Levels)
        //{
        //    EndLevel(true);
        //}
        //game ends if time ran out
        /*else */if (gameTimeLeft <= 0)
        {
            EndGame();
            EndLevel();
            //if (gameMode == GameMode.Levels)
            //{
            //    EndLevel(false);
            //}
        }
        else
        {
            // Calculate minutes and seconds
            int minutes = Mathf.FloorToInt(gameTimeLeft / 60);
            int seconds = Mathf.FloorToInt(gameTimeLeft % 60);

            // Format the time as a string
            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
            lefthandTimerText.text = "Time left:\n" + timeString;

            //add time elapsed if it is test chamber
            if (gameMode == GameMode.Test_Chamber)
            {
                timeTaken += Time.deltaTime;
            }
            //remove time needed to win if levels mode
            else
            {
                timeNeededToWin -= Time.deltaTime;
            }
        }
        if (isRequest)
        {
            elapsedTimeToNextRequest += Time.deltaTime;
            //if time exceed return
            if (elapsedTimeToNextRequest >= timeToNextRequest)
            {
                ToggleOrder();
                return;
            }
        }


    }

    public void ToggleOrder(bool toggledByButton = false)
    {
        if (moveBoxCoroutineHandler != null) return;
        //request
        if (isRequest)
        {
            //dont not get request manually if game already started
            if (toggledByButton && gameStart)
            {
                return;
            }
            //randomise what to order
            int randomRequest = Random.Range(0, posibleRequests.Count);
            //play order came sound
            requestBox.SetRequestedItem(posibleRequests[randomRequest]);
            orderText.text = "Product needed: " + posibleRequests[randomRequest].itemName;

            //start game if first time pressing button
            if (toggledByButton && !gameStart)
            {
                robotDisplayOrder.EnableDisplay.Invoke();
                gameTimeLeft = posibleRequests[randomRequest].GetTimeGiven();
                //game start sound
                gameStart = true;
                requestBox.StartGame();
            }
            else
            {
                //hard lock time given to 20 sec if alr exceed 5 mins
                if (gameTimeLeft > (60 * 5))
                {
                    gameTimeLeft += 20;
                }
                else
                {
                    gameTimeLeft += posibleRequests[randomRequest].GetTimeGiven();
                }
              
            }
            //animate box upwards
            moveBoxCoroutineHandler = StartCoroutine(MoveBoxCoroutine());


        }
        //send
        else
        {
            //animate box downwards
            moveBoxCoroutineHandler = StartCoroutine(MoveBoxCoroutine()); 
            //check item quality (how long it takes to complete)

            //reset all variable
            elapsedTimeToNextRequest = 0;
            orderText.text = $"Time taken: {(int)timeTaken}\n\nScore awarded:  {requestBox.ShowScoreResult()}";
            totalScore += requestBox.ShowScoreResult();
            RandomiseNextRequestTimer();


            _isEnteredSuccess = true;
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
            e_requestRecieve?.InvokeEvent(transform.position, Quaternion.identity, transform);
            boxAnimator.SetTrigger("Recieve");
        }
        else
        {       

            startPosition = new Vector3(requestBox.transform.localPosition.x, boxRequestYPosition, requestBox.transform.localPosition.z);
            targetPosition = new Vector3(requestBox.transform.localPosition.x, boxSendYPosition, requestBox.transform.localPosition.z);
            e_requestSend?.InvokeEvent(transform.position, Quaternion.identity, transform);
            boxAnimator.SetTrigger("Send");
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
        if (!isRequest)
        {
            //reset the items
            requestBox.SendRequestOver();
            requestBox.ResetPointTracker();
        }
        isRequest = !isRequest;
        moveBoxCoroutineHandler = null;
    }

    void ResetBoxPosition()
    {
        //reset box back
        Vector3 currentPosition = requestBox.transform.localPosition;
        currentPosition.y = boxSendYPosition;
        requestBox.transform.localPosition = currentPosition;
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
        if (gameMode == GameMode.Test_Chamber)
        {
            orderText.text = $"Total Score: {totalScore} \n\nTime survived: {(int)timeTaken} seconds";
        }
        else
        {
            orderText.text = "Shift ended";
        }

        ResetBoxPosition();
    }
    private void EndLevel()
    {
        //Jerald edit here

        //show the end level UI
        _EndGameUI?.SetActive(true);
        Time.timeScale = 0f;
        //show score
    }
    public bool isEndGame() => !gameStart;

  
}
