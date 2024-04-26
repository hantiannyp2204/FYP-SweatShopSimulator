using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShredderMouthCollision : MonoBehaviour
{
    [SerializeField] private GameObject mouth1;
    [SerializeField] private GameObject mouth2;


    private void Start()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        mouth1.GetComponent<Animator>().SetBool("isActivate", false);
        mouth2.GetComponent<Animator>().SetBool("isActivate", false);
    }

    private void OnTriggerExit(Collider other)
    {
        mouth1.GetComponent<Animator>().SetBool("isActivate", true);
        mouth2.GetComponent<Animator>().SetBool("isActivate", true);
    }
}
