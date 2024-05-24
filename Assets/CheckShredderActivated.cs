using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckShredderActivated : GenericQuest
{
    [SerializeField] private MachineShredder shredder;
    // Update is called once per frame
    void Update()
    {
        shredder.SetBreakValue(500);

        if (shredder.initShredding)
        {
            Destroy(gameObject);
        }
    }
}
