using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderButton : VRButton  
{
       [SerializeField] private MachineShredder shredder;
    [SerializeField] private ShredderSpamButton spamButton;

    public override void PressedFunction()
    {
        //if list is empty, there is nothing in collider
        if (shredder.shredderItemCollider.GetProductList().Count == 0)
        {
            shredder.shredderFuelText.text = "Nothing to Shred";
            return;
        }
        else
        {
            if (!spamButton.gameObject.activeSelf)
            {
                spamButton.gameObject.SetActive(true);
            }
            shredder.RunActive();
        }
    }

    public override void ReleasedFunction()
    {
        shredder.RunDeactive();
    }
}
