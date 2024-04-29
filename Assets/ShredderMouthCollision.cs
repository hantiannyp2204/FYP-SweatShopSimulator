using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderMouthCollision : MonoBehaviour
{
    public GameObject mouth1;
    public GameObject mouth2;

    private void Start()
    {
        if (mouth1 == null || mouth2 == null) return;
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
