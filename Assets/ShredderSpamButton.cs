using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderSpamButton : VRButton
{
    [SerializeField] private MachineShredder shredder;

    private void Start()
    {
        transform.gameObject.SetActive(false);
        base.Start();
    }

    public override void PressedFunction()
    {
        shredder.RunSpamButton();
    }
}
