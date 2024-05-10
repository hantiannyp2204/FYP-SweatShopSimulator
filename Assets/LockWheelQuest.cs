using UnityEngine;

public class LockWheelQuest : GenericQuest
{
    [SerializeField] private SmelterWheel wheel;

    // Update is called once per frame
    void Update()
    {
        if (wheel.GetTurnStatus())
        {
            Destroy(gameObject);
        }
    }
}
