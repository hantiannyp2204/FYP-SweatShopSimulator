using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreshRawMaterial : MonoBehaviour
{
    [SerializeField] float timeToCool = 5;
    List<Material[]> initialMaterials = new();
    float timeLeft;
    Coroutine coolDownHandler;
    private void Start()
    {
        timeLeft = timeToCool;
       
    }
    public void ApplyHotTexture(Material hotMaterial)
    {
        // Retrieve all MeshRenderer components on the GameObject and its children
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        int currentIndex = 0;
        foreach (MeshRenderer renderer in meshRenderers)
        {
            // Get the current materials array of the renderer
            initialMaterials.Add(renderer.materials);

            // Create a new array with one extra slot for the hotMaterial
            Material[] newMaterials = new Material[initialMaterials[currentIndex].Length + 1];

            // Copy the existing materials to the new array
            for (int i = 0; i < initialMaterials[currentIndex].Length; i++)
            {
                newMaterials[i] = initialMaterials[currentIndex][i];
            }

            // Add the hotMaterial to the last slot of the new array
            newMaterials[newMaterials.Length - 1] = hotMaterial;

            // Assign the new materials array back to the renderer
            renderer.materials = newMaterials;

            foreach(Material appliedMaterial in renderer.materials)
            {
                Debug.Log(appliedMaterial.name);
            }

            currentIndex++;
        }
    }
    public void CoolMaterial()
    {
        coolDownHandler = StartCoroutine(CoolMaterialCoroutine());
    }
    public void CoolMaterialPause()
    {
        if(coolDownHandler != null)
        {
            StopCoroutine(coolDownHandler);
        }
    }
    IEnumerator CoolMaterialCoroutine()
    {
        float currentElapsedTime = 0;
        while(currentElapsedTime < timeLeft)
        {
            currentElapsedTime++;
            yield return null;
        }
        //return eveyone back to normal textures
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        int currentIndex = 0;
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.materials = initialMaterials[currentIndex];
            currentIndex++;
        }

        Destroy(this);
    }
}
