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
    [SerializeField] private GameObject center;

    private void Start()
    {

        _fuelManager = refillStation.GetComponent<RefillFuelManager>();

        if (_fuelManager == null) return;

        base.Start(); // need to use base start still

    }

    public override void UseFunction()
    {
        //if (Vector3.Distance(refillStation.transform.position, player.transform.position) >= saidDistance)
        //{
        //    return;
        //}

        // Calculate spawn position in front of the refill can, aligned with player's forward direction
        Vector3 spawnPosition = center.transform.position;

        GameObject spawnGasoline = Instantiate(gasolineParticle, spawnPosition,  player.transform.rotation);
     


        shredder.secretHealth = 0;
        _fuelManager.activateRefill = true;

        base.UseFunction();
    }

    public override void UseReleaseFunction()
    {
        base.UseReleaseFunction();
    }
}
