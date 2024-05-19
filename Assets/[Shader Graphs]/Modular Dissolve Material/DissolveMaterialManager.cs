using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveMaterialManager : MonoBehaviour
{
    // Shader
    public Material dissolveMaterial;
    [SerializeField] private float timeToDissolve = 1f; // Adjustable time to dissolve in seconds

    private List<Material> dissolveMaterialList = new List<Material>(); // List to track all new material instances made by applying
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>(); // Dictionary to store original materials
    private float cutoffHeight = 0f;
    private float currentCutoffValue = 1;
    private float completeDissolveValue = -1f;
    private float completeExistanceValue = 1f;
    private float lerpTimer = 0f;
    private Coroutine DissolveCoroutineHandler;

    // To completely stop dissolve and set the item back to original state
    public void ResetDissolve()
    {
        PauseDissolve();
        lerpTimer = 0;
        currentCutoffValue = completeExistanceValue;
        foreach (Material mat in dissolveMaterialList)
        {
            mat.SetFloat("_DissolvePercentage", currentCutoffValue);
        }
        RevertMaterials(); // Ensure materials are reverted to the original state
    }

    // To pause the current dissolve
    public void PauseDissolve()
    {
        if (DissolveCoroutineHandler != null)
        {
            StopCoroutine(DissolveCoroutineHandler);
            DissolveCoroutineHandler = null;
        }
        else
        {
            Debug.Log("No coroutine running");
        }
    }

    public void StartDissolve()
    {
        if (DissolveCoroutineHandler != null)
        {
            StopCoroutine(DissolveCoroutineHandler);
        }
        DissolveCoroutineHandler = StartCoroutine(DissolveCoroutine(true));
    }

    public void StartReverseDissolve()
    {
        if (DissolveCoroutineHandler != null)
        {
            StopCoroutine(DissolveCoroutineHandler);
        }
        DissolveCoroutineHandler = StartCoroutine(DissolveCoroutine(false));
    }

    public virtual void StartOfDissolve()
    {
        SwitchMaterialsToDissolve(dissolveMaterial);
    }

    public virtual void EndOfDissolve()
    {
    }

    IEnumerator DissolveCoroutine(bool dissolveIntoExistance)
    {
        StartOfDissolve();
        lerpTimer = 0; // Reset the timer at the start of the coroutine

        float startValue = dissolveIntoExistance ? completeDissolveValue : completeExistanceValue;
        float endValue = dissolveIntoExistance ? completeExistanceValue : completeDissolveValue;

        currentCutoffValue = startValue;

        while (lerpTimer < timeToDissolve)
        {
            cutoffHeight = Mathf.Lerp(startValue, endValue, lerpTimer / timeToDissolve);
            foreach (Material mat in dissolveMaterialList)
            {
                mat.SetFloat("_DissolvePercentage", cutoffHeight); // Update each material's dissolve percentage
            }

            lerpTimer += Time.deltaTime;
            yield return null;
        }

        // Ensure it is set to the target value to reduce anomaly
        foreach (Material mat in dissolveMaterialList)
        {
            mat.SetFloat("_DissolvePercentage", endValue);
        }

        EndOfDissolve();
        //only reset the material if it is dissovling into existance

        ///NOTE: Might breack if EndOfDissolve() contains Destroying the script
        ///Make sure the script don't get deleted until RevertMaterials() is triggered
        ///Meaning gameobject should not Destroy(this) until the gameobject is done dissovling into existance
        if (!dissolveIntoExistance)
        {
            RevertMaterials();
        }
    }

    private void SwitchMaterialsToDissolve(Material dissolveMaterialTemplate)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;

            // Store the original materials
            originalMaterials[renderer] = (Material[])materials.Clone();

            for (int i = 0; i < materials.Length; i++)
            {
                Material newMaterial = new Material(dissolveMaterialTemplate);
                // Setting the dissolve material's base, normal, metallic, AO, and smoothness according to the source texture
                CopyMaterialProperties(materials[i], newMaterial);
                materials[i] = newMaterial;

                dissolveMaterialList.Add(newMaterial);
            }

            renderer.materials = materials;
        }
    }

    private void CopyMaterialProperties(Material sourceMaterial, Material targetMaterial)
    {
        // Transfer base (albedo) texture
        if (sourceMaterial.HasProperty("_MainTex") && sourceMaterial.GetTexture("_MainTex") != null)
        {
            targetMaterial.SetTexture("_BaseTex", sourceMaterial.GetTexture("_MainTex"));
        }
        else
        {
            targetMaterial.SetTexture("_BaseTex", null);
        }
        // Transfer normal map
        if (sourceMaterial.HasProperty("_BumpMap") && sourceMaterial.GetTexture("_BumpMap") != null)
        {
            targetMaterial.SetTexture("_NormalTex", sourceMaterial.GetTexture("_BumpMap"));
        }
        else
        {
            targetMaterial.SetTexture("_NormalTex", null);
        }
        // Transfer metallic map
        if (sourceMaterial.HasProperty("_MetallicGlossMap") && sourceMaterial.GetTexture("_MetallicGlossMap") != null)
        {
            targetMaterial.SetTexture("_MetallicTex", sourceMaterial.GetTexture("_MetallicGlossMap"));
        }
        else
        {
            targetMaterial.SetTexture("_MetallicTex", null);
        }
        // Transfer occlusion map
        if (sourceMaterial.HasProperty("_OcclusionMap") && sourceMaterial.GetTexture("_OcclusionMap") != null)
        {
            targetMaterial.SetTexture("_AOTex", sourceMaterial.GetTexture("_OcclusionMap"));
        }
        else
        {
            targetMaterial.SetTexture("_AOTex", null);
        }
        // Transfer smoothness
        if (sourceMaterial.HasProperty("_Smoothness"))
        {
            targetMaterial.SetFloat("_Smoothness", sourceMaterial.GetFloat("_Smoothness"));
        }
        // Transfer base color (albedo tint)
        if (sourceMaterial.HasProperty("_Color"))
        {
            targetMaterial.SetColor("_Base_Map_RGB", sourceMaterial.GetColor("_Color"));
        }
        else
        {
            targetMaterial.SetColor("_Base_Map_RGB", Color.white);
        }
    }

    public void RevertMaterials()
    {
        foreach (var entry in originalMaterials)
        {
            Renderer renderer = entry.Key;
            Material[] originalMats = entry.Value;
            renderer.materials = originalMats;
        }

        dissolveMaterialList.Clear();
        originalMaterials.Clear();
    }
}
