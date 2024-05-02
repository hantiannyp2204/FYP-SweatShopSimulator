using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class MachineSmelter : MonoBehaviour
{
    public TMP_Text debugTxt;

    [Header("Machine Gameplay settings")]
    [SerializeField] private float smeltTime = 3f;
    [SerializeField] private float maxPercentageFuel;
    [SerializeField] private int fuelMaxCapacityWarningCount;
    [SerializeField] private float timeToBlowUp = 5f;
    [SerializeField] private int healthPoints;

    [Header("Machine Refrences")]
    [SerializeField] private SmelterInputHitbox smelterInputHitbox;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text coalPercentage;
    [SerializeField] private Collider outputCollider;
    [SerializeField] private SmelterFuelPointer smelterFuelPointer;
    [SerializeField] Renderer smelterRenderer;

    [Header("Feedback Events")]
    [SerializeField] private Transform smelterSoundLocation;
    [SerializeField] private Transform smelterDoneSoundLocation;
    [SerializeField] private FeedbackEventData e_run;
    [SerializeField] private FeedbackEventData e_done;
    [SerializeField] private FeedbackEventData e_countdown;
    [SerializeField] private FeedbackEventData e_explosion;
    [SerializeField] private FeedbackEventData e_blewUpWarning;
    [SerializeField] private FeedbackEventData e_stopAllSound;


    [Header("Door System")]
    [SerializeField] private SmelterWheel smelterWheel;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private List<ParticleSystem> fireParticleList;
    [SerializeField] private ParticleSystem fuelFire;

    [Header("Coal System")]
    [SerializeField] GameObject coalRender;

    [Header("Fresh Material System")]
    [SerializeField] Material hotMaterial;

    private float elapsedTime = 0f;
    private Coroutine smeltingCoroutineHandler;
    private Coroutine blowUpCountdownCoroutineHandler;
    private bool scrapConverted = false;
    private float fuelLeft;
    private bool outOfFuel = false;
    private bool blewUp = false;
    private bool aboutToBlow = false;
    private bool machineActive = false;
    private bool ableToStart = false;
    private Bounds outputSpawnBounds;
    private float defaultMaxFuel = 100;
    private int currentFuelMaxWarningCount;
    private float smeltSpeed = 1;
    public bool AbilityToStart
    {
        get => ableToStart;
        set => ableToStart = value; 
    }
    public bool HasBlownUp() => blewUp;
    public int GetHealthPoints() => healthPoints;
    private void Awake()
    {
        AddFuel(100);
        currentFuelMaxWarningCount = fuelMaxCapacityWarningCount;
        timerText.text = "Ready";
        if(outputCollider != null)
        {
            outputSpawnBounds = outputCollider.bounds;
        }
        UpdateCoalPercentage();
    }
    private void ToggleMachineEmmision()
    {
        Material material;
        material = smelterRenderer.material;
        if (machineActive)
        {
            material.EnableKeyword("_EMISSION");
        }
        else
        {
            material.DisableKeyword("_EMISSION");
        }

    }
    private void HandleSmeltingSpeed()
    {
        if (fuelLeft > 100)
        {
            smeltSpeed = 2;
        }
        else if(fuelLeft < 40)
        {
            smeltSpeed = .5f;
        }
        else
        {
            smeltSpeed = 1;
        }
    }
  
    private IEnumerator SmeltCoroutine()
    {
        // Smelting process
        while (elapsedTime <= smeltTime)
        {
            HandleSmeltingSpeed();
            UpdateTimer();
            yield return null;
        }

        // Process and replace all scrap materials
        ProcessScrapMaterials();
        e_done?.InvokeEvent(transform.position, Quaternion.identity, smelterDoneSoundLocation);

        ResetSmeltingVariables();
    }
    private List<XRBaseInteractable> GetInteractablesInBounds(Bounds bounds)
    {
        List<XRBaseInteractable> interactableList = new();

        // Center and size of the bounds are used for the OverlapBox
        Vector3 center = bounds.center;
        Vector3 halfExtents = bounds.extents;

        // Optionally, provide the rotation of the collider
        Quaternion rotation = outputCollider.transform.rotation;

        // Get all colliders overlapping the box
        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, rotation);
        foreach (Collider hitCollider in hitColliders)
        {
            if (smelterInputHitbox.GetIgnoreLayer() == (smelterInputHitbox.GetIgnoreLayer() | (1 << hitCollider.gameObject.layer)))
            {
                continue;
            }
            XRBaseInteractable interactable = hitCollider.GetComponentInChildren<XRBaseInteractable>();
            if(interactable !=null)
            {
                interactableList.Add(interactable);
            }
        }
        return interactableList;
    }
    public void DisableItemGrab()
    {
        List<XRBaseInteractable> interactableList = GetInteractablesInBounds(outputSpawnBounds);
        foreach(XRBaseInteractable baseInteractableScript in interactableList)
        {
            baseInteractableScript.enabled= false;
        }
    }
    public void EnableItemGrab()
    {
        List<XRBaseInteractable> interactableList = GetInteractablesInBounds(outputSpawnBounds);
        foreach (XRBaseInteractable baseInteractableScript in interactableList)
        {
            baseInteractableScript.enabled = true;
        }
    }
    public void ToggleMachine()
    {
        if (blewUp || !AbilityToStart)
        {
            return;
        }
     

        if (smeltingCoroutineHandler == null && !scrapConverted)
        {
            ToggleMachineEmmision();
            smelterWheel.enabled = true;
            fuelFire.Play();
            e_run?.InvokeEvent(transform.position, Quaternion.identity, smelterSoundLocation);
            machineActive = true;
            StartSmelting();
        }
        else if (scrapConverted)
        {
            DeactivateMachine();
        }
    }

    public void AddFuel(float addedFuelCount)
    {
        if (fuelLeft == 0)
        {
            outOfFuel = false;
            //enable coal render
            coalRender.SetActive(true);
        }
        fuelLeft += addedFuelCount; // Reset fuel to this maximum

     
        //if exceed max percentage
        if (fuelLeft > maxPercentageFuel)
        {
            fuelLeft = maxPercentageFuel;
            currentFuelMaxWarningCount--;
            if(currentFuelMaxWarningCount <= 0)
            {
                BlowUp();
                return;
            }
            else
            {
                //play warning ping
            }
        }
        if (debugTxt != null)
        {
            debugTxt.text = fuelLeft.ToString();
        }
        UpdateCoalPercentage();

        smelterFuelPointer.UpdatePosition(fuelLeft);
        // Check if the machine was paused and resume operation if necessary
        if (smeltingCoroutineHandler != null)
        {
            ToggleMachine(); // Resume smelting
        }
    }

    private void Update()
    {
        if(outOfFuel || !machineActive || blewUp)
        {
            if (fuelFire.isPlaying)
            {
                fuelFire.Stop();
            }
            return;
        }
        //if too much heat and scraps are already converted
        if(fuelLeft > 100 && !aboutToBlow && scrapConverted)
        {
            blowUpCountdownCoroutineHandler = StartCoroutine(BlowUpCountdown());
            aboutToBlow = true;
        }
        //runs as long as machine is active and usable
        HandleFuelDepletion();

        //Updating UI text
        UpdateCoalPercentage();
    }
    private void UpdateCoalPercentage()
    {
        int percentage = (int)((fuelLeft / defaultMaxFuel) * 100); // Calculate fuel percentage

        //reset the warning count when it reaches back stable 100
        if (percentage <= 100 && currentFuelMaxWarningCount != fuelMaxCapacityWarningCount)
        {
            currentFuelMaxWarningCount = fuelMaxCapacityWarningCount;
        }
      


        // Ensure we don't divide by zero
        if (fuelLeft > 0)
        {
            //setting colors
            if(fuelLeft > 100)
            {
                coalPercentage.color = Color.red;
            }
            else if (fuelLeft < 40)
            {
                coalPercentage.color = Color.yellow;
            }
            else
            {
                coalPercentage.color = Color.green;
            }
            coalPercentage.text = $"Coal: {Mathf.Clamp(percentage, 0, maxPercentageFuel):0}%"; // Clamp to ensure it's between 0% and 100%

        }
        else
        {
            coalPercentage.text = "Coal: 0%";
        }
    }
    private void HandleFuelDepletion()
    {

        if (fuelLeft > 0)
        {

            ReduceFuel();
            //update the pointer's position
            smelterFuelPointer.UpdatePosition(fuelLeft);
        }
        else
        {
            fuelLeft = 0;
            //enable coal render
            coalRender.SetActive(false);
            PauseSmelting();

            outOfFuel = true;
        }
    }
    private void StartSmelting()
    {
        smeltingCoroutineHandler = StartCoroutine(SmeltCoroutine());
    }

    private void DeactivateMachine()
    {
        ToggleMachineEmmision();
        smelterWheel.enabled= true;
        machineActive = false;
        scrapConverted = false;
        aboutToBlow = false;
        elapsedTime= 0;
        if(blowUpCountdownCoroutineHandler != null)
        {
            StopCoroutine(blowUpCountdownCoroutineHandler);
            blowUpCountdownCoroutineHandler = null;
        }
        Debug.Log("Machine Deactivated");
        e_stopAllSound?.InvokeEvent(transform.position, Quaternion.identity, smelterSoundLocation);
    }

    private void PauseSmelting()
    {
        e_stopAllSound?.InvokeEvent(transform.position, Quaternion.identity, smelterSoundLocation);
        //play out of fuel sound
        timerText.text = "Out of fuel";
        if(smeltingCoroutineHandler != null)
        {
            StopCoroutine(smeltingCoroutineHandler);

            smeltingCoroutineHandler = null;
        }

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
        elapsedTime += Time.deltaTime * smeltSpeed;
        
        timerText.text = ((int)(smeltTime - elapsedTime) + 1).ToString();
    }
    void ReduceFuel()
    {
        fuelLeft -= Time.deltaTime; // Simulate fuel consumption
    }    


    private void ProcessScrapMaterials()
    {
        //spawn the raw materials
        foreach (Scrap scrap in smelterInputHitbox.GetScrapList())
        {
            if (scrap.GetMaterial() != null)
            {
                float x = Random.Range(-outputSpawnBounds.extents.x, outputSpawnBounds.extents.x);
                float z = Random.Range(-outputSpawnBounds.extents.z, outputSpawnBounds.extents.z);
                GameObject freshRawMaterial = Instantiate(scrap.GetMaterial(), outputSpawnBounds.center + new Vector3(x, 0f, z), Quaternion.identity);
                freshRawMaterial.AddComponent<FreshRawMaterial>().ApplyHotTexture(hotMaterial);
            }

            Destroy(scrap.gameObject);
        }

        foreach (GameObject wrongItemType in smelterInputHitbox.GetDestroyList())
        {
            Destroy(wrongItemType);
        }
    }

    IEnumerator BlowUpCountdown()
    {
        e_countdown?.InvokeEvent(transform.position, Quaternion.identity, smelterSoundLocation);
        yield return new WaitForSeconds(3);
        BlowUp();
    }

    private void BlowUp()
    {
        //run the blow up sound
        //run the blow up effects
        blowUpCountdownCoroutineHandler = null;
        explosionParticle.Play();
        foreach(ParticleSystem particle in fireParticleList)
        {
            particle.Play();
        }
       
        e_explosion?.InvokeEvent(transform.position, Quaternion.identity, smelterSoundLocation);
        e_blewUpWarning?.InvokeEvent(transform.position, Quaternion.identity, smelterSoundLocation);
        timerText.text = "WARNING!!!";
        blewUp = true;
        aboutToBlow = false;
    }

    public void FixBlowUp()
    {
        foreach (ParticleSystem particle in fireParticleList)
        {
            particle.Stop();
        }

        blewUp = false;
        currentFuelMaxWarningCount = fuelMaxCapacityWarningCount;
        DeactivateMachine();
    }
}
