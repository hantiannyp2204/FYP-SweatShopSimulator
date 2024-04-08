using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LookAtMeUI : MonoBehaviour
{
    [SerializeField] private GameObject followPlayer;
    [SerializeField] private Camera toFollow;
    

    // Update is called once per frame
    void Update()
    {
        // transform.LookAt(transform.position + playerCam.transform.rotation * Vector3.forward, playerCam.transform.rotation * Vector3.up);
        followPlayer.transform.LookAt(followPlayer.transform.position + toFollow.transform.rotation * Vector3.forward, toFollow.transform.rotation * Vector3.up);
    }
}
