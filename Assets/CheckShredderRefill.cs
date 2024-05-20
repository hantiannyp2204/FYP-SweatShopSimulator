using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckShredderRefill : GenericQuest
{
    [SerializeField] private MachineShredder shredder;

    // Update is called once per frame
    void Update()
    {
        shredder.SetBreakValue(500);

        if (shredder.IsOutOfFuel())
        {
            shredder.SetBreakValue(shredder.GetRandomValueToBreak());
            Destroy(gameObject);
        }
    }
}
