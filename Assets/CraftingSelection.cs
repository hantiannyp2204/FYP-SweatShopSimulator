using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSelection : MonoBehaviour
{
    public FabricatorCrafting fabricatorCrafting;
    public CraftingRecepie _craftingRecepie;
    public TabButton TabButton;
    public List<TabButton> tabButtons; // List of tab buttons for each product
    public int currentIndex = 0; // Index of the currently selected product

    void Start()
    {
        // Ensure that at least one tab button exists
        if (tabButtons.Count > 0)
        {
            // Set the first tab button as selected and enable its associated product
            tabButtons[currentIndex].Select();
        }
    }

    void Update()
    {
        // Check for input to navigate between products
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Move to the next product
            SelectNextProduct();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Move to the previous product
            SelectPreviousProduct();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            // Move to the previous product
            fabricatorCrafting.HasChosenCraftingItem = true;
            ConfirmProduct();
        }
    }

    // Method to select the next product
    public void SelectNextProduct()
    {
        // Disable the currently selected product
        tabButtons[currentIndex].Deselect();

        // Increment the index to select the next product
        currentIndex = (currentIndex + 1) % tabButtons.Count;

        // Enable the newly selected product
        tabButtons[currentIndex].Select();
    }
     
    // Method to select the previous product
    public void SelectPreviousProduct()
    {
        // Disable the currently selected product
        tabButtons[currentIndex].Deselect();

        // Decrement the index to select the previous product
        currentIndex = (currentIndex - 1 + tabButtons.Count) % tabButtons.Count;

        // Enable the newly selected product
        tabButtons[currentIndex].Select();
    }

    public void ConfirmProduct()
    {
        _craftingRecepie._ConfirmSelection(currentIndex);
    }
}

