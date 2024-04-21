using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MachineSmelter : MonoBehaviour
{
    [SerializeField] private float smeltTime = 3f;
    [SerializeField] private List<ItemData> outputItemList;
    [SerializeField] private SmelterInputHitbox smelterInputHitbox;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float timeToBlowUp = 5f;
    [SerializeField] private TMP_Text coalPercentage;
    [SerializeField] int healthPoints;
    [Header("Feedback Events")]
    [SerializeField] private FeedbackEventData e_run;
    [SerializeField] private FeedbackEventData e_done;
    [SerializeField] private FeedbackEventData e_countdown;
    [SerializeField] private FeedbackEventData e_explosion;
    [SerializeField] private FeedbackEventData e_blewUpWarning;
    [SerializeField] private FeedbackEventData e_stopAllSound;


    [Header("Door System")]
    [SerializeField] private XRDoor door;

    private float elapsedTime = 0f;
    private Coroutine smeltingCoroutineHandler;
    private bool scrapConverted = false;
    private float fuelLeft;
    private float maxFuel; // Variable to track the maximum fuel level for current refill
    private float elapsedTimeToBlowUp = 0f;
    private bool blewUp = false;
    bool aboutToBlow = false;
    bool machineActive = false;

    public TMP_Text debugTxt;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private ParticleSystem fireParticle;

    public bool HasBlownUp() => blewUp;
    public int GetHealthPoints() => healthPoints;
    private void Awake()
    {
        RefillFuel();
        timerText.text = "Ready";
    }

    private IEnumerator SmeltCoroutine()
    {


        // Smelting process
        while (elapsedTime <= smeltTime)
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
            e_run?.InvokeEvent(transform.position, Quaternion.identity, transform);
            machineActive = true;
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
        if (fuelLeft == 0 || !machineActive || blewUp) return;
        if (fuelLeft > 0) // Ensure we don't divide by zero
        {
            float percentage = (fuelLeft / maxFuel) * 100f; // Calculate fuel percentage
            coalPercentage.text = $"Coal: {Mathf.Clamp(percentage, 0, 100):0}%"; // Clamp to ensure it's between 0% and 100%
            ReduceFuel();
        }
        else
        {
            fuelLeft = 0;
            coalPercentage.text = "Coal: 0%";
        }
    }
    private void StartSmelting()
    {
        door.SetAbilityToGrab(false);
        smeltingCoroutineHandler = StartCoroutine(SmeltCoroutine());
    }

    private void DeactivateMachine()
    {
        machineActive = false;
        door.SetAbilityToGrab(true);
        scrapConverted = false;
        Debug.Log("Machine Deactivated");
        e_stopAllSound?.InvokeEvent(transform.position, Quaternion.identity, transform);
    }

    private void PauseSmelting()
    {
        e_stopAllSound?.InvokeEvent(transform.position, Quaternion.identity, transform);
        //play out of fuel sound
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
        
        timerText.text = ((int)(smeltTime - elapsedTime) + 1).ToString();
    }
    void ReduceFuel()
    {
        fuelLeft -= Time.deltaTime; // Simulate fuel consumption
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
            GameObject freshRawMaterial = Instantiate(outputMaterial.GetPrefab(), scrap.transform.position, scrap.transform.rotation);
            freshRawMaterial.AddComponent<FreshRawMaterial>();
            Destroy(scrap.gameObject);
        }

        foreach (GameObject wrongItemType in smelterInputHitbox.GetDestroyList())
        {
            Destroy(wrongItemType);
        }
    }

    private void HandleBlowUpCountdown()
    {
        if (!scrapConverted || blewUp || aboutToBlow) return;

        elapsedTimeToBlowUp += Time.deltaTime;
        // Give 5 second leeway so it will never blow up in the first 5 seconds.
        if (elapsedTimeToBlowUp >= timeToBlowUp)
        {
            // Randomize 30% chance to blow up.
            float chance = Random.Range(0f, 1f); // Generates a random number between 0.0 to 1.0.
            if (chance <= 0.3f) // Checks if the random number falls within the first 30% range.
            {
                // 30% chance hit, start countdown to blow up.
                StartCoroutine(BlowUpCountdown());
                aboutToBlow = true;
            }
            else
            {
                //  check if it will blow up next second again
                elapsedTimeToBlowUp = timeToBlowUp -1;
            }
        }
    }

    IEnumerator BlowUpCountdown()
    {
        // Assuming a 5-second countdown before the actual blow-up.
        e_countdown?.InvokeEvent(transform.position, Quaternion.identity, transform);
        yield return new WaitForSeconds(3);
        BlowUp();
    }

    private void BlowUp()
    {
        //run the blow up sound
        //run the blow up effects
        explosionParticle.Play();
        fireParticle.Play();
        e_explosion?.InvokeEvent(transform.position, Quaternion.identity, transform);
        e_blewUpWarning?.InvokeEvent(transform.position, Quaternion.identity, transform);
        timerText.text = "WARNING!!!";
        blewUp = true;
        aboutToBlow = false;
    }

    public void FixBlowUp()
    {
        fireParticle.Stop();
        blewUp = false;
        elapsedTimeToBlowUp = 0;
        DeactivateMachine();
    }
}
