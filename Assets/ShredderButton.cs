using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class ShredderButton : VRButton  
{
    [SerializeField] private MachineShredder shredder;
    [SerializeField] private ShredderSpamButton spamButton;

    private void Start()
    {
        base.Start();
        shredder.lever.onLeverDeactivate.AddListener(PressedFunction); // when activate do what?
    }
    public override void PressedFunction()
    {
        ////if list is empty, there is nothing in collider
        //if (shredder.shredderItemCollider.GetProductList().Count == 0)
        //{
        //    shredder.shredderFuelText.text = "Nothing to Shred";
        //    return;
        //}
        //else
        //{
        //    if (!spamButton.gameObject.activeSelf)
        //    {
        //        spamButton.gameObject.SetActive(true);
        //    }
        //    shredder.RunActive();
        //}
        //if (!spamButton.gameObject.activeSelf)
        //{
        //    spamButton.gameObject.SetActive(true);
        //}

        // testing
        if (!shredder.wheel.gameObject.activeSelf)
        {
            shredder.wheel.gameObject.SetActive(true);
        }
        shredder.RunActive();
    }

    public override void ReleasedFunction()
    {
        shredder.RunDeactive();
    }
}
