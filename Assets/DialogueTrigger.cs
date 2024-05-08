using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueNarrator
{
    public string name;
    public GameObject obj;
}

[System.Serializable] 
public class DialogueLine
{
    public UnityEvent TriggerThisEvent;
    public DialogueNarrator narrator;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>(); 
}

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager diagManager;
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        diagManager.StartDialogue(dialogue);
    }

    private void Start()
    {
        StartCoroutine(DelayedTrigger());
    }


    private IEnumerator DelayedTrigger()
    {
        yield return new WaitForSeconds(0.2f); // Adjust the delay time as needed
        TriggerDialogue();
    }
}
    