using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item_Usable : Item
{
    public abstract void Use();

    protected override void Start()
    {
        base.Start();
    }
}
