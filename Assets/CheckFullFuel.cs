using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFullFuel : GenericQuest
{
    [SerializeField] private MachineShredder shredder;

    // Update is called once per frame
    void Update()
    {
        if (shredder.AlreadyFull())
        {
            shredder.SetValueToComplete(5f);
            shredder.SetBreakValue(shredder.GetRandomValueToBreak());
            Destroy(gameObject);
        }
    }
}
