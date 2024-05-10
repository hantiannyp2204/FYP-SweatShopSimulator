using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckShredderInput : GenericQuest
{
    [SerializeField] private VrMachineItemCollider inputCollider;

    // Update is called once per frame
    void Update()
    {
        if (inputCollider.GetProductList().Count != 0) // something has been put inside jaw
        {
            Destroy(gameObject);
        }
    }
}
