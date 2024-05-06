using System.Collections;
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
        Vector3 targetPosition = CalculateTargetPosition(currentFuel);
        pointerTransform.localPosition = Vector3.Lerp(pointerTransform.localPosition, targetPosition, Time.deltaTime * 5);
    }

    private Vector3 CalculateTargetPosition(float currentFuel)
    {
        Vector3 targetPosition = pointerTransform.localPosition;

        if (currentFuel < 40)
        {
            if (currentFuel == 0)
            {
                targetPosition.x = 2.5f;
            }
            else
            {
                float lerpFactor = currentFuel / 40f; // Normalize the current fuel level between 0 and 40
                targetPosition.x = Mathf.Lerp(2.5f, 0.8f, lerpFactor);
            }
           
            UpdateFuelType(FuelType.Low);
        }
        else if (currentFuel >= 40 && currentFuel <= 100)
        {
            targetPosition.x = Mathf.Lerp(0.8f, -0.8f, (currentFuel - 40) / 60);
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
                targetPosition.x = Mathf.Lerp(-0.8f, -2.5f, (currentFuel - 100) / 100);
            }
            UpdateFuelType(FuelType.High);
        }

        return targetPosition;
    }

    public void AddedFuelPointerUpdate(float newFuel)
    {
        StartCoroutine(SmoothPointerUpdate(newFuel));
    }

    IEnumerator SmoothPointerUpdate(float newFuel)
    {
        Vector3 start = pointerTransform.localPosition;
        Vector3 end = CalculateTargetPosition(newFuel);
        float duration = 0.5f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            pointerTransform.localPosition = Vector3.Lerp(start, end, t);
            yield return null;
        }
        pointerTransform.localPosition = end;
    }

    private void UpdateFuelType(FuelType newType)
    {
        if (currentFuelType != newType)
        {
            currentFuelType = newType;
            lowFuelUI.material = null;
            midFuelUI.material = null;
            highFuelUI.material = null;

            // Assign the glow material to the active fuel type
            switch (newType)
            {
                case FuelType.Low:
                    lowFuelUI.material = UIglowMaterial;
                    break;
                case FuelType.Mid:
                    midFuelUI.material = UIglowMaterial;
                    break;
                case FuelType.High:
                    highFuelUI.material = UIglowMaterial;
                    break;
            }
        }
    }
}