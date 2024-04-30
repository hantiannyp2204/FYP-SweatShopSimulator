using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerForFab : MonoBehaviour
{
    public float _PowerForFab;
    public float _CurrentPower;
    public bool _IsTherePower = false;
    public NewController _NewController;
    public PowerPlug _PowerPlug;

    [SerializeField] private Image _PowerBar;
    public void Update()
    {
        if (_PowerPlug.isStuckInSocket == false)
        {
            _CurrentPower = 0;
        }
    }
    public void Start()
    {
        UpdatePowerBar(_PowerForFab, _CurrentPower);
    }
    public void RandomPower()
    {
        _PowerForFab = Random.Range(50f, 100f);
        _CurrentPower = _PowerForFab;
        Debug.Log("Random power generated: " + _PowerForFab);
    }

    public void CheckIfGotPower()
    {
        if (_CurrentPower >= 0)
        {
            _IsTherePower=true; 
        }
        else
        {

            _NewController.ResetEverything();
            Debug.Log("No power");
        }
    }

    public void UpdatePowerBar(float MaxPower, float CurrentPower)
    {
        _PowerBar.fillAmount = CurrentPower / MaxPower;
    }
}
