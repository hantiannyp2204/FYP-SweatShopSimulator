using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerEventManager : MonoBehaviour
{
    [SerializeField] private DialogueManager robotDiagManager;
    public List<UnityEvent> PlayerDialogueEvents = new List<UnityEvent>();

    //public void InitDialogueEvents()
    //{
    //    Queue<DialogueLine> dialogueQueue = robotDiagManager.GetDialogueList();

    //    //PlayerDialogueEvents.Clear(); // clear just in case

    //    foreach (var diag in dialogueQueue) // every dialogue will have an event
    //    {
    //        UnityEvent h = new UnityEvent();
    //        PlayerDialogueEvents.Add(h);
    //    }

    //    for (int i = 0; i < dialogueQueue.Count; ++i)
    //    {
    //        PlayerDialogueEvents[i].AddListener(robotDiagManager.SetNextDialogueLine);
    //    }
    //}

    //private void Start()
    //{
    //}
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        PlayerDialogueEvents[0].Invoke();
    //        Debug.Log("coon:" + PlayerDialogueEvents.Count);
    //    }
    //}
}
