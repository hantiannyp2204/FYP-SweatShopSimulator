using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckShredderRefill : GenericQuest
{
    [SerializeField] private MachineShredder shredder;
    // Update is called once per frame
    void Update()
    {
        if (shredder.IsOutOfFuel())
        {
            Destroy(gameObject);
        }
    }
}
