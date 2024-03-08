using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData_")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private Vector3 holdingPositionOffset;
    [SerializeField] private Quaternion holdingRotationOffset;
    [SerializeField] private GameObject itemPrefab;
    public string GetName() => itemName;
    public GameObject GetPrefab() => itemPrefab;

    public Vector3 GetPosOffset() => holdingPositionOffset;
    public Quaternion GetRotationOffset() => holdingRotationOffset;
}
