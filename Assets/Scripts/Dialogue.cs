using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Followed tutorial from https://github.com/SABentley/Zelda-Dialogue

[RequireComponent(typeof(Text))]
public class Dialogue : MonoBehaviour
{
    private Text textComponent;

    public string[] DialogueStrings;

    public float SecondsBetweenCharacters = 0.1f;
    public float characterSpeedup = 0.1f;

    public KeyCode DialogueInput = KeyCode.Return;

    private bool isStringBeingRevealed = false;
    private bool isEndOfDialogue = false;
    private bool isDialoguePlaying = false;

    public GameObject continueIcon;
    public GameObject stopIcon;

    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponent<Text>();
        textComponent.text = "";

        HideIcons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isDialoguePlaying)
            {
                isDialoguePlaying = true;
                StartCoroutine(StartDialogue());
            }

        }
    }

    private IEnumerator StartDialogue()
    {
        int dialogueLength = DialogueStrings.Length;
        int currentDialogueIndex = 0;

        while (currentDialogueIndex < dialogueLength || !isStringBeingRevealed)
        {
            if (!isStringBeingRevealed)
            {
                isStringBeingRevealed = true;
                StartCoroutine(DisplayString(DialogueStrings[currentDialogueIndex++]));

                if (currentDialogueIndex >= dialogueLength)
                {
                    isEndOfDialogue = true;
                }
            }

            yield return 0;
        }

        while (true)
        {
            if (Input.GetKeyDown(DialogueInput))
            {
                break;
            }

            yield return 0;
        }

        HideIcons();
        isEndOfDialogue = false;
        isDialoguePlaying = false;
    }

    private IEnumerator DisplayString(string stringToDisplay)
    {
        int stringLength = stringToDisplay.Length;
        int index = 0;

        textComponent.text = "";

        while(index < stringLength)
        {
            textComponent.text += stringToDisplay[index];
            index++;
            if (index < stringLength)
            {
                if(Input.GetKey(DialogueInput))
                {
                    yield return new WaitForSeconds(SecondsBetweenCharacters * characterSpeedup);
                } else
                {
                    yield return new WaitForSeconds(SecondsBetweenCharacters);
                }

            } else
            {
                break;
            }
        }

        ShowIcons();

        while(true)
        {
            if(Input.GetKeyDown(DialogueInput))
            {
                break;
            }

            yield return 0;
        }

        HideIcons();

        isStringBeingRevealed = false;
        textComponent.text = "";
    }

    private void HideIcons()
    {
        continueIcon.SetActive(false);
        stopIcon.SetActive(false);
    }

    private void ShowIcons()
    {
        if(isEndOfDialogue)
        {
            stopIcon.SetActive(true);
            return;
        }

        continueIcon.SetActive(true);

    }
}
