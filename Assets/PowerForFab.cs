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
    public bool Isin = true;
    public Rigidbody _Rigidbody;
    public BoxCollider boxCollider; // Reference to the BoxCollider component


    [Header("Feedback Events")]
    [SerializeField] private FeedbackEventData e_powerOutage;
    [SerializeField] private Transform PowerOutageTransform;
    [SerializeField] private Image _PowerBar;
    public void Update()
    {
        if (_PowerPlug.isStuckInSocket == false)
        {
            _CurrentPower = 0;
        }
        if (_CurrentPower <= 1)
        {
            if (Isin == true)
            {
                e_powerOutage?.InvokeEvent(transform.position, Quaternion.identity, PowerOutageTransform);
                _Rigidbody.isKinematic = false;
                Debug.Log("Isin");
                DisableBoxColliderForDuration(2f);
                PushPlugOut();
                Isin = false;
            }
        }

        
    }



    public void Start()
    {
        Isin = true;
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
        // Use the local forward direction of the PowerPlug to push it along the positive Z direction
        Vector3 forwardDirection = _PowerPlug.transform.forward; // Assuming forward is the positive Z direction in local space
        _PowerPlug.GetComponent<Rigidbody>().AddForce(forwardDirection * 5, ForceMode.Impulse);
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
