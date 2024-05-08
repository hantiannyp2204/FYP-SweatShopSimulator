using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private PlayerEventManager eventManager;
    [SerializeField] private Image narratorIcon;
    [SerializeField] private TMP_Text dialogueText;

    private Queue<DialogueLine> _lines;

    public bool isDialogueActive = false;

    public float wordSpeed = 0.2f;

    private DialogueLine _firstElement;


    private DialogueLine _queueTracker;

    // Start is called before the first frame update
    void Start()
    {
        _lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        _lines.Clear();
        foreach (DialogueLine lines in dialogue.dialogueLines)
        {
            _lines.Enqueue(lines);
            lines.TriggerThisEvent = new UnityEvent();
            lines.TriggerThisEvent.AddListener(SetNextDialogueLine);
            Debug.Log("size of:" + _lines.Count);
        }


        _firstElement = _lines.Peek();

       // eventManager.InitDialogueEvents();
    }

    public void SetNextDialogueLine()
    {
        if (_lines.Count <= 0)
        {
            EndDialogue();
            return;
        }

        _queueTracker = _lines.Peek();

        StopAllCoroutines();

        StartCoroutine(TypeDialogue(_queueTracker));
    }

    IEnumerator TypeDialogue(DialogueLine diagLine)
    {
        dialogueText.text = "";
        foreach (char letter in diagLine.line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        _queueTracker = _lines.Dequeue();
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
    }

    public Queue<DialogueLine> GetDialogueList()
    {
        return _lines;
    }
}
