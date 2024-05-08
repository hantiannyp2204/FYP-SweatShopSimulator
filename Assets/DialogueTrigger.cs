using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.AI;

[System.Serializable]
public class DialogueNarrator
{
    public string name; // name of the narrator talking
    public GameObject obj;
}

[System.Serializable] 
public class DialogueLine
{
    [Header("Possible Pathfind Location after dialogue")]
    public GameObject pathFindDestination; // make robot pathfind if applicable

    [Header("Events for each line")]
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
    public InputActionReference toggleNextDialogue = null;

    [SerializeField] private DialogueManager diagManager;
    public Dialogue dialogue;

    private NavMeshAgent _robotNavMesh;
    private void Awake()
    {
        toggleNextDialogue.action.started += PerformNextDialogue;
    }

    private void OnDestroy()
    {
        toggleNextDialogue.action.started -= PerformNextDialogue;
    }

    private void Start()
    {
        _robotNavMesh = GetComponent<NavMeshAgent>();
        if (_robotNavMesh == null) 
        {
            Debug.Break();
            return; // null check js in case
        }


        StartCoroutine(DelayedTrigger());
    }

    public void TriggerDialogue()
    {
        diagManager.StartDialogue(dialogue);
    }

    private IEnumerator DelayedTrigger()
    {
        yield return new WaitForSeconds(0.2f); // Adjust the delay time as needed
        TriggerDialogue();
    }

    private void PerformNextDialogue(InputAction.CallbackContext context)
    {
        if (diagManager.GetIsTyping()) return;
        DialogueLine temp = diagManager.GetDialogueList().Dequeue();
        DialogueLine peekedElement = diagManager.GetDialogueList().Peek();
        diagManager.GetDialogueList().Enqueue(temp);

        // check if it has  a destination to go to 
        if (diagManager.GetCurrentIterator().pathFindDestination != null) // means object needs to move somewhere
        {
            _robotNavMesh.SetDestination(diagManager.GetCurrentIterator().pathFindDestination.transform.position);
            // pathfind to the destination and set next dialogue only after finished
            if (!_robotNavMesh.pathPending)
            {
                if (_robotNavMesh.remainingDistance <= _robotNavMesh.stoppingDistance)
                {
                    if  (!_robotNavMesh.hasPath || _robotNavMesh.velocity.sqrMagnitude == 0)
                    {
                        // done
                    }
                }
            }
        }
        diagManager.SetNextDialogueLine();
    }
}
    