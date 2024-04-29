using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    //public string toFind;
    private ParticleSystem _particle;
    private RefillFuelManager _fuelManager;
  
    private void OnEnable()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        if (GetStation().shredder.AlreadyFull()) return;

        if (GetStation().gameObject != null)
        {
            GetStation().AddFuelEvent.Invoke();
            //_fuelManager.e_refillFuel?.InvokeEvent(transform.position, Quaternion.identity, transform);
        }
    }

    public void SetCollider(Collider collider)
    {
        _particle.trigger.AddCollider(collider);
    }

    public RefillFuelManager GetStation()
    {
        return _fuelManager;
    }

    public void SetStation(RefillFuelManager fuelManager)
    {
        _fuelManager = fuelManager;
    }
}
