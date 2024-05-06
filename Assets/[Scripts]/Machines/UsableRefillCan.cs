using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableRefillCan : VRItemUsable
{
    [SerializeField] private GameObject gasolineParticle;
    [SerializeField] private MachineShredder shredder;

    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject refillStation;

    [SerializeField] private GameObject shootFrom;

    private RefillFuelManager _fuelManager;
    private GameObject _storedObject;

    private bool _isHolding = false;

    private new void Start()
    {
        _fuelManager = refillStation.GetComponent<RefillFuelManager>();

        if (_fuelManager == null) return;

        base.Start(); // need to use base start still
    }

    private void Update()
    {
        if (_isHolding)
        {
            if (GetStored() == null)
            {
                GameObject gasolineInstance = Instantiate(gasolineParticle, shootFrom.transform.position, shootFrom.transform.rotation);
                SetStored(gasolineInstance);

                ParticleTrigger trigger = gasolineInstance.GetComponent<ParticleTrigger>();
                if (trigger == null) { return; }

                trigger.SetCollider(refillStation.GetComponent<Collider>());
                trigger.SetStation(refillStation.GetComponent<RefillFuelManager>());
            }
            else
            {
                // Update position of the gasoline particle to follow the player's hand position
                GetStored().transform.position = shootFrom.transform.position;
            }
        }
    }

    public override void UseFunction()
    {
        _isHolding = true;

        base.UseFunction();
    }

    public override void UseReleaseFunction()
    {
        _isHolding = false;

        Destroy(GetStored(), 1);

        base.UseReleaseFunction();
    }

    private void SetStored(GameObject store)
    {   
        _storedObject = store;
    }

    private GameObject GetStored()
    {
        return _storedObject;
    }
}
