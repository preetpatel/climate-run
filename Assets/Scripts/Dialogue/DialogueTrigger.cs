using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;
    private int secondsToWait = 3;

    public void Start()
    {
       StartCoroutine(TriggerDialogue());
    }

    public IEnumerator TriggerDialogue()
    {
        yield return new WaitForSeconds(secondsToWait);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
