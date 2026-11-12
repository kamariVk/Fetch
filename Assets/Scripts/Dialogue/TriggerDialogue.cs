using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueObject dialogue;
    [SerializeField] private string triggerTag = "Dog";
    [SerializeField] private bool onlyOnce = true;

    private bool hasPlayed = false;
  
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Ramte triggeren: " + other.name + " med tag: " + other.tag);
        if (!other.CompareTag(triggerTag)) return;
        PlayDialogue();
    }

    public void PlayDialogue()
    {
        if (onlyOnce && hasPlayed) return;
        hasPlayed = true;
        dialogueUI.ShowDialogue(dialogue);
    }
}
