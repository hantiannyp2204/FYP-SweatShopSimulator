using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public string toFind;
    private ParticleSystem _particle;
    private RefillFuelManager _fuelManager;

    // list to contain particles that collide
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    private int _counter = 0;
  
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
        }
        //_counter++;

        //if (_counter >= 10)
        //{
        //    _counter = 0;

        //    if (GetStation() != null)
        //    {
        //        GetStation().AddFuelEvent.Invoke();
        //    };
        //}
        
        //int numEnter = _particle.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        //for (int i = 0; i < numEnter; ++i)
        //{
        //    _counter++;

        //    if (_counter >= 10)
        //    {
        //        _counter = 0;
        //    }
        //    ParticleSystem.Particle p = enter[i];
        //}
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
