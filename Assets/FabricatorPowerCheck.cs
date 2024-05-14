using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricatorPowerCheck : GenericQuest
{
    [SerializeField] private PowerForFab powerCheck;
    // Update is called once per frame
    void Update()
    {
        if (powerCheck._IsTherePower) // check power plugged in
        {
            Destroy(gameObject);
        }
    }
}
