using UnityEngine;
using UnityEngine.UI;

public class AnvilGame2 : MonoBehaviour
{
    public Slider progressBar;
    public float progressIncreaseAmount = 10f;
    public float Newprogress;
    //public float timeLimit = 10f; // Time limit in seconds
    public float penaltyAmount = 15f; // Penalty amount if time limit is exceeded
    [SerializeField] private Hammer hammer;
    [SerializeField] private MachineAnvil anvil;
    private float currentProgress = 0f;
    public float delayBeforeHit = 5f;
    private float delayTimer = 0f;
    //private float timer = 0f;
    public bool canHit = false;

    private void Update()
    {
        if (!canHit)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= delayBeforeHit)
            {
                canHit = true;
                Debug.Log("hit the item!");
                delayTimer = 0;
            }
        }
       // Debug.Log(delayTimer);
        //if (timerRunning)
        //{
        //    timer += Time.deltaTime;

        //    if (timer >= timeLimit)
        //    {
        //        ApplyPenalty();
        //        ResetTimer();
        //    }
        //}
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
        canHit = false;
    }

    public void ApplyPenalty()
    {
        currentProgress -= penaltyAmount;
        currentProgress = Mathf.Clamp(currentProgress, 0f, 100f);
        progressBar.value = currentProgress;
        //hammer.Penalty();
        Debug.Log("Penalty applied!");
    }
}