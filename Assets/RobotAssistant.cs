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
    [Header("FEEDBACK")]
    public FeedbackEventData robotMoving;
    public FeedbackEventData robotTalking;

    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem fireBelowFootLeft;
    [SerializeField] private ParticleSystem fireBelowFootRight;
    [SerializeField] private GameObject player;

    public ZoneType DEBUGZONE;
    private Rigidbody _rb;
    private ZoneSaver _zoneSaver;

    private NavMeshAgent _getAgent;

    private Animator _robotAnim;

    [SerializeField] private ROBOT_STATE _currState;

    public Animator GetAnimator()
    {
        return _robotAnim;
    }
    // Start is called before the first frame update
    void Start()
    {
        _getAgent = GetComponent<NavMeshAgent>();

        _rb = GetComponent<Rigidbody>();
        _currState = ROBOT_STATE.PATROL;

        _robotAnim = GetComponentInChildren<Animator>();
        if (_robotAnim == null) return;

        fireBelowFootLeft.Play();
        fireBelowFootRight.Play();
    }

    // Update is called once per frame
    void Update()
    {
        _zoneSaver = player.GetComponent<ZoneSaver>();

        DEBUGZONE = player.GetComponent<ZoneSaver>().GetCurrentZone();
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
