using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class ClipboardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ClipboardPrefab;
    [SerializeField] private KeyItemSpawnButton KeyItemButton;
    public XRSocketInteractor socket;  // Reference to the socket
    //[SerializeField] private BoxCollider spawnCollider;
    private GameObject currentSpawnedKeyItem;
    private void Start()
    {
        SpawnClipboard();
    }

    private void OnEnable()
    {
        KeyItemButton.OnButtonPressed += SpawnClipboard;
    }

    private void OnDisable()
    {
        KeyItemButton.OnButtonPressed -= SpawnClipboard;
    }

    private void SpawnClipboard()
    {
        //play spawn sound
        if (currentSpawnedKeyItem != null && socket != null)
        {
            Destroy(currentSpawnedKeyItem);
        }

        currentSpawnedKeyItem = Instantiate(ClipboardPrefab, socket.transform.position, socket.transform.rotation);
      
        

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
