using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderMouthCollision : MonoBehaviour
{
    [SerializeField] private GameObject mouth1;
    [SerializeField] private GameObject mouth2;

    private bool _hasEntered;
  
    private void OnTriggerEnter(Collider other)
    {
        _hasEntered = true;
        mouth1.GetComponent<Animator>().SetBool("isActivate", false);
        //mouth2.GetComponent<Animator>().SetBool("isOpen", true);
    }

    private void OnTriggerExit(Collider other)
    {
        _hasEntered = false;

        mouth1.GetComponent<Animator>().SetBool("isActivate", true);
        //mouth2.GetComponent<Animator>().SetBool("isOpen", false);
    }

    public bool GetHasEntered()
    {
        return _hasEntered;
    }
}
