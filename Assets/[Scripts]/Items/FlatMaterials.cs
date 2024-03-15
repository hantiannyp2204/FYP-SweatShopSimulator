using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatMaterials : MonoBehaviour
{
    public enum FlatMaterialType
    {
        Plastic,
        Wood,
        Metal
    }
    [SerializeField] FlatMaterialType type;

    public FlatMaterialType GetMaterialType() => type;
}
