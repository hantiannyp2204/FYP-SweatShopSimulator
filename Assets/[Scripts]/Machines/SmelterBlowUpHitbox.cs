using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmelterBlowUpHitbox : MonoBehaviour
{
    [SerializeField] MachineSmelter smelter;
    int currentHitParticle = 0;
    [SerializeField] private bool _isFixed = false;

    public bool IsSmelterFixed()
    {
        return _isFixed;
    }

    private void OnParticleTrigger()
    {
        if (!smelter.HasBlownUp()) return;
        Debug.Log("HIT");
        Debug.Log("PARTICLE: " + currentHitParticle);
        currentHitParticle++;
        if(currentHitParticle >= smelter.GetHealthPoints())
        {
            smelter.FixBlowUp();
            currentHitParticle= 0;
        }
    }

    public void SetFixedBool(bool status)
    {
        _isFixed = status;
    }
}
