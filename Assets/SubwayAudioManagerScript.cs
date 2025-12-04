using UnityEngine;

public class SubwayAudioManagerScript : MonoBehaviour
{
    public float minDelay = 1f;
    public float maxDelay = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartRandomTimer();
    }

    void StartRandomTimer()
    {
        float randomDelay = Random.Range(minDelay, maxDelay);
        Invoke("PlayAudio", randomDelay);
        //Debug.Log($"Next action in: {randomDelay} seconds.");
    }

    void PlayAudio()
    {
        Debug.Log("Playing Audio");

        StartRandomTimer();
    }
}
