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
        TriggerDialogue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Queue<DialogueLine> spare = diagManager.GetDialogueList();
            var list = spare.ToArray();
            list[0].TriggerThisEvent.Invoke();
        }
    }
}
