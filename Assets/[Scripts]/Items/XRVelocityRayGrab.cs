using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XRVelocityRayGrab : XRGrabInteractable
{
    public float velocityThreshold = 2;
    public float jumpAngleInDegree = 60;

    private XRRayInteractor rayInteractor;
    private Transform hoverInteractorTransform;
    private Vector3 previousPos;
    private Rigidbody interactableRigidbody;
    private bool canJump = false;
    private bool grabbedByRay = false;
    private bool itemIsHovered = false;
    private Vector3 raycastHitPoint;
    public bool IsGrabbedByRay() => grabbedByRay;
    public bool CanJump() => canJump;
    protected override void Awake()
    {
        base.Awake();
        interactableRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(isSelected && rayInteractor != null && canJump && grabbedByRay)
        {
            Vector3 velocity = (rayInteractor.transform.position - previousPos) / Time.deltaTime;
            previousPos = rayInteractor.transform.position;

            //if hand velocity exceeds set value, enable item to fly towards said hand
            if(velocity.magnitude > velocityThreshold)
            {
                Drop();
                interactableRigidbody.velocity = ComputeVelocity();
                canJump = false;

              
            }
        }


    }
    private void FixedUpdate()
    {
        if (itemIsHovered && rayInteractor != null)
        {
            float distanceToItem = Vector3.Distance(rayInteractor.rayEndPoint, hoverInteractorTransform.position);
            if (distanceToItem >= 0.4f)
            {
                grabbedByRay = true;
                trackPosition = false;
                trackRotation = false;
                throwOnDetach = false;
            }
            else
            {
                grabbedByRay = false;
                trackPosition = true;
                trackRotation = true;
                throwOnDetach = true;
            }
        }
    }
    //balastic projectile equation calculation
    public Vector3 ComputeVelocity()
    {
        Vector3 diff = rayInteractor.transform.position - transform.position;
        Vector3 diffXZ = new Vector3(diff.x, 0, diff.z);
        float diffXZLength = diffXZ.magnitude;
        float diffYLength = diff.y;

        float angleInRadian = jumpAngleInDegree * Mathf.Deg2Rad;

        float jumpSpeed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(diffXZLength, 2)
            / (2 * Mathf.Cos(angleInRadian) * Mathf.Cos(angleInRadian) * (diffXZ.magnitude * Mathf.Tan(angleInRadian) - diffYLength)));

        Vector3 jumpVelocityVector = diffXZ.normalized * Mathf.Cos(angleInRadian) * jumpSpeed + Vector3.up * Mathf.Sin(angleInRadian) * jumpSpeed;

        return jumpVelocityVector;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
   
        if (args.interactorObject is XRRayInteractor)
        {
            grabbedByRay = true;
            itemIsHovered = true;
            hoverInteractorTransform = args.interactorObject.transform;
            rayInteractor = (XRRayInteractor)args.interactorObject;
        }
        
        base.OnHoverEntered(args);
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        if (args.interactorObject is XRRayInteractor)
        {
            itemIsHovered = false;
            hoverInteractorTransform = null;
            rayInteractor = null;
        }
        base.OnHoverExited(args);
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {

        //seperates what happens if its interacted by ray or direct
        //if ray grab
        if (grabbedByRay && args.interactorObject is XRRayInteractor)
        {

            //if (!canJump)
            //{
            //    return;
            //}
            Debug.Log("SELECTED by Ray");


            rayInteractor = (XRRayInteractor)args.interactorObject;
            previousPos = rayInteractor.transform.position;
            canJump = true;

        }

        //if direct grab
        else
        {
          
            Debug.Log("SELECTED by Direct");
            //if too far and flying, ignore

            //disable hand render
            DisableHandModels disableHandModelComponent = args.interactorObject.transform.GetComponent<DisableHandModels>();
            if(disableHandModelComponent != null )
            {
                disableHandModelComponent.DisableHandRender();
            }
        }

        base.OnSelectEntered(args);
        itemIsHovered = false;
        //disable hand render

    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {

        DisableHandModels disableHandModelComponent = args.interactorObject.transform.GetComponent<DisableHandModels>();
        if (disableHandModelComponent != null && !disableHandModelComponent.GetActive())
        {
            disableHandModelComponent.EnableHandRender();
        }

    }
}
