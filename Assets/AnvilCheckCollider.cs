using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilCheckCollider : GenericQuest
{
    [SerializeField] private AnvilHitbox hitbox;
    // Update is called once per frame
    void Update()
    {
        if (hitbox.GetRMaterialList().Count != 0)
        {
            Destroy(gameObject);
        }
    }
}
