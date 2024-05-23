using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBarrier : MonoBehaviour
{
    [SerializeField] private GameObject smelterTutorialBarrier;
    [SerializeField] private GameObject anvilTutorialBarrier;
    [SerializeField] private GameObject shredderTutorialBarrier;
    [SerializeField] private GameObject fabricatorTutorialBarrier;

    private void Start()
    {
        DisableAll();
    }
    public void EnableSmelter()
    {
        smelterTutorialBarrier.SetActive(false);
    }
    public void EnableAnvil()
    {
        anvilTutorialBarrier.SetActive(false);
    }
    public void EnableShredder()
    {
        shredderTutorialBarrier.SetActive(false);
    }
    public void EnableFabricator()
    {
        fabricatorTutorialBarrier.SetActive(false);
    }
    public void DisableAll()
    {
        smelterTutorialBarrier.SetActive(true);
        anvilTutorialBarrier.SetActive(true);
        shredderTutorialBarrier.SetActive(true);
        fabricatorTutorialBarrier.SetActive(true);
    }
}
