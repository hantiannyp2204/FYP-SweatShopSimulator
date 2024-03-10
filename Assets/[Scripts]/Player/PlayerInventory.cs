using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;

public class PlayerInventory : MonoBehaviour, ISubscribeEvents<Iinventory>
{
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private Item[] itemList = new Item[4];
    [SerializeField] int itemsIn = 0;
    [SerializeField] int currentSelect = 0;
    [SerializeField] float inventoryDropOffset = 0;

    [SerializeField] Transform playerHandPosition;

    // EVENTS
    System.Action<int, int> OnChangeSelect;
    System.Action<int, Item> OnAddItem;
    System.Action<int, Item> OnRemoveItem;
    public int GetInventorySize() => itemList.Length;
    public int GetAmountOfItemsInInvenotry() => itemsIn;

    public Item GetCurrentItem() => itemList[currentSelect];
    public void AddItem(Item item)
    {
        if (IsFull()) return;
        item.ChangeState(ITEM_STATE.PICKED_UP);
        for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i] == null)
            {
                itemList[i] = item;

                itemList[i].transform.SetParent(playerHandPosition);
                // Reset position + their off sets
                itemList[i].transform.localPosition = (itemList[i].Data.GetPosOffset());
                itemList[i].transform.localRotation = (itemList[i].Data.GetRotationOffset());



                Debug.Log("item added to inventory");
                ChangeSelect(i);
                itemsIn++;
                OnAddItem?.Invoke(i, item);
                break;
            }
        }

    }

    public void CallRemoveAll(bool disable = false, bool dropItem = true)
    {
        for (int i = 0; i < itemList.Length; i++)
            RemoveItem(i, disable, dropItem);

    }

    // Remove item at index
    public void RemoveItem(int n, bool disable = false, bool dropItem = true)
    {
        // Return if out of range
        if (n >= itemList.Length) return;

        Item itemRemove = itemList[n];
        // Return if item does not exist in array
        if (itemRemove == null) return;

        itemsIn--;
        // Change item state to not picked up
        itemRemove.ChangeState(ITEM_STATE.NOT_PICKED_UP);
        //for dropping item
        if (dropItem)
        {
            //reset the position
            itemRemove.transform.localPosition = Vector3.zero;
            itemRemove.transform.localRotation = Quaternion.identity;
            //set the collider back
            itemRemove.GetComponent<Collider>().enabled = true;
            // Remove from parent
            itemRemove.transform.SetParent(null);

            // Calculate the drop position in front of the player, offset by half the item's Z scale and the adjustable forward offset
            float forwardOffset = itemRemove.transform.localScale.z / 2 + inventoryDropOffset;
            Vector3 dropPosition = transform.position + transform.forward * forwardOffset;

            // Reset rotation except Y rotation
            Quaternion currentRotation = itemRemove.transform.rotation;
            Quaternion resetRotation = Quaternion.Euler(0, currentRotation.eulerAngles.y, 0);
            itemRemove.transform.rotation = resetRotation;




            // Adjust the drop position to the ground if there's a surface directly below
            if (Physics.Raycast(dropPosition, -Vector3.up, out RaycastHit hit, Mathf.Infinity, ~ignoreLayer))
            {
                dropPosition.y = hit.point.y;
                itemRemove.transform.SetParent(hit.transform.root.transform);
            }
            itemRemove.transform.position = dropPosition;


            Debug.Log("Item removed from inventory");

        }

        if (disable)
            itemRemove.gameObject.SetActive(false);

        OnRemoveItem?.Invoke(n, itemRemove);
        itemList[n] = null;
    }


    // Remove item at current select
    public void RemoveAtCurrentSlot(bool disable = false, bool dropItem = true)
    {
        RemoveItem(currentSelect, disable, dropItem);
    }

    public void CallChangeSelect(int n)
    {
        ChangeSelect(n);

    }
    // Change inventory Slot
    public void ChangeSelect(int n)
    {
        bool hasItem = false;
        int prevSelect = currentSelect;

        currentSelect = n;


        if (currentSelect < 0)
            currentSelect = itemList.Length - 1;
        else if (currentSelect >= itemList.Length)
            currentSelect = 0;

        // Loop all items
        for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i] != null)
            {
                // Enable current item
                if (currentSelect == i)
                {
                    //theres item, lock the bone for right hand
                    hasItem = true;

                    itemList[i].gameObject.SetActive(true);
                    //disables his collider
                    itemList[i].GetComponent<Collider>().enabled = false;
                }

                // Disable item
                else
                    itemList[i].gameObject.SetActive(false);
            }
        }
      
        OnChangeSelect?.Invoke(prevSelect, currentSelect);


    }
    public void ChangeSelectUp() => CallChangeSelect(currentSelect + 1);
    public void ChangeSelectDown() => CallChangeSelect(currentSelect - 1);


    // Check if its full
    public bool IsFull() => itemsIn >= itemList.Length;

    public void UpdateInventory()
    {
        // Input for changing slots
        if (Input.mouseScrollDelta.y > 0)
            ChangeSelectUp();
        else if (Input.mouseScrollDelta.y < 0)
            ChangeSelectDown();


        // Input for Removing Item
        if (Input.GetKeyDown(KeyCode.G))
        {
            RemoveAtCurrentSlot();
        }
    }

    public void SubcribeEvents(Iinventory action)
    {
        OnAddItem += action.OnAddItem;
        OnRemoveItem += action.OnRemoveItem;
        OnChangeSelect += action.OnChangeSelect;
    }

    public void UnsubcribeEvents(Iinventory action)
    {
        OnAddItem -= action.OnAddItem;
        OnRemoveItem -= action.OnRemoveItem;
        OnChangeSelect -= action.OnChangeSelect;
    }
}

public interface Iinventory
{
    public void OnAddItem(int n, Item item);
    public void OnRemoveItem(int n, Item item);
    public void OnChangeSelect(int prevIndex, int currentIndex);
}

