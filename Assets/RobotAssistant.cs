using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public ZoneType DEBUGZONE;
    private Rigidbody _rb;
    private ZoneSaver _zoneSaver;

    private ROBOT_STATE _currState;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _currState = ROBOT_STATE.PATROL;
    }

    // Update is called once per frame
    void Update()
    {
        _zoneSaver = player.GetComponent<ZoneSaver>();

        DEBUGZONE = player.GetComponent<ZoneSaver>().GetCurrentZone();
    }

    public void JumpToZone()
    {
        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        Vector3 initialPosition = transform.position;
        float jumpStartTime = Time.time;

        while (Time.time - jumpStartTime < jumpDuration)
        {
            float elapsedTime = Time.time - jumpStartTime;
            float t = elapsedTime / jumpDuration;
            float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            Vector3 jumpArc = Vector3.up * yOffset;
            Vector3 targetPosition = _zoneSaver.GetCurrentZoneGo().transform.position;
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
}
