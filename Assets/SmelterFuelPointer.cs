using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmelterFuelPointer : MonoBehaviour
{
    [SerializeField] private Transform pointerTransform; // Assign the transform of the GameObject that needs to move
    [SerializeField] private Material UIglowMaterial;
    [SerializeField] private Image lowFuelUI;
    [SerializeField] private Image midFuelUI;
    [SerializeField] private Image highFuelUI;

    enum FuelType
    {
        Low,
        Mid,
        High
    }

    private FuelType currentFuelType = FuelType.Low;

    private void Start()
    {
        // Set initial position of the pointer
        pointerTransform.localPosition = new Vector3(-0.8f, -0.06f, 0.11f);
        // Set the initial material for the midFuel UI
        midFuelUI.material = UIglowMaterial;
    }
    public void UpdatePosition(float currentFuel)
    {
        Vector3 targetPosition = pointerTransform.localPosition;

        if (currentFuel < 40)
        {
            float lerpFactor = currentFuel / 40f; // Normalize the current fuel level between 0 and 40
            targetPosition.x = Mathf.Lerp(2.5f, 0.8f, lerpFactor);
            UpdateFuelType(FuelType.Low);
        }
        else if (currentFuel >= 40 && currentFuel <= 100)
        {
            targetPosition.x = Mathf.Lerp(0.8f, -0.5f, (currentFuel - 40) / 60);
            UpdateFuelType(FuelType.Mid);
        }
        else if (currentFuel > 100 && currentFuel <= 200)
        {
            if (currentFuel == 200)
            {
                targetPosition.x = -2.5f;
            }
            else
            {
                targetPosition.x = Mathf.Lerp(-0.5f, -2.5f, (currentFuel - 100) / 100);
            }
            UpdateFuelType(FuelType.High);
        } 
        pointerTransform.localPosition = Vector3.Lerp(pointerTransform.localPosition, targetPosition, Time.deltaTime * 5);
    }

    private void UpdateFuelType(FuelType newType)
    {
        if (currentFuelType != newType)
        {
            currentFuelType = newType;


            // Assign the glow material to the active fuel type
            switch (newType)
            {
                case FuelType.Low:
                    midFuelUI.material = null;
                    lowFuelUI.material = UIglowMaterial;
                    break;
                case FuelType.Mid:
                    lowFuelUI.material = null;
                    highFuelUI.material = null;
                    midFuelUI.material = UIglowMaterial;
                    break;
                case FuelType.High:
                    midFuelUI.material = null;
                    highFuelUI.material = UIglowMaterial;
                    break;
            }
        }
    }
}