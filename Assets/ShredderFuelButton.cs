using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderFuelButton : VRButton
{
    [SerializeField] private MachineShredder shredder;
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(false);
        base.Start();
    }
    public override void PressedFunction()
    {
        shredder.FillFuel();
    }
}
