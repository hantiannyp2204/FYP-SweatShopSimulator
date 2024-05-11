using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSmelterFIxed : GenericQuest
{
    [SerializeField] private SmelterBlowUpHitbox blowUpHitbox;

    // Update is called once per frame
    void Update()
    {
        if (blowUpHitbox.IsSmelterFixed())
        {
            Destroy(gameObject);
        }
    }
}
