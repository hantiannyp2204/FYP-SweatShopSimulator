using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderMouthCollision : MonoBehaviour
{
    public GameObject mouth1;
    public GameObject mouth2;

    [SerializeField] private List<GameObject> _itemsOnJaw = new List<GameObject>();

    private void Start()
    {
        if (mouth1 == null || mouth2 == null) return;
    }

    private void OnTriggerEnter(Collider other)
    {
        _itemsOnJaw.Add(other.gameObject);
        if (_itemsOnJaw.Count == 1)
        {
            if (_itemsOnJaw[0].TryGetComponent<Item>(out Item item))
            {
                if (item.gameObject.CompareTag("Product"))
                {
                    DisableJaw();
                    return;
                }
            }
            if (_itemsOnJaw[0].TryGetComponent<RawMaterial>(out RawMaterial raw))
            {
                DisableJaw();
                return;
            }
            EnableJaw();
        }
        else EnableJaw();
    }

    private void OnTriggerExit(Collider other)
    {
        _itemsOnJaw.Remove(other.gameObject);
        if (_itemsOnJaw.Count == 1)
        {
            if (_itemsOnJaw[0].TryGetComponent<Item>(out Item item))
            {
                if (item.gameObject.CompareTag("Product"))
                {
                    DisableJaw();
                    return;
                }
            }
            if (_itemsOnJaw[0].TryGetComponent<RawMaterial>(out RawMaterial raw))
            {
                DisableJaw();
                return;
            }
            EnableJaw();
        }
        else EnableJaw();
    }

    public void EnableJaw()
    {
        mouth1.GetComponent<Animator>().SetBool("isActivate", true);
        mouth2.GetComponent<Animator>().SetBool("isActivate", true);
    }

    public void DisableJaw()
    {
        mouth1.GetComponent<Animator>().SetBool("isActivate", false);
        mouth2.GetComponent<Animator>().SetBool("isActivate", false);
    }
}
