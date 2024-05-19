using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckShredderFinished : GenericQuest
{
    [SerializeField] private MachineShredder shredder;

    // Update is called once per frame
    void Update()
    {
        if (!shredder.initShredding)
        {
            shredder.GetWheelHandler().chance.SetChance(2);
            Destroy(gameObject);
        }
    }
}
