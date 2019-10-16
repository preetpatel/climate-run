using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;
    private int secondsToWait = 0;

    public void Begin()
    {
       StartCoroutine(TriggerDialogue());
    }

    public IEnumerator TriggerDialogue()
    {
        yield return new WaitForSeconds(secondsToWait);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
