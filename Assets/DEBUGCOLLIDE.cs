using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGCOLLIDE : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Product")
        { 
           Debug.Break();           
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
