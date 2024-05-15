using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform moveArea;
    [SerializeField] private float rangeToFindPoint;

    private NavMeshAgent _refAgent;

    private RobotItemPlate _itemPlateRef;
    // Start is called before the first frame update
    void Start()
    {
        _refAgent = GetComponent<NavMeshAgent>();

        _itemPlateRef = _refAgent.GetComponentInChildren<RobotItemPlate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetNewWaypoint(_itemPlateRef.machineDestination);
        }


        if (_refAgent.GetComponent<RobotAssistant>().GetCurrState() == ROBOT_STATE.DELIVERING)
        {
            if (CheckIsReachedDestination(_itemPlateRef.machineDestination))
            {
                _itemPlateRef.box.SetInsertedItem(_itemPlateRef.GetRobotHoldingItem().transform.gameObject);
                Debug.Log("item:ahah" + _itemPlateRef.GetRobotHoldingItem().gameObject.name);
                _itemPlateRef.table.ToggleOrder();

                _refAgent.GetComponent<RobotAssistant>().SetState(ROBOT_STATE.PATROL);
            }
            else
            {
                return;
            }
        } 
        else
        {
            SetNextDestinaton();
        }
    }

    bool GetRandomPoint(Vector3 center, float range, out Vector3 output)
    {
        Vector3 point = center + Random.insideUnitSphere * range; // get point in sphere
        NavMeshHit meshhit;
        if (NavMesh.SamplePosition(point, out meshhit, 1.0f, NavMesh.AllAreas))
        {
            output = meshhit.position; // output holds final random point in sphere
            return true;
        }
        output = Vector3.zero;
        return false;
    }

    void SetNextDestinaton() // Patrolling Logic
    {
        if (_refAgent.remainingDistance <= _refAgent.stoppingDistance) // check if agent
        {
            Vector3 point;
            if (GetRandomPoint(moveArea.position, rangeToFindPoint, out point))
            {
                _refAgent.SetDestination(point);
            }
        }
        Vector3 dir = _refAgent.steeringTarget - transform.position; // get distance from destination and robot
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
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

    public void SetNewWaypoint(GameObject dest)
    {
        if (!_refAgent.enabled)
        {
            _refAgent.enabled = true;
        }
        _refAgent.transform.GetComponent<RobotAssistant>().SetState(ROBOT_STATE.DELIVERING);
        _refAgent.SetDestination(dest.transform.position);
    }

    public bool CheckIsReachedDestination(GameObject dest)
    {
        Debug.Log("distance:" + Vector3.Distance(transform.position, dest.transform.position));
        return Vector3.Distance(transform.position, dest.transform.position) < 0.5f;
    }
}
