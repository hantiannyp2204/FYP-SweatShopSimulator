using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inspired by Raqib
[CreateAssetMenu(fileName = "ItemData_")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [SerializeField] private Vector3 holdingPositionOffset;
    [SerializeField] private Quaternion holdingRotationOffset;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Quaternion requestBoxRotationOffset;

    public string GetName() => itemName;
    public GameObject GetPrefab() => itemPrefab;

    public Vector3 GetPosOffset() => holdingPositionOffset;
    public Quaternion GetRotationOffset() => holdingRotationOffset;
    public Quaternion GetRequestBoxRotationOffset() => requestBoxRotationOffset;

    [Header("Product Content")]
    public List<ItemData> productContainable; // If applicable

    
}
