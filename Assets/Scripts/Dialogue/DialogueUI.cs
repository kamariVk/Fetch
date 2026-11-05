using System.Collections;
using UnityEngine;
using TMPro;
using NUnit.Framework.Constraints;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguebox;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private DialogueObject testDialogue;

    private TypewriterEffect typewriterEffect;

    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        CloseDialogueBox();
        ShowDialogue(testDialogue);
    }
    public void ShowDialogue(DialogueObject dialogueObject)
    {
        dialoguebox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return typewriterEffect.Run(dialogue, dialogueText);
            yield return new WaitForSeconds(1f);
        }

        CloseDialogueBox();
    }

    private void CloseDialogueBox()
    {
        dialoguebox.SetActive(false);
        dialogueText.text = string.Empty;
    }
}