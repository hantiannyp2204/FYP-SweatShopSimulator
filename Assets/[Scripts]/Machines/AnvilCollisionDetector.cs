using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilCollisionDetector : MonoBehaviour
{
    [SerializeField] private MachineAnvil anvil;
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the hammer
        if (collision.gameObject.name=="Hammer")
        {
            Debug.Log("Interacting with hammer");
            // Call the RunMachine() function in the MachineAnvil script
            anvil.RunMachine();
        }
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
