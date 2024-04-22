using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerForFab : MonoBehaviour
{
    public float _PowerForFab;
    public bool _IsTherePower = false;
    public NewController _NewController;

    public void Start()
    {
        RandomPower();
    }
    public void RandomPower()
    {
        _PowerForFab = Random.Range(50f, 100f);
        Debug.Log("Random power generated: " + _PowerForFab);
    }

    public void CheckIfGotPower()
    {
        if (_PowerForFab >= 0)
        {
            _IsTherePower=true;
        }
        else
        {
            _NewController.ResetEverything();
            Debug.Log("No power");
        }
    }
}
