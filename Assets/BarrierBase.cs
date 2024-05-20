using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBase : MonoBehaviour
{
    [SerializeField] private GameObject smelterBarrier;
    [SerializeField] private GameObject shredderBarrier;
    [SerializeField] private GameObject fabricatorBarrier;
    [SerializeField] private GameObject anvilBarrier;

    private void Start()
    {
        //EnableBarrier();
    }
    public void DisableBarrier()
    {
        smelterBarrier.SetActive(false);
        smelterBarrier.GetComponent<Collider>().enabled = false;

        shredderBarrier.SetActive(false);
        shredderBarrier.GetComponent<Collider>().enabled = false;

        fabricatorBarrier.SetActive(false);
        fabricatorBarrier.GetComponent<Collider>().enabled = false;

        anvilBarrier.SetActive(false);
        anvilBarrier.GetComponent<Collider>().enabled = false;
    }

    public void EnableBarrier()
    {
        smelterBarrier.SetActive(true);
        smelterBarrier.GetComponent<Collider>().enabled = true;

        shredderBarrier.SetActive(true);
        shredderBarrier.GetComponent<Collider>().enabled = true;

        fabricatorBarrier.SetActive(true);
        fabricatorBarrier.GetComponent<Collider>().enabled = true;

        anvilBarrier.SetActive(true);
        anvilBarrier.GetComponent<Collider>().enabled = true;
    }
}
