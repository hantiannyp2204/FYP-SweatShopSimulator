using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    [SerializeField] GameObject rawMaterial;
    public GameObject GetMaterial() => rawMaterial;
}
