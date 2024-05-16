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
    private Collider _collider;

    private void Start()
    {
        _playerLayer = LayerMask.NameToLayer("Player");
        _robotFind = GameObject.Find("Robot");

        if (_robotFind == null) Debug.Break();

        onEnterZone.AddListener(_robotFind.GetComponent<RobotMovement>().DisableNavMesh);

        onExitZone.AddListener(_robotFind.GetComponent<RobotMovement>().EnableNavMesh);

        _collider = GetComponent<Collider>();
        if (_collider == null) return;
    }


    private void Update()
    {
        if (assistant.GetCurrState() == ROBOT_STATE.DELIVERING)
        {
            _collider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _playerLayer) return;
        else
        {
            //if (!assistant.lookAtMe.enabled)
            //{
            //    assistant.lookAtMe.enabled = true;
            //}
            Debug.Log("inzone");
            Debug.Log("CURRENT ZONE:" + _currZone);
            _currZone = zoneType;
            assistant.JumpToZone();
            other.gameObject.GetComponentInParent<ZoneSaver>().SaveCurrentZone(_currZone);
            other.gameObject.GetComponentInParent<ZoneSaver>().SaveCurrentZoneGO(zoneGO);
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
            //if (assistant.lookAtMe.enabled)
            //{
            //    assistant.lookAtMe.enabled = false; 
            //}
            //assistant.GetRobotNavMesh().enabled = true;
            assistant.SetState(ROBOT_STATE.PATROL);
            assistant.SetIsJumping(false);
            _currZone = ZoneType.NONE;
            onExitZone.Invoke();    
        }
    }

    public ZoneType GetCurrentZone()
    {
        return _currZone;
    }
}
