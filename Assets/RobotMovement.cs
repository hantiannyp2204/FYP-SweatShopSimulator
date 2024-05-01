using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotMovement : MonoBehaviour
{
    [SerializeField] private Transform moveArea;
    [SerializeField] private float rangeToFindPoint;

    private NavMeshAgent _refAgent;
    // Start is called before the first frame update
    void Start()
    {
        _refAgent = GetComponent<NavMeshAgent>();        
    }

    // Update is called once per frame
    void Update()
    {
        SetNextDestinaton();
    }

    bool GetRandomPoint(Vector3 center, float range, out Vector3 output)
    {
        Vector3 point = center + Random.insideUnitSphere * range; // get point in sphere
        NavMeshHit meshhit;
        if (NavMesh.SamplePosition(point, out meshhit, 1.0f, NavMesh.AllAreas))
        {
            output = meshhit.position; // out put holds final random point in sphere
            return true;
        }
        output = Vector3.zero;
        return false;
    }

    void SetNextDestinaton()
    {
        if (!_refAgent.enabled) return;
        if (_refAgent.remainingDistance <= _refAgent.stoppingDistance && _refAgent.GetComponent<RobotAssistant>().GetCurrState() != ROBOT_STATE.DELIVERING) // check if agent
        {
            Vector3 point;
            if (GetRandomPoint(moveArea.position, rangeToFindPoint, out point))
            {
                _refAgent.SetDestination(point);
            }
        }
    }

    public void EnableNavMesh()
    {
        _refAgent.enabled = true;
    }

    public void DisableNavMesh()
    {
        _refAgent.enabled = false;
    }

    public void SetNewWaypoint(GameObject dest)
    {
        _refAgent.transform.GetComponent<RobotAssistant>().SetState(ROBOT_STATE.DELIVERING);
        _refAgent.SetDestination(dest.transform.position);
    }
}
