using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    public GameObject associatedGameObject; // Reference to the associated game object
    // Method to handle button selection
    public void Select()
    {
        associatedGameObject.SetActive(true);
    }

    // Method to handle button deselection
    public void Deselect()
    {
        associatedGameObject.SetActive(false);
    }
}
