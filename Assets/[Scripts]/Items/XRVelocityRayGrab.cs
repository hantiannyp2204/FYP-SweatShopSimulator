using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static OVRPlugin;

public class XRVelocityRayGrab : XRGrabInteractable
{
    public float velocityThreshold = 2;
    public float jumpAngleInDegree = 60;

    private XRRayInteractor rayInteractor;
    private XRBaseInteractor hoveringInteractor;
    private Vector3 previousPos;
    private Rigidbody interactableRigidbody;
    private bool canJump = false;
    private bool grabbedByRay = false;
    private bool itemIsHovered = false;
    private GeneralItem item;
    public bool IsGrabbedByRay() => grabbedByRay;
    public bool CanJump() => canJump;
    protected override void Awake()
    {
        base.Awake();
        interactableRigidbody = GetComponent<Rigidbody>();
        item = GetComponent<GeneralItem>();
    }

    private void Update()
    {
        if (isSelected && rayInteractor != null && canJump && grabbedByRay)
        {
            Vector3 velocity = (rayInteractor.transform.position - previousPos) / Time.deltaTime;
            previousPos = rayInteractor.transform.position;

            //if hand velocity exceeds set value, enable item to fly towards said hand
            if (velocity.magnitude > velocityThreshold)
            {
                Drop();
                interactableRigidbody.velocity = ComputeVelocity();
                canJump = false;


            }
        }


    }
    private void FixedUpdate()
    {
        if (isHovered && hoveringInteractor is XRRayInteractor)
        {
            Debug.Log("NIGGERS");
            float distanceToItem = Vector3.Distance(this.transform.position, hoveringInteractor.transform.position);
            Debug.Log("distance to item: " + distanceToItem);
            if (distanceToItem > 1)
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

        // Ensure the denominator is never zero or negative which can result in NaN values
        float denominator = 2 * Mathf.Cos(angleInRadian) * Mathf.Cos(angleInRadian) * (diffXZLength * Mathf.Tan(angleInRadian) - diffYLength);
        if (denominator <= 0.001f) // A small epsilon value to avoid division by zero
        {
            Debug.LogError("Invalid denominator value: " + denominator);
            return Vector3.zero; // Return zero velocity to avoid errors
        }

        float jumpSpeed = Mathf.Sqrt(-Physics.gravity.y * diffXZLength * diffXZLength / denominator);

        // Ensure jumpSpeed is not NaN
        if (float.IsNaN(jumpSpeed))
        {
            Debug.LogError("Computed jumpSpeed is NaN");
            return Vector3.zero;
        }

        Vector3 jumpVelocityVector = diffXZ.normalized * Mathf.Cos(angleInRadian) * jumpSpeed + Vector3.up * Mathf.Sin(angleInRadian) * jumpSpeed;
        return jumpVelocityVector;
    }
    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        base.OnHoverEntering(args);
    }
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        hoveringInteractor = (XRBaseInteractor)args.interactorObject;
        if (args.interactorObject is XRRayInteractor)
        {
            grabbedByRay = true;
            itemIsHovered = true;
        }
        //update the hand text
        VRHandRenderers disableHandModelComponent = args.interactorObject.transform.GetComponent<VRHandRenderers>();
        GeneralItem item = args.interactableObject.transform.GetComponent<GeneralItem>();
        if (disableHandModelComponent != null && item != null)
        {
            disableHandModelComponent.SetItemHoverName(item.Data.itemName);
        }
        base.OnHoverEntered(args);
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        if (args.interactorObject is XRRayInteractor)
        {
            itemIsHovered = false;
        }
        //update the hand text
        VRHandRenderers disableHandModelComponent = args.interactorObject.transform.GetComponent<VRHandRenderers>();
        if (disableHandModelComponent != null)
        {
            disableHandModelComponent.ResetItemHoverName();
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
            //reset all variables
            rayInteractor = null;
            canJump = false;
            grabbedByRay = false;
            trackPosition = true;
            trackRotation = true;
            throwOnDetach = true;

            //if was flying, remove it's rb velocity
            interactableRigidbody.velocity = Vector3.zero;

            //snap the object's transfor to the player's hand incase if goes off
            args.interactableObject.transform.position = args.interactorObject.transform.position;
            args.interactableObject.transform.rotation = args.interactorObject.transform.rotation;



            //disable hand render
            VRHandRenderers disableHandModelComponent = args.interactorObject.transform.GetComponent<VRHandRenderers>();
            if (disableHandModelComponent != null)
            {
                disableHandModelComponent.DisableHandRender();
            }

            //play pick up sound
            item?.GetComponent<BaseItem>().PlayEquipSound();
        }

        base.OnSelectEntered(args);
        itemIsHovered = false;

    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {

        VRHandRenderers disableHandModelComponent = args.interactorObject.transform.GetComponent<VRHandRenderers>();
        if (disableHandModelComponent != null && !disableHandModelComponent.GetActive())
        {
            disableHandModelComponent.EnableHandRender();
        }

    }
    //prevent fresh materail from being selected by left hand
    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        // Check if this object has the Fresh script
        if (GetComponent<FreshRawMaterial>() != null && interactor.transform.gameObject.layer == LayerMask.NameToLayer("Left hand interactors"))
        {
            return false; // Prevent grabbing if the Fresh script is present
        }

        // Call the base method to preserve default behavior
        return base.IsSelectableBy(interactor);
    }
    private void OnCollisionEnter(Collision collision)
    {
        item?.GetComponent<BaseItem>().PlayCollisionSound();
    }
}