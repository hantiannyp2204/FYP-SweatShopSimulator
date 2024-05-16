using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum ROBOT_STATE
{
    DELIVERING,
    PATROL, 
}

public class RobotAssistant : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpDuration;
    [SerializeField] private RobotItemPlate itemPlate;

    public ZoneType DEBUGZONE;
    //public LookAtPlayerBillboard lookAtMe;
    private Rigidbody _rb;
    private ZoneSaver _zoneSaver;

    private NavMeshAgent _getAgent;

    public bool _isJumping = false;

    private Animator _robotAnim;
    private RobotMovement _robotMovement;
    private RobotAssistant _assistant;
    private Vector3 _currentTarget;

    [SerializeField] private ROBOT_STATE _currState;

    public Animator GetAnimator()
    {
        return _robotAnim;
    }

    public bool GetIsJumping()
    {
        return _isJumping;
    }

    public void SetIsJumping(bool status)
    {
        _isJumping = status;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _getAgent = GetComponent<NavMeshAgent>();
        //if (_getAgent != null)
        //{
        //  //  _getAgent.baseOffset = 2;
        //}

        _rb = GetComponent<Rigidbody>();
        _currState = ROBOT_STATE.PATROL;

        _robotAnim = GetComponentInChildren<Animator>();
        if (_robotAnim == null) return;

        _robotMovement = GetComponent<RobotMovement>();

        _assistant = GetComponent<RobotAssistant>();

        //lookAtMe.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        _zoneSaver = player.GetComponent<ZoneSaver>();

        DEBUGZONE = player.GetComponent<ZoneSaver>().GetCurrentZone();

        if (_assistant.GetIsJumping())
        {
            LookAtWho(player);
        }
        else
        {
            if (_getAgent.hasPath)
            {
                // get the position of the next corner or waypoint along the path
                Vector3 nextCorner = _getAgent.steeringTarget;

                // ensure the robot is facing towards the next corner
                LookAtWhoV3(nextCorner);
            }
        }


        if (GetIsJumping())
        {
            if (_robotMovement.DirectCheckIsReachedDestination(_currentTarget)) // reached target
            {
                itemPlate.GetCollider().enabled = true;
                _robotAnim.SetBool("isHandOut", true);
            }
        }
        else
        {
            if (_robotAnim.GetBool("isHandOut")) // only disable when robot has hand out animation played
            {
                _robotAnim.SetBool("isHandOut", false); 
            }
        }
    }

    public void LookAtWho(GameObject go)
    {
        transform.LookAt(go.transform.position, Vector3.up);
    }

    public void LookAtWhoV3(Vector3 dir)
    {
        // Create a rotation that looks along the direction vector
        Quaternion rotation = Quaternion.LookRotation(dir);

        // Apply the rotation to the robot
        transform.rotation = rotation;
    }

    public void JumpToZone()
    {
        if (!GetIsJumping())
        {
            if (_zoneSaver.GetCurrentZoneGo() != null)
            {
                StartCoroutine(JumpCoroutine());
            }
            //_robotAnim.SetBool("isJumping", false);
        }
    }

    private IEnumerator JumpCoroutine()
    {
        //_robotAnim.SetBool("isJumping", true);

        Vector3 initialPosition = transform.position;
        float jumpStartTime = Time.time;

        while (Time.time - jumpStartTime < jumpDuration)
        {
            float elapsedTime = Time.time - jumpStartTime;
            float t = elapsedTime / jumpDuration;
            float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            Vector3 jumpArc = Vector3.up * yOffset;
            //Vector3 targetPosition = _zoneSaver.GetCurrentZoneGo().transform.position;
            Vector3 targetPosition = _zoneSaver.GetCurrentZoneGo().GetComponent<Collider>().bounds.center;
            _currentTarget = targetPosition;
            _rb.MovePosition(Vector3.Lerp(initialPosition, targetPosition + jumpArc, t));
            yield return null; // Wait for the next frame
        }
    }

    public void SetState(ROBOT_STATE state)
    {
        _currState = state;
    }

    public ROBOT_STATE GetCurrState()
    {
        return _currState;
    }

    public GameObject GetRobotGameobject()
    {
        return gameObject;
    }

    public NavMeshAgent GetRobotNavMesh()
    {
        return _getAgent;
    }
}
