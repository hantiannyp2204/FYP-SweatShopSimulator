using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testdialogue : MonoBehaviour
{
    public DialogueManager dialogue;
    private int player;

    private void Start()
    {
        player = LayerMask.NameToLayer("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == player)
        {
            Queue<DialogueLine> spare = dialogue.GetDialogueList();
            var list = spare.ToArray();

            if (list[0] != null)
            {
                list[0].TriggerThisEvent.Invoke();
            }
        }
    }
}
