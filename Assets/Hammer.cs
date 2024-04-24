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
            game.IncreaseProgress();
            Hit();
            hitting = true;
            Debug.Log("Hit!");
        }
        else
        {
            hitting = false;
        }
       
    }

    public void Hit()
    {
        // Reduce the hammer's health based on damage per hit
        currentHealth -= (int)damagePerHit;
        Debug.Log("Hammer health:" + currentHealth);

        // Check if the hammer's health has dropped to zero or below
        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Destroy the hammer if health is 0
        }
    }
    public void Penalty()
    {
        // Reduce the hammer's health based on damage per hit
        currentHealth -= (int)damagePerHit*2;
        
        Debug.Log("Hammer health:" + currentHealth);
    }
}
