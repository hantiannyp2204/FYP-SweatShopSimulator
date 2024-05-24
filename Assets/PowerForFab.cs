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
    public BoxCollider boxCollider;

    [Header("Feedback Events")]
    [SerializeField] private FeedbackEventData e_powerOutage;
    [SerializeField] private Transform PowerOutageTransform;
    [SerializeField] private Image _PowerBar;

    private void Start()
    {
        Isin = true;
        RandomPower();
        UpdatePowerBar(_PowerForFab, _CurrentPower);
    }

    private void Update()
    {
        if (_PowerPlug == null || _Rigidbody == null || boxCollider == null)
        {
            Debug.LogError("One or more required components are not assigned.");
            return;
        }

        if (_PowerPlug.isStuckInSocket == false)
        {
            _CurrentPower = 0;
        }

        if (_CurrentPower <= 1 && Isin)
        {
            e_powerOutage?.InvokeEvent(transform.position, Quaternion.identity, PowerOutageTransform);
            _Rigidbody.isKinematic = false;
            Debug.Log("Power outage occurred");
            DisableBoxColliderForDuration(2f);
            PushPlugOut();
            Isin = false;
        }

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
            _NewController?.ResetEverything();
            Debug.Log("No power");
        }
    }

    public void UpdatePowerBar(float MaxPower, float CurrentPower)
    {
        if (_PowerBar != null)
        {
            if (MaxPower > 0)
            {
                _PowerBar.fillAmount = Mathf.Clamp01(CurrentPower / MaxPower);
            }
            else
            {
                Debug.LogWarning("MaxPower is zero or negative.");
                _PowerBar.fillAmount = 0;
            }
        }
        else
        {
            Debug.LogWarning("PowerBar Image reference is not set.");
        }
    }

    public void PushPlugOut()
    {
        if (_PowerPlug != null)
        {
            Vector3 forwardDirection = _PowerPlug.transform.forward;
            Rigidbody plugRigidbody = _PowerPlug.GetComponent<Rigidbody>();

            if (plugRigidbody != null)
            {
                plugRigidbody.AddForce(forwardDirection * 5, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("PowerPlug Rigidbody is not found.");
            }
        }
        else
        {
            Debug.LogWarning("PowerPlug reference is not set.");
        }
    }

    private void DisableBoxColliderForDuration(float duration)
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
            StartCoroutine(EnableBoxColliderAfterDelay(duration));
        }
        else
        {
            Debug.LogWarning("BoxCollider reference is not set.");
        }
    }

    private IEnumerator EnableBoxColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }
        else
        {
            Debug.LogWarning("BoxCollider reference is not set.");
        }
    }
}
