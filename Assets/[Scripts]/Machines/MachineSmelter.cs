using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Scrap;

public class MachineSmelter : MonoBehaviour
{
    [SerializeField] float smeltTime = 3;
    float elapsedTime;
    [SerializeField] TMP_Text timerText;
    Coroutine smeltingCoroutineHandler;

    [SerializeField] List<ItemData> OutputItemList = new();
    [SerializeField] SmelterInputHitbox smelterInputHitbox;
    // Feedback
    [Header("FEEDBACK")]
    [SerializeField] private FeedbackEventData e_run;
    [SerializeField] private FeedbackEventData e_done;
   
    private void Awake()
    {
        timerText.text = "Ready";
    }

    IEnumerator SmeltCoroutine()
    {  
        //play the start machine sound
        e_run?.InvokeEvent(transform.position, Quaternion.identity, transform);
       
        //timer
        while (elapsedTime <= smeltTime)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = ((int)(smeltTime - elapsedTime)+1).ToString();
            yield return null;
        }
        //play the stop machine sound
        e_done?.InvokeEvent(transform.position, Quaternion.identity, transform);
        timerText.text = "Done";
        elapsedTime = 0;

        //replace all scrap with their respective material
        foreach(Scrap scrap in smelterInputHitbox.GetScrapList())
        {
            ItemData outputMaterial;
            //convert scrap to its specific raw material
            //0 is plastic, 1 is wood, 2 is metal
            switch (scrap.GetScrapType())
            {
                case ScrapType.Plastic:
                    outputMaterial= OutputItemList[0];
                    break;
                case ScrapType.Wood:
                    outputMaterial = OutputItemList[1];
                    break;
                case ScrapType.Metal:
                    outputMaterial = OutputItemList[2];
                    break;
                default:
                    outputMaterial = OutputItemList[0];
                    break;
            }
            //spwan the materail at where the scrap was at
            Instantiate(outputMaterial.GetPrefab(), scrap.transform.position, scrap.transform.rotation);
            //delete the scrap
            Destroy(scrap.gameObject);

        }
        foreach(GameObject wrongItemType in smelterInputHitbox.GetDestroyList())
        {
            Destroy(wrongItemType);
        }
        smeltingCoroutineHandler = null;

        yield return null;
    }
    
    public void RunMachine()
    {
        if (smeltingCoroutineHandler != null) return;
        smeltingCoroutineHandler = StartCoroutine(SmeltCoroutine());
    }
    public void RunDective()
    {
        Debug.Log("Machine Deactive");
    }
}
