using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float writingSpeed = 50f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private float minPitch = 0.7f;
    [SerializeField] private float maxPitch = 0.95f;
    [SerializeField] private int charsPerSound = 2;
    public Coroutine Run(string textToType, TMP_Text dialogueText)
    {
        return StartCoroutine(routine: TypeText(textToType, dialogueText));
    }

    private IEnumerator TypeText(string textToType, TMP_Text dialogueText)
    {

        float t = 0;
        int charIndex = 0;
        int lastCharIndex = 0;

        while (charIndex < textToType.Length)
        {

        
            t += Time.deltaTime * writingSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            dialogueText.text = textToType.Substring(0, charIndex);

            if (charIndex > lastCharIndex && textToType[charIndex - 1] != ' ' && charIndex % charsPerSound == 0)
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.Stop();
                audioSource.clip = typingSound;
                audioSource.Play();
            }
            lastCharIndex = charIndex;
             
            yield return null;
        }

        dialogueText.text = textToType;
        audioSource.Stop();
    }
}
