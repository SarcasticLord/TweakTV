using UnityEngine;

public class MasterAudioController : MonoBehaviour
{
    public AudioClip[] sounds;              // your 5–6 random sounds
    public AudioSource[] speakerSources;    // all speakers in the station

    public float interval = 10f;            // time between random plays
    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            PlayRandomToAllSpeakers();
            timer = interval;
        }
    }

    void PlayRandomToAllSpeakers()
    {
        // Pick random clip
        AudioClip selected = sounds[Random.Range(0, sounds.Length)];

        // Assign to each speaker
        foreach (AudioSource s in speakerSources)
        {
            s.clip = selected;
        }

        // Sync playback
        foreach (AudioSource s in speakerSources)
        {
            s.Play();
        }
    }
}

