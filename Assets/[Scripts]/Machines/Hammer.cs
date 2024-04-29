using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public AnvilGame2 game; // Reference to the Anvil script
    public int maxHealth = 100; // Maximum health of the hammer
    public int currentHealth; // Current health of the hammer
    public float damagePerHit = 5f; // Damage inflicted per hit
    public bool hitting = false;

    private void Start()
    {
        currentHealth = maxHealth; // Initialize current health to maximum health
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an item on the anvil
        if (collision.gameObject.CompareTag("RawMaterial"))
        {
            // Increase the progress of the anvil
            hitting = true;
            Debug.Log("bam");
        }
       
    }
}
