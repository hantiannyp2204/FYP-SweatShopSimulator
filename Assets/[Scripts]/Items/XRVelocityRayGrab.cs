using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XRVelocityRayGrab : XRGrabInteractable
{
    public float velocityThreshold = 2;
    public float jumpAngleInDegree = 60;

    private XRRayInteractor rayInteractor;
    private Vector3 previousPos;
    private Rigidbody interactableRigidbody;
    private bool canJump = true;
    private bool grabbedByRay = false;
    private bool itemIsHovered = false;
    public bool IsGrabbedByRay() => grabbedByRay;
    public bool CanJump() => canJump;
    protected override void Awake()
    {
        base.Awake();
        interactableRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(isSelected && firstInteractorSelecting is XRRayInteractor && canJump)
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
        if (itemIsHovered && (Vector3.Distance(this.transform.position, hoveredItemTransform.position) >= 0.4f))
        {
            grabbedByRay = true;
        }
        else if (itemIsHovered && !(Vector3.Distance(this.transform.position, hoveredItemTransform.position) >= 0.4f))
        {
            grabbedByRay = false;
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
    Transform hoveredItemTransform;
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
   
        if (args.interactorObject is XRRayInteractor)
        {
            itemIsHovered = true;
            hoveredItemTransform = args.interactorObject.transform;
        }
        
        base.OnHoverEntered(args);
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        if (args.interactorObject is XRRayInteractor)
        {
            itemIsHovered = false;
            hoveredItemTransform = null;
        }
        base.OnHoverExited(args);
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        itemIsHovered = false;
        //seperates what happens if its interacted by ray or direct
        Debug.Log("Distance: " + Vector3.Distance(this.transform.position, args.interactorObject.transform.position));
        //if direct grab
        if(grabbedByRay)
        {

            //if (!canJump)
            //{
            //    return;
            //}
            Debug.Log("SELECTED by Ray");
            trackPosition = false;
            trackRotation = false;
            throwOnDetach = false;

            rayInteractor = (XRRayInteractor)args.interactorObject;
            previousPos = rayInteractor.transform.position;
            canJump = true;

        }

        //if ray grab
        else
        {
          
            Debug.Log("SELECTED by Direct");
            trackPosition = true;
            trackRotation = true;
            throwOnDetach = true;

        }
        //canJump = true;
        base.OnSelectEntered(args);
    }
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {

        //canJump = true;

        base.OnSelectExited(interactor);
    }
}
