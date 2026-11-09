using UnityEngine;

public class Bark : MonoBehaviour
{
    public AudioClip[] barkSounds;
    public float minWait = 8f;
    public float maxWait = 15f;

    AudioSource source;
    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
        timer = Random.Range(minWait, maxWait);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            source.pitch = Random.Range(0.75f, 0.95f);
            source.PlayOneShot(barkSounds[Random.Range(0, barkSounds.Length)]);
            timer = Random.Range(minWait, maxWait);
        }
    }

}
