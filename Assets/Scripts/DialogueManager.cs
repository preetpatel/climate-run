using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject ContinueIcon;
    public Animator DialogueAnimator;
    public Animator backgroundAnimator;

    private Queue<string> Sentences;
    private bool isSentenceShowing = false;
   

    // Start is called before the first frame update
    void Start()
    {
        Sentences = new Queue<string>();
    }

    private void Update()
    {
        if(Input.anyKeyDown && !isSentenceShowing)
        {
            isSentenceShowing = true;
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        DialogueAnimator.SetBool("isOpen", true);
        backgroundAnimator.SetBool("isOpen", true);
        nameText.text = dialogue.name;

        Sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            Sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        ContinueIcon.SetActive(false);
        if (Sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = Sentences.Dequeue();
        dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        
        foreach ( char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            // wait a single frame
            yield return null;
        }
        isSentenceShowing = false;
        ContinueIcon.SetActive(true);
    }

    void EndDialogue()
    {
        DialogueAnimator.SetBool("isOpen", false);
        backgroundAnimator.SetBool("isOpen", false);
    }
   
}
