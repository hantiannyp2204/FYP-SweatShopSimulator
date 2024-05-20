using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemSpawn : MonoBehaviour
{
    [SerializeField] private GameObject KeyItemPrefab;
    [SerializeField] private KeyItemSpawnButton KeyItemButton;
    [SerializeField] private BoxCollider spawnCollider;
    private GameObject currentSpawnedKeyItem;
    private void Start()
    {
        SpawnKeyItem();
    }

    private void OnEnable()
    {
        KeyItemButton.OnButtonPressed += SpawnKeyItem;
    }

    private void OnDisable()
    {
        KeyItemButton.OnButtonPressed -= SpawnKeyItem;
    }

    private void SpawnKeyItem()
    {
        //play spawn sound
        if (currentSpawnedKeyItem != null)
        {   
            Destroy(currentSpawnedKeyItem);
        }
        Vector3 randomPosition = GetRandomPositionWithinBounds(spawnCollider.bounds);
        currentSpawnedKeyItem = Instantiate(KeyItemPrefab, randomPosition, Quaternion.identity,this.transform);

        //do dissolve effect
        currentSpawnedKeyItem.GetComponent<KeyItemDissolve>()?.StartReverseDissolve();
    }

    private Vector3 GetRandomPositionWithinBounds(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, y, z);
    }
}
