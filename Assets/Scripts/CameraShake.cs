using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform train;
    public float maxShakeDistance = 30f;
    public float maxShakeIntensity = 0.5f;
    public float shakeFrequency = 25f;

    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, train.position);
        if (distance < maxShakeDistance)
        {
            float proximityFactor = 1f - (distance / maxShakeDistance);
            float intensity = proximityFactor * maxShakeIntensity;


            float shakeX = (Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) - 0.5f) * 2f;
            float shakeY = (Mathf.PerlinNoise(0f, Time.time * shakeFrequency) - 0.5f) * 2f;


            Vector3 shakeOffset = new Vector3(shakeX, shakeY, 0f) * intensity;
            transform.localPosition = originalPos + shakeOffset;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

}
