using System.Collections;
using UnityEngine;

public class TimedDialogue : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueObject dialogue;
    [SerializeField] private float delay = 2.5f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        dialogueUI.ShowDialogue(dialogue);
    }
}