using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionQuest : GenericQuest
{
    [SerializeField] private SmelterInputHitbox hitbox;
 
    // Update is called once per frame
    void Update()
    {
        if (hitbox.GetScrapList() != null)
        {
            Destroy(gameObject);
        }
    }
}
