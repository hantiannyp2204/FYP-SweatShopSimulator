using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnvilGame : MonoBehaviour
{
    public float hitInterval = 2f; // Interval between hits in seconds
    public float tolerance = 0.2f; // Tolerance for hitting early or late
    [SerializeField] private Hammer hammer; // Reference to the hammer object
    [SerializeField] private MachineAnvil anvil;
    //public Slider progressBar; // Reference to the progress bar UI element
   

    private bool canHit = true;
    private int hitsCount = 0;
    private float progress = 0f;

    private void Start()
    {
        //UpdateProgressBar();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the hammer object and if the player can currently hit the anvil
        if ((collision.gameObject.name=="Hammer") && canHit)
        {
            float hitTime = Time.time;
            float anvilTime = hitTime % hitInterval;

            // Check if the player hit the anvil within the acceptable time window
            if (anvilTime < tolerance || anvilTime > hitInterval - tolerance)
            {
                StartCoroutine(HitCooldown());
                HitAnvil();
               
            }
            else
            {
                Debug.Log("Penalty");
                hammer.Penalty();
            }
        }
    }

    IEnumerator HitCooldown()
    {
        canHit = false;
        yield return new WaitForSeconds(hitInterval);
        canHit = true;
    }

    void HitAnvil()
    {
        hitsCount++;
        progress = (float)hitsCount / 5f; // Assuming 10 hits form the product

        if (progress >= 1f)
        {
            anvil.RunMachine();
            ResetBar();
        }
        hammer.Hit();
        //UpdateProgressBar();
    }

    //void UpdateProgressBar()
    //{
    //    progressBar.value = progress;
    //}

    void ResetBar()
    {
        // Reset progress and hits count
        progress = 0f;
        hitsCount = 0;

        // Update the progress bar
        //UpdateProgressBar();
    }
}