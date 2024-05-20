using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        currentHitParticle++;
        if(currentHitParticle >= smelter.GetHealthPoints())
        {
            _isFixed = true;
            smelter.FixBlowUp();
            currentHitParticle= 0;
        }
    }
}
