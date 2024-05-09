using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLogic : MonoBehaviour
{
    [SerializeField] private float timeToCool = 5;
    [SerializeField] private FeedbackEventData e_materialSizzleSound;
    [SerializeField] private int poolAmount;
    [SerializeField] private ParticleSystem bubbleParticleEffectPrefab;
    [SerializeField] private Transform bubblePoolTransform;

    private List<ParticleSystem> particlePool = new List<ParticleSystem>();
    private Dictionary<GameObject, ParticleSystem> activeParticles = new Dictionary<GameObject, ParticleSystem>();

    private void Start()
    {
        InitializeParticlePool();
    }

    private void InitializeParticlePool()
    {
        for (int i = 0; i < poolAmount; i++)
        {
            ParticleSystem newParticle = Instantiate(bubbleParticleEffectPrefab, transform);
            newParticle.gameObject.SetActive(false);
            particlePool.Add(newParticle);
        }
    }

    private ParticleSystem GetAvailableParticleSystem()
    {
        foreach (ParticleSystem particle in particlePool)
        {
            if (!particle.gameObject.activeInHierarchy)
            {
                return particle;
            }
        }

        // Optionally extend the pool dynamically if all particles are in use
        ParticleSystem newParticle = Instantiate(bubbleParticleEffectPrefab, transform);
        newParticle.gameObject.SetActive(false);
        particlePool.Add(newParticle);
        return newParticle;
    }


    private void OnTriggerEnter(Collider other)
    {
        FreshRawMaterial freshRawMaterialScript = other.GetComponent<FreshRawMaterial>();
        if (freshRawMaterialScript != null)
        {
            e_materialSizzleSound?.InvokeEvent(transform.position, Quaternion.identity);
            freshRawMaterialScript.CoolMaterial(timeToCool);

            ParticleSystem particleSystem = GetAvailableParticleSystem();
            particleSystem.transform.position = other.transform.position;
            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();

            activeParticles[other.gameObject] = particleSystem;

            // Register for the destroy event
            freshRawMaterialScript.OnMaterialDestroyed += HandleMaterialDestroyed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HandleObjectExit(other.gameObject);
    }

    private void Update()
    {
        // Update all active particle systems to follow their respective objects
        foreach (var pair in activeParticles)
        {
            pair.Value.transform.position = pair.Key.transform.position;
        }
    }
    private void HandleMaterialDestroyed(GameObject obj)
    {
        HandleObjectExit(obj);
    }

    private void HandleObjectExit(GameObject obj)
    {
        if (activeParticles.TryGetValue(obj, out ParticleSystem particleSystem))
        {
            particleSystem.Stop();
            particleSystem.gameObject.SetActive(false);
            activeParticles.Remove(obj);
        }

        // Unsubscribe from the event
        FreshRawMaterial freshRawMaterialScript = obj.GetComponent<FreshRawMaterial>();
        if (freshRawMaterialScript != null)
        {
            freshRawMaterialScript.OnMaterialDestroyed -= HandleMaterialDestroyed;
        }
    }
}