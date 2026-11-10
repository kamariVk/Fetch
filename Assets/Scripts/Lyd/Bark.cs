using UnityEngine;

public class Bark : MonoBehaviour
{
    public AudioClip[] barkSounds;
    public float minWait = 8f;
    public float maxWait = 20f;
    public float minPitch = 0.75f;
    public float maxPitch = 0.95f;

    AudioSource source;
    float timer;

    void Start()
    {
        source = GetComponent<AudioSource>();
        timer = Random.Range(minWait, maxWait);
    }

    void Update()
    {
        if (barkSounds.Length == 0) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            source.pitch = Random.Range(minPitch, maxPitch);
            source.panStereo = Random.Range(-0.8f, 0.8f);
            float volume = Random.Range(0.4f, 1f);
            source.PlayOneShot(barkSounds[Random.Range(0, barkSounds.Length)], volume);

            timer = Random.Range(minWait, maxWait);
        }
    }
}