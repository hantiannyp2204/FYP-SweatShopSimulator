using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    public enum ScrapType
    {
        Plastic,
        Wood,
        Metal
    }
    [SerializeField] ScrapType type = ScrapType.Plastic;

    public ScrapType GetScrapType() => type;
   
}
