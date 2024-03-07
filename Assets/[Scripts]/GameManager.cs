using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] GameFeedback gameFeedback;
    void Start()
    {
        playerMovement.Init();
        gameFeedback.InIt();
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.UpdateTransform();
    }
}
