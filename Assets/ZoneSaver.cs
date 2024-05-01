using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSaver : MonoBehaviour
{
    private ZoneType _zone;
    [SerializeField]private GameObject _zoneGO;
    public void SaveCurrentZone(ZoneType zone)
    {
        _zone = zone;
    }

    public ZoneType GetCurrentZone()
    {
        return _zone;
    }

    public void SaveCurrentZoneGO(GameObject go)
    {
        _zoneGO = go;
    }
    
    public GameObject GetCurrentZoneGo()
    {
        return _zoneGO;
    }
}
