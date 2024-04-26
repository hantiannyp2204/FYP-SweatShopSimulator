using UnityEngine;

public class ProbabilityManager : MonoBehaviour
{
    [Range(1, 6)]
    [SerializeField] private int probabilityOfEvent;

    public bool TryLuck()
    {
        int randValue = Random.Range(1, 6);
        Debug.Log("value: " + randValue);   
        if (randValue < probabilityOfEvent)
        {
            // lucky innit
            return true;
        }
        return false;
    }
}
