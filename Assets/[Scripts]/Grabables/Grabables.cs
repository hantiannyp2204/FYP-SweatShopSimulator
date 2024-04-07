using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabables : MonoBehaviour
{
    public virtual void Grab()
    {
        Debug.Log("Grabbing " + gameObject.name);
    }
    public virtual void Released()
    {
        Debug.Log("Released " + gameObject.name);
    }
}
