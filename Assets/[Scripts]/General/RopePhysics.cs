using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePhysics : MonoBehaviour
{
    [Header("Look")]
    [SerializeField] private int numberOfPoints;
    [SerializeField, Min(0.01f)] private float space = 0.3f;
    [SerializeField, Min(0.01f)] private float size = 0.3f;

    [Header("Bahaviour")]
    [SerializeField, Min(1f)] private float springForce = 200;
    [SerializeField, Min(1f)] private float brakeLengthMultiplier = 2f;
    [SerializeField, Min(0.1f)] private float minBrakeTime = 1f;
    //private float brakeLength;
    //private float timeToBrake = 1f;

    [Header("Object to set")]
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject end;

    public List<Transform> pointList = new();
    public List<Transform> connectorList = new();

    private void Start()
    {
        numberOfPoints = pointList.Count;
        //brakeLength = space * numberOfPoints * brakeLengthMultiplier + 2f;

        pointList.Add(start.transform);
        pointList.Add(end.transform);
    }

    private void Update()
    {
        float cableLength = 0f;
        //bool isConnected = startConnector.IsConnected || endConnector.IsConnected;

        int numOfParts = connectorList.Count;
        Transform lastPoint = pointList[0];
        for (int i = 0; i < numOfParts; i++)
        {
            Transform nextPoint = pointList[i + 1];
            Transform connector = connectorList[i].transform;
            connector.position = CountConPos(lastPoint.position, nextPoint.position);
            if (lastPoint.position == nextPoint.position || nextPoint.position == connector.position)
            {
                connector.localScale = Vector3.zero;
            }
            else
            {
                connector.rotation = Quaternion.LookRotation(nextPoint.position - connector.position);
                connector.localScale = CountSizeOfCon(lastPoint.position, nextPoint.position);
            }

            //if (isConnected)
            //    cableLength += (lastPoint.position - nextPoint.position).magnitude;

            lastPoint = nextPoint;
        }

        //if (isConnected)
        //{
        //    if (cableLength > brakeLength)
        //    {
        //        timeToBrake -= Time.deltaTime;
        //        if (timeToBrake < 0f)
        //        {
        //            startConnector.Disconnect();
        //            endConnector.Disconnect();
        //            timeToBrake = minBrakeTime;
        //        }
        //    }
        //    else
        //    {
        //        timeToBrake = minBrakeTime;
        //    }
        //}
    }
    private Vector3 CountConPos(Vector3 start, Vector3 end) => (start + end) / 2f;
    private Vector3 CountSizeOfCon(Vector3 start, Vector3 end) => new Vector3(size, size, (start - end).magnitude / 2f);
}
