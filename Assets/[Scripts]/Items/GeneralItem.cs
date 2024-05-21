using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneralItem : BaseItem
{
    [HideInInspector] public Material originalMaterial;
    [SerializeField] private ItemData data;
    public ItemData Data => data;
    [SerializeField] private ITEM_STATE itemState;

    //[SerializeField] private FeedbackEventData e_drop;

   // [SerializeField] private LayerMask groundLayer;

    List<Collider> ignoredColliderList = new();

    public string GetInteractName() => "Interact with: " + data.GetName();

    private MeshRenderer _renderer;

    
    private void Start()
    {
        //originalMaterial = GetComponentInChildren<Material>();

        //Debug.Log("say my: " + originalMaterial);
        if (originalMaterial == null)
        {
            return;
        }
        _renderer = GetComponentInChildren<MeshRenderer>();
        if (_renderer == null)
        {
            return;
        }
    }

   
    public void SetMaterial(Material toSwitch)
    {
        _renderer.material = toSwitch;
    }

    public bool CanInteract()
    {
        return !CompareState(ITEM_STATE.PICKED_UP);
    }
    public float GetInteractingLast() => 0f;

    public void ChangeState(ITEM_STATE newState)
    {
        itemState = newState;
        switch (itemState)
        {
            case ITEM_STATE.NOT_PICKED_UP:
                //e_drop?.InvokeEvent(transform.position, Quaternion.identity, transform);
                break;
            case ITEM_STATE.PICKED_UP:
                //e_pickUp?.InvokeEvent(transform.position, Quaternion.identity, transform);
                break;
            default:
                break;
        }
    }

    public bool CompareState(ITEM_STATE checkState)
    {
        return itemState == checkState;
    }
    public ITEM_STATE GetState()=> itemState;

    //for collision
    public void IgnoreCollision(Collider objectToIgnore)
    {
        List<Collider> itemColliderList = ObtainAllColliders();
        foreach (Collider itemCollider in itemColliderList)
        {
            Physics.IgnoreCollision(itemCollider, objectToIgnore, true);
        }

        ignoredColliderList.Add(objectToIgnore);
    }
    public void ResetIgnoreCollisions()
    {
        List<Collider> itemColliderList = ObtainAllColliders();
        foreach (Collider ignoredColliders in ignoredColliderList)
        {
            foreach (Collider itemCollider in itemColliderList)
            {
                Physics.IgnoreCollision(ignoredColliders, itemCollider, false);
            }
        }
        ignoredColliderList.Clear();
    }
    List<Collider> ObtainAllColliders()
    {
        List<Collider> allColliders = new();
        Collider[] childColliders = GetComponentsInChildren<Collider>();
        foreach (Collider childCollider in childColliders)
        {
            allColliders.Add(childCollider);
        }
        return allColliders;
    }

    public enum ITEM_STATE
    {
        NOT_PICKED_UP,
        PICKED_UP
    }
}