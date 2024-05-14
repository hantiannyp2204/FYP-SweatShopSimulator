using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ZoneType
{
    NONE,
    SHREDDER,
    SMELTER,
    ANVIL,
    FABRICATOR
}

public class ZoneDetector : MonoBehaviour
{
    public RobotAssistant assistant;
    [SerializeField] private GameObject zoneGO;
    [SerializeField] private ZoneType zoneType;
    [SerializeField] private UnityEvent onEnterZone;
    [SerializeField] private UnityEvent onExitZone;


    private GameObject _robotFind;
    private int _playerLayer;

    private ZoneType _currZone;

    private void Start()
    {
        _playerLayer = LayerMask.NameToLayer("Player");
        _robotFind = GameObject.Find("Robot");

        if (_robotFind == null) Debug.Break();

        onEnterZone.AddListener(_robotFind.GetComponent<RobotMovement>().DisableNavMesh);

        onExitZone.AddListener(_robotFind.GetComponent<RobotMovement>().EnableNavMesh);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _playerLayer) return;
        else
        {
            Debug.Log("inzone");
            Debug.Log("CURRENT ZONE:" + _currZone);
            _currZone = zoneType;
            other.gameObject.GetComponentInParent<ZoneSaver>().SaveCurrentZone(_currZone);
            other.gameObject.GetComponentInParent<ZoneSaver>().SaveCurrentZoneGO(zoneGO);
            assistant.JumpToZone();
            assistant.SetIsJumping(true);
            //assistant.GetRobotNavMesh().enabled = false;
            onEnterZone.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != _playerLayer) return;
        else
        {
            //assistant.GetRobotNavMesh().enabled = true;
            assistant.SetIsJumping(false);
            onExitZone.Invoke();
        }
    }

    public ZoneType GetCurrentZone()
    {
        return _currZone;
    }
}
