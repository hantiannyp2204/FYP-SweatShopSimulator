using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MachineSmelter : MonoBehaviour
{
    [SerializeField] private float smeltTime = 3f;
    [SerializeField] private List<ItemData> outputItemList;
    [SerializeField] private SmelterInputHitbox smelterInputHitbox;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float timeToBlowUp = 5f;
    [SerializeField] private TMP_Text coalPercentage;

    [Header("Feedback Events")]
    [SerializeField] private FeedbackEventData e_run;
    [SerializeField] private FeedbackEventData e_done;

    [Header("Door System")]
    [SerializeField] private XRDoor door;

    private float elapsedTime = 0f;
    private Coroutine smeltingCoroutineHandler;
    private bool scrapConverted = false;
    private float fuelLeft;
    private float maxFuel; // Variable to track the maximum fuel level for current refill
    private float elapsedTimeToBlowUp = 0f;
    private bool blewUp = false;

    public TMP_Text debugTxt;

    private void Awake()
    {
        RefillFuel();
        timerText.text = "Ready";
    }

    private IEnumerator SmeltCoroutine()
    {
        e_run?.InvokeEvent(transform.position, Quaternion.identity, transform);

        // Smelting process
        while (elapsedTime <= smeltTime && fuelLeft > 0)
        {
            UpdateTimer();
            yield return null;
        }

        // Check if smelting was paused due to fuel running out
        if (fuelLeft <= 0)
        {
            PauseSmelting();
            yield break;
        }

        // Process and replace all scrap materials
        ProcessScrapMaterials();
        e_done?.InvokeEvent(transform.position, Quaternion.identity, transform);

        ResetSmeltingVariables();
    }

    public void ToggleMachine()
    {
        if (!door.IsDoorLocked() || blewUp) return;

        if (smeltingCoroutineHandler == null && !scrapConverted)
        {
            StartSmelting();
        }
        else if (scrapConverted)
        {
            DeactivateMachine();
        }
    }

    public void RefillFuel()
    {
        maxFuel = Random.Range(20, 30); // Determine max fuel on refill
        fuelLeft = maxFuel; // Reset fuel to this maximum

        coalPercentage.text = "Coal: 100%"; // Initially, coal is at 100%

        // Check if the machine was paused and resume operation if necessary
        if (smeltingCoroutineHandler != null)
        {
            ToggleMachine(); // Resume smelting
        }
    }

    private void Update()
    {
        HandleFuelDepletion();
        HandleBlowUpCountdown();
        UpdateCoalPercentage();
    }
    private void UpdateCoalPercentage()
    {
        if (maxFuel > 0) // Ensure we don't divide by zero
        {
            float percentage = (fuelLeft / maxFuel) * 100f; // Calculate fuel percentage
            coalPercentage.text = $"Coal: {Mathf.Clamp(percentage, 0, 100):0}%"; // Clamp to ensure it's between 0% and 100%
        }
    }
    private void StartSmelting()
    {
        door.SetAbilityToGrab(false);
        smeltingCoroutineHandler = StartCoroutine(SmeltCoroutine());
    }

    private void DeactivateMachine()
    {
        door.SetAbilityToGrab(true);
        scrapConverted = false;
        Debug.Log("Machine Deactivated");
    }

    private void PauseSmelting()
    {
        timerText.text = "Out of fuel";
        StopCoroutine(smeltingCoroutineHandler);
        smeltingCoroutineHandler = null;
    }

    private void ResetSmeltingVariables()
    {
        timerText.text = "Done";
        elapsedTime = 0;
        smeltingCoroutineHandler = null;
        smelterInputHitbox.ClearList();
        scrapConverted = true;

    }

    private void UpdateTimer()
    {
        elapsedTime += Time.deltaTime;
        fuelLeft -= Time.deltaTime; // Simulate fuel consumption
        timerText.text = ((int)(smeltTime - elapsedTime) + 1).ToString();
    }

    private void HandleFuelDepletion()
    {
        if (fuelLeft <= 0 && smeltingCoroutineHandler != null)
        {
            PauseSmelting();
        }
    }

    private void ProcessScrapMaterials()
    {
        foreach (Scrap scrap in smelterInputHitbox.GetScrapList())
        {
            var outputMaterial = outputItemList[(int)scrap.GetScrapType()];
            Instantiate(outputMaterial.GetPrefab(), scrap.transform.position, scrap.transform.rotation);
            Destroy(scrap.gameObject);
        }

        foreach (GameObject wrongItemType in smelterInputHitbox.GetDestroyList())
        {
            Destroy(wrongItemType);
        }
    }

    private void HandleBlowUpCountdown()
    {
        if (!scrapConverted || blewUp) return;

        elapsedTimeToBlowUp += Time.deltaTime;
        if (elapsedTimeToBlowUp >= timeToBlowUp)
        {
            BlowUp();
        }
    }

    private void BlowUp()
    {
        //run the blow up sound
        //run the blow up effects
        debugTxt.text = "ALLAHUAKBARR";
        timerText.text = "WARNING!!!";
        blewUp = true;
    }

    public void FixBlowUp()
    {
        blewUp = false;
        elapsedTimeToBlowUp = 0;
    }
}
