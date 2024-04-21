using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmelterBlowUpHitbox : MonoBehaviour
{
    [SerializeField] MachineSmelter smelter;
    int currentHitParticle = 0;
    private void OnParticleTrigger()
    {
        
        if (!smelter.HasBlownUp()) return;
        Debug.Log("HIT");
        currentHitParticle++;
        if(currentHitParticle >= smelter.GetHealthPoints())
        {
            smelter.FixBlowUp();
            currentHitParticle= 0;
        }
    }
}
