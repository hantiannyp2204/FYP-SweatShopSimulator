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
        smelterTutorialBarrier.SetActive(true);
    }
    public void EnableAnvil()
    {
        anvilTutorialBarrier.SetActive(true);
    }
    public void EnableShredder()
    {
        shredderTutorialBarrier.SetActive(true);
    }
    public void EnableFabricator()
    {
        fabricatorTutorialBarrier.SetActive(true);
    }
    public void DisableAll()
    {
        smelterTutorialBarrier.SetActive(false);
        anvilTutorialBarrier.SetActive(false);
        shredderTutorialBarrier.SetActive(false);
        fabricatorTutorialBarrier.SetActive(false);
    }
}
