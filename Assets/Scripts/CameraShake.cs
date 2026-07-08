using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Train Camera Shake")]
    public Transform train;
    public float maxShakeDistance = 30f;
    public float maxShakeIntensity = 0.5f;
    public float shakeFrequency = 25f;
    private Vector3 _originalPos;


    [Header("Called Camera Shake")]
    public float defaultDuration = 0.5f;
    public float defaultIntensity = 0.3f;  // Max offset magnitude
    public float defaultFrequency = 20f;   // How fast the noise evolves
    public bool useUnscaledTime = false;   // True for pause-resistant shake

    [Tooltip("Controls how intensity changes over time (0..1). Left is start, right is end.")]
    public AnimationCurve intensityOverTime = AnimationCurve.EaseInOut(0, 1, 1, 0);

    // Optional: also shake rotation (roll/pitch)

    private Coroutine _runningShake;


    void Start()
    {
        _originalPos = transform.localPosition;
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
            transform.localPosition = _originalPos + shakeOffset;
        }
        else
        {
            transform.localPosition = _originalPos;
        }
    }

    void Awake()
    {
        _originalPos = transform.localPosition;
    }

    /// <summary>
    /// Starts a timed camera shake. If a previous shake is running, it will be stopped.
    /// </summary>
    public void StartShake(
        float duration,
        float intensity,
        float frequency,
        AnimationCurve curve = null
    )
    {
        if (_runningShake != null)
        {
            StopCoroutine(_runningShake);
            ResetTransform();
        }
        _runningShake = StartCoroutine(ShakeRoutine(duration, intensity, frequency, curve ?? intensityOverTime));
    }

    /// <summary>
    /// Convenience overload using the default parameters.
    /// </summary>
    public void StartShake()
    {
        StartShake(defaultDuration, defaultIntensity, defaultFrequency, intensityOverTime);
    }

    private IEnumerator ShakeRoutine(
        float duration,
        float intensity,
        float frequency,
        AnimationCurve curve
    )
    {
        float t = 0f;
        // Randomize noise phase so repeated shakes don’t look identical
        float seedX = Random.value * 1000f;
        float seedY = Random.value * 1000f;

        while (t < duration)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;

            float normalized = Mathf.Clamp01(t / duration);
            float envelope = curve.Evaluate(normalized);   // 0..1
            float currentIntensity = intensity * envelope;

            // Perlin-based 2D shake centered around 0
            float timeFactor = (useUnscaledTime ? Time.unscaledTime : Time.time) * frequency;
            float shakeX = (Mathf.PerlinNoise(timeFactor + seedX, 0f) - 0.5f) * 2f;
            float shakeY = (Mathf.PerlinNoise(0f, timeFactor + seedY) - 0.5f) * 2f;

            Vector3 offset = new Vector3(shakeX, shakeY, 0f) * currentIntensity;
            transform.localPosition = _originalPos + offset;

            yield return null;
        }

        ResetTransform();
        _runningShake = null;
    }

    private void ResetTransform()
    {
        transform.localPosition = _originalPos;
    }

    /// <summary>
    /// Stop an ongoing shake immediately and reset the camera.
    /// </summary>
    public void StopShake()
    {
        if (_runningShake != null)
        {
            StopCoroutine(_runningShake);
            _runningShake = null;
        }
        ResetTransform();
    }

}
