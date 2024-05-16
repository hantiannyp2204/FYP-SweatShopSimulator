using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Inspired by Raqib
[CreateAssetMenu(fileName = "ItemData_")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [SerializeField] private Vector3 holdingPositionOffset;
    [SerializeField] private Quaternion holdingRotationOffset;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Vector3 requestBoxPositionOffset;
    [SerializeField] private Quaternion requestBoxRotationOffset;
    [SerializeField] private float timeGiven;
    [SerializeField] private float scoreGiven;

    public float GetTimeGiven() => timeGiven;
    public float GetScoreGiven() => scoreGiven;
    public string GetName() => itemName;
    public GameObject GetPrefab() => itemPrefab;

    public Vector3 GetPosOffset() => holdingPositionOffset;
    public Quaternion GetRotationOffset() => holdingRotationOffset;
    public Quaternion GetRequestBoxRotationOffset() => requestBoxRotationOffset;

    public Vector3 GetRequestBoxPositionOffset() => requestBoxPositionOffset;

    [Header("Product Content")]
    public List<ItemData> productContainable; // If applicable

    [Header("Material Change")]
    public Material mat; // if applicable

    [Header("Relevant Robot Display Image")]
    public Sprite imageToDisplay;
}
