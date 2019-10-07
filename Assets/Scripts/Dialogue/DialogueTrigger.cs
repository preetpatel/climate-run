using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Used to call the DialogueManager and start the dialgoue 
 **/
public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;
    private int secondsToWait = 3;

    public void Start()
    {
        // use a coroutine to wait for a few seconds before calling the dialogue starter
       StartCoroutine(TriggerDialogue());
    }

    public IEnumerator TriggerDialogue()
    {
        yield return new WaitForSeconds(secondsToWait);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
