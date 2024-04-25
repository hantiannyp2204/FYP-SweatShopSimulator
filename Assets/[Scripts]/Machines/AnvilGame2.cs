using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnvilGame2 : MonoBehaviour
{

    [SerializeField] private Hammer hammer;
    [SerializeField] private MachineAnvil anvil;
    [SerializeField] private AnvilHitbox hitbox;

    public Slider progressBar;
    public TMP_Text timerText; // UI Text to display the timer
    public float progressIncreaseAmount = 10f;
    public float penaltyAmount = 15f; // Penalty amount if time limit is exceeded
    public float offset = 1f; // Offset to adjust timing
    public float BeatInterval = 5f;
    //public int minBeatInterval = 2; // Minimum beat interval in seconds
    //public int maxBeatInterval = 5; // Maximum beat interval in seconds
    private float currentProgress = 0f;
    private float timer = 0f;
    public bool canHit = false;
    public bool timerRunning = false;

    [SerializeField] private float hitCooldown = 1f; // Cooldown period in seconds
    private bool hitRegistered = false; // Flag to track if a hit is registered
    private float cooldownTimer = 0f; // Timer to track the cooldown period


    private void Update()
    {
        if ((hitbox.ItemOnAnvil == true) && (timerRunning ==false))//checks for item on anvil
        {
            StartCoroutine(GameTimer());
            timerRunning = true;
            Debug.Log("Timer starting");
        }
        // Update cooldown timer
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                cooldownTimer = 0f;
                hitRegistered = false; // Reset hitRegistered flag after cooldown
            }
        }

        if (canHit&& !hitRegistered)
        {
            timer += Time.deltaTime;
           // Debug.Log("HIT IT");

            //checks if the player hits the anvil on time or within the delay
            if ((hammer.hitting==true) && timer <= offset)
            {
                Debug.Log("ontime");
                IncreaseProgress();
                hitRegistered = true;
                cooldownTimer = hitCooldown;
            }
        }
        else if ((hammer.hitting == true)&&(canHit==false))
        {
            ApplyPenalty();
            //Debug.Log("Penalty applied!");
            hammer.hitting = false;
        }
        //Debug.Log("hammer.hitting: " + hammer.hitting + ", hitRegistered: " + hitRegistered);
    }
    public void IncreaseProgress()
    {
        currentProgress += progressIncreaseAmount;
        currentProgress = Mathf.Clamp(currentProgress,0f,100f); // Ensure currentProgress is clamped between 0 and 100
        Debug.Log("Current progress: " + currentProgress);

        progressBar.value = currentProgress; // Update the progress bar with the clamped currentProgress

        if (currentProgress >= 100f)
        {
            Debug.Log("Item is fully crafted!");
            anvil.RunMachine();
            currentProgress = 0; // Reset currentProgress after crafting is complete
        }
    }

    public void ApplyPenalty() //Penalty if the player doesnt hit the item on time
    {
        currentProgress -= penaltyAmount;
        currentProgress = Mathf.Clamp(currentProgress, 0f, 100f);
        progressBar.value = currentProgress;
        hammer.Penalty();
        Debug.Log("Penalty applied!");
    }
    private System.Collections.IEnumerator GameTimer()
    {
        timerText.text = "Anvil is ready";
        while (true)
        {
            //int randomBeatInterval = Random.Range(minBeatInterval, maxBeatInterval); // Generate a random beat interval
            yield return new WaitForSeconds(BeatInterval - offset); // Wait for beat interval
            Debug.Log(BeatInterval);
            timer = 0f; // Reset the timer

            // Display the timer countdown 
            for (int i = Mathf.RoundToInt(BeatInterval); i >= 0; i--)
            {
                timerText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
            // Display "HIT" when the timer reaches zero
            timerText.text = "HIT!";
            canHit = true;
            yield return new WaitForSeconds(1f);
            timerText.text = ""; // Clear the timer display
             // allows player to hit
            timerRunning = false;//prevents coroutine from running agains
            // Reset canHit after a delay
            yield return new WaitForSeconds(2f); // Adjust delayTime as needed
            canHit = false; // Reset canHit
            hitRegistered = false;
        }
    }
}