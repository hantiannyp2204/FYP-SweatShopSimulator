using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Image xButtonImage;
    [SerializeField] private Image narratorIcon;
    [SerializeField] private TMP_Text dialogueText;

    private Queue<DialogueLine> _lines;

    public bool isDialogueActive = false;

    public float wordSpeed = 0.2f;

    private DialogueLine _queueTracker;

    private bool _isTyping = false;

    // Start is called before the first frame update
    void Start()
    {
        _lines = new Queue<DialogueLine>();

        xButtonImage.gameObject.SetActive(false);

    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        _lines.Clear();
        foreach (DialogueLine lines in dialogue.dialogueLines)
        {
            _lines.Enqueue(lines);
             _queueTracker = _lines.Peek();
            lines.TriggerThisEvent = new UnityEvent();
            lines.TriggerThisEvent.AddListener(SetNextDialogueLine);
            Debug.Log("size of:" + _lines.Count);
        }
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
        if (xButtonImage.gameObject.activeSelf)
        {
            xButtonImage.gameObject.SetActive(false);   
        }
        _isTyping = true;
        dialogueText.text = "";
        foreach (char letter in diagLine.line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        _queueTracker = _lines.Dequeue();
        _isTyping = false; // set to false
        xButtonImage.gameObject.SetActive(true);
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
    }

    public Queue<DialogueLine> GetDialogueList()
    {
        return _lines;
    }
    
    public bool GetIsTyping()
    {
        return _isTyping;
    }

    public DialogueLine GetCurrentIterator()
    {
        return _queueTracker;
    }

    // Add this method to your DialogueManager class
    public DialogueLine PeekNextDialogueLine()
    {
        if (_lines.Count == 0)
            return null; // Queue is empty

        return _lines.Peek();
    }
}
