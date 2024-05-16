using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> robotWaypoints;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform moveArea;
    [SerializeField] private float rangeToFindPoint;
    [SerializeField] private ZoneSaver zoneSaver;
    [SerializeField] private float patrolRadius;
    [SerializeField] private float patrolSpeed;

    private NavMeshAgent _refAgent;     
    private RobotAssistant _assistant;

    [SerializeField] private int _waypointIndex = 0; 

    // Start is called before the first frame update
    void Start()
    {
        _refAgent = GetComponent<NavMeshAgent>();
        _assistant = GetComponent<RobotAssistant>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleWaypointMovement();

        if (_refAgent.GetComponent<RobotAssistant>().GetCurrState() == ROBOT_STATE.PATROL)
        {
            _assistant.GetAnimator().SetBool("isMoving", true);
        }
        else
        {
            _assistant.GetAnimator().SetBool("isMoving", false);
        }
        {
            //if (_refAgent.GetComponent<RobotAssistant>().GetCurrState() == ROBOT_STATE.DELIVERING)
            //{
            //    if (CheckIsReachedDestination(_itemPlateRef.machineDestination))
            //    {
            //        _itemPlateRef.box.SetInsertedItem(_itemPlateRef.GetRobotHoldingItem().transform.gameObject);
            //        Debug.Log("item:ahah" + _itemPlateRef.GetRobotHoldingItem().gameObject.name);
            //        _itemPlateRef.table.ToggleOrder(false);

            //        _refAgent.GetComponent<RobotAssistant>().SetState(ROBOT_STATE.PATROL);
            //    }
            //    else
            //    {
            //        return;
            //    }
            //} 
            //else
            //{
            //    if (!_assistant.GetIsJumping())
            //    {
            //         SetNextDestinaton();
            //    }
            //}
        }
    }

    void HandleWaypointMovement()
    {
        if (robotWaypoints.Count == 0) return;

        float distToWaypoint = Vector3.Distance(robotWaypoints[_waypointIndex].position, transform.position);

        if (distToWaypoint <= 1f)
        {
            _waypointIndex = (_waypointIndex + 1) % robotWaypoints.Count;
        }

        _refAgent.SetDestination(robotWaypoints[_waypointIndex].position);
    }

    public void EnableNavMesh()
    {
        if (_refAgent != null)
        {
            _refAgent.enabled = true;
        }
    }

    public void DisableNavMesh()
    {
        if (_refAgent != null)
        {
            _refAgent.enabled = false;
        }
    }
}
