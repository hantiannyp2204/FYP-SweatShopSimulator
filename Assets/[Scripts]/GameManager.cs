using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    void Start()
    {
        playerMovement.Init();
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.UpdateTransform();
    }
}
