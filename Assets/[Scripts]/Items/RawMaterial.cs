using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawMaterial : MonoBehaviour
{
    public enum RawMaterialType
    {
        Plastic,
        Wood,
        Metal
    }
    [SerializeField] public RawMaterialType type;

    public RawMaterialType GetRawMaterialType() => type;
}
