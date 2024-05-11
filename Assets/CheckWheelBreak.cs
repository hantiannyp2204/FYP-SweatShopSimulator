using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWheelBreak : GenericQuest
{
    [SerializeField] private MachineShredder shredder;
    // Update is called once per frame
    void Update()
    {
        if (shredder.GetWheelHandler().GetWheelCurrState() == WheelStatus.BROKEN)
        {
            Destroy(gameObject);
        }
    }
}
