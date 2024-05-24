using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckShredderActivated : GenericQuest
{
    [SerializeField] private MachineShredder shredder;
    // Update is called once per frame
    void Update()
    {

        if (shredder.initShredding)
        {
            shredder.SetBreakValue(500);
            Destroy(gameObject);
        }
    }
}
