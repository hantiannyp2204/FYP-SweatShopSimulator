using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketSync : MonoBehaviour
{
    private XRSocketInteractor _interactor;

    // Start is called before the first frame update
    void Start()
    {
        _interactor = GetComponent<XRSocketInteractor>();
        if (_interactor == null)
        {
            return;
        }
    }

    public IXRSelectInteractable GetObjectInteracted()
    {
        IXRSelectInteractable a = _interactor.GetOldestInteractableSelected();
        return a;
    }

    public void homie()
    {
        IXRSelectInteractable b = GetObjectInteracted();
      
        if (b != null)
        { 
            Debug.Log("jj:" + b.transform.gameObject.name);
            GeneralItem c = b.transform.GetComponent<GeneralItem>();
            if (c != null)
            {
                _interactor.enabled = false;
               // c.EnableItemPhysics();
            }
        }
        else
        {
            _interactor.enabled = true;
            //// object is not null
            GeneralItem d = b.transform.GetComponent<GeneralItem>();
            if (d != null)
            {
                //d.DisableItemPhysics();
            }
            Debug.Log("hi");
        }
    }

    private void Update()
    {
        homie();
    }
}
