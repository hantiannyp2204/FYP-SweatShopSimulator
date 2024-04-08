using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] TMP_Text objectiveTxt;
    public void Init()
    {
        ResetObjective();
    }
    public void UpdateObjetcive(string newObjective)
    {
        objectiveTxt.text = "Objective:\n" + newObjective;
    }
    public void ResetObjective()
    {
        objectiveTxt.text = "Objective:\nNone";
    }
}
