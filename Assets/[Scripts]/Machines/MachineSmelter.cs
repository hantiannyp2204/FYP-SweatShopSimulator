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

    //fuel system
    bool scrapConverted = false;
    bool convertingPaused = false; // This variable is now unnecessary
    float fuelLeft;
    private void Awake()
    {
        RefillFuel();
        timerText.text = "Ready";
    }

    IEnumerator SmeltCoroutine()
    {
        e_run?.InvokeEvent(transform.position, Quaternion.identity, transform);

        while (elapsedTime <= smeltTime && fuelLeft > 0) // Check fuel level
        {
            elapsedTime += Time.deltaTime;
            fuelLeft -= Time.deltaTime; // Simulate fuel consumption
            timerText.text = ((int)(smeltTime - elapsedTime) + 1).ToString();
            yield return null;
        }

        if (fuelLeft <= 0)
        {
            // Pause execution here by returning and not proceeding to the rest of the method.
            // The coroutine is technically still running but doing nothing.
            convertingPaused = true; // This marks that the process is paused.
            yield break; // Exit the coroutine
        }

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
        smelterInputHitbox.ClearList();
        scrapConverted = true;
        yield return null;
    }

    public void RunMachine()
    {
        if (smeltingCoroutineHandler == null) // Start only if not already running
        {
            elapsedTime = 0; // Reset elapsed time
            smeltingCoroutineHandler = StartCoroutine(SmeltCoroutine());
        }
    }
    public void RunDective()
    {
        scrapConverted = false;
        Debug.Log("Machine Deactive");
    }
    public void RefillFuel()
    {
        fuelLeft = Random.Range(100, 300); // Refill the fuel
        if (convertingPaused) // Check if it was paused due to fuel running out
        {
            convertingPaused = false;
            RunMachine(); // Resume smelting by starting the coroutine again
        }
    }
    private void Update()
    {
        //if ran out of fuel, pause coroutine
        if (fuelLeft <= 0 && smeltingCoroutineHandler != null)
        {
            fuelLeft = 0;
            timerText.text = "Out of fuel";
            StopCoroutine(smeltingCoroutineHandler); // Stop the coroutine if it's running
            smeltingCoroutineHandler = null; // Clear the handler to allow restarting
        }
        //do countdown to burning
        if (scrapConverted)
        {

        }
    }
}
