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

    public BoxCollider boxCollider; // Reference to the BoxCollider component


    [SerializeField] private Image _PowerBar;
    public void Update()
    {
        if (_PowerPlug.isStuckInSocket == false)
        {
            _CurrentPower = 0;
        }
        if (_CurrentPower <= 1)
        {
            DisableBoxColliderForDuration(2f);
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
        if (_CurrentPower > 0)
        {
            _IsTherePower = true; 
        }
        else
        {
            _IsTherePower = false;
            _NewController.ResetEverything();
            Debug.Log("No power");
        }
    }

    public void UpdatePowerBar(float MaxPower, float CurrentPower)
    {
        _PowerBar.fillAmount = CurrentPower / MaxPower;
    }

    public void PushPlugOut()
    {
        Vector3 direction = (_PowerPlug.Start_Plug.position - _PowerPlug.End_Plug.position).normalized;
        _PowerPlug.GetComponent<Rigidbody>().AddForce(-direction * 25, ForceMode.Impulse);
    }

    void DisableBoxColliderForDuration(float duration)
    {
        
        // Disable the BoxCollider
        boxCollider.enabled = false;

        // Start a coroutine to re-enable the collider after the specified duration
        StartCoroutine(EnableBoxColliderAfterDelay(duration));
    }

    IEnumerator EnableBoxColliderAfterDelay(float delay)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(delay);

        // Re-enable the BoxCollider
        boxCollider.enabled = true;
    }
}
