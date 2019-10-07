using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * Class is used to hold the animators for the dialogue, the
 * name of the speaker and their dialogue.
 **/
public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject ContinueIcon;
    public Animator DialogueAnimator;
    public Animator backgroundAnimator;
    public Animator CharacterAnimator;
    public string nextScene;

    private Queue<string> Sentences;
    private bool isSentenceShowing = false;
    private int secondsToWaitAtEnd = 2;
   

    // Start is called before the first frame update
    void Start()
    {
        // create a queue for the sentences
        Sentences = new Queue<string>();
    }

    private void Update()
    {
        // used for the player to show the next sentence when they tap on the screen
        if(Input.anyKeyDown && !isSentenceShowing)
        {
            isSentenceShowing = true;
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        // animate the dialogue, background and character to appear
        DialogueAnimator.SetBool("isOpen", true);
        backgroundAnimator.SetBool("isOpen", true);
        CharacterAnimator.SetBool("isOpen", true);
        nameText.text = dialogue.name;

        // clear any queue that may be left over from other dialogue
        Sentences.Clear();

        // add the sentences needed into the queue
        foreach (string sentence in dialogue.sentences)
        {
            Sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        ContinueIcon.SetActive(false);
        if (Sentences.Count == 0) // This checks that the queue is empty
        {
            // end the dialogue if true
            StartCoroutine(EndDialogue());
            return;
        }

        // Type out the sentence on the screen
        string sentence = Sentences.Dequeue();
        dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        
        // used to type out the sentence letter by letter
        foreach ( char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            // wait a single frame
            yield return null;
        }
        isSentenceShowing = false;
        ContinueIcon.SetActive(true);
    }

    IEnumerator EndDialogue()
    {
        // trigger the closing animations
        DialogueAnimator.SetBool("isOpen", false);
        backgroundAnimator.SetBool("isOpen", false);
        CharacterAnimator.SetBool("isOpen", false);
        yield return new WaitForSeconds(secondsToWaitAtEnd);
        SceneManager.LoadScene(nextScene);
    }

}
