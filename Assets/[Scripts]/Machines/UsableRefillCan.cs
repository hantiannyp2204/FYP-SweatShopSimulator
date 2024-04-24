using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableRefillCan : VRItemUsable
{
    [SerializeField] private GameObject gasolineParticle;
    [SerializeField] private float saidDistance;
    [SerializeField] private MachineShredder shredder;

    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject refillStation;

    private RefillFuelManager _fuelManager;

    [SerializeField] private GameObject shootFrom;

    private void Start()
    {
        _fuelManager = refillStation.GetComponent<RefillFuelManager>();

        if (_fuelManager == null) return;

        base.Start(); // need to use base start still
    }

    public override void UseFunction()
    {
        GameObject spawnGasoline = Instantiate(gasolineParticle, shootFrom.transform.position, shootFrom.transform.rotation);

        ParticleTrigger trigger = spawnGasoline.GetComponent<ParticleTrigger>();
        if (trigger == null) return;

        trigger.SetCollider(refillStation.GetComponent<Collider>());
        trigger.SetStation(refillStation.GetComponent<RefillFuelManager>());
     
        //shredder.secretHealth = 0;
        //_fuelManager.activateRefill = true;

        base.UseFunction();
    }

    public override void UseReleaseFunction()
    {
        base.UseReleaseFunction();
    }
}
