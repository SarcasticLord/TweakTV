
using System.Collections;
using UnityEngine;

/// <summary>
/// Place this on a GameObject with a BoxCollider (isTrigger=true) that covers your room.
/// When the player enters, global fog is disabled (optionally blended to zero).
/// When the player exits, original fog settings are restored.
/// Works with URP when shaders are compiled with fog support.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class FogDisableZone : MonoBehaviour
{
    [Header("Detection")]
    [Tooltip("Layers that will trigger fog toggle (e.g., Player).")]
    public LayerMask triggerLayers = ~0;

    [Header("Blend")]
    [Tooltip("Blend duration when entering/exiting the zone.")]
    public float blendSeconds = 0.5f;

    // Cache original fog settings
    private bool originalFogEnabled;
    private Color originalFogColor;
    private float originalFogDensity;
    private float originalFogStart;
    private float originalFogEnd;
    private FogMode originalFogMode;

    private int occupants = 0;         // support multiple colliders (e.g., player + camera)
    private Coroutine blendRoutine;

    void Awake()
    {
        // Record original fog settings once at start
        originalFogEnabled = RenderSettings.fog;
        originalFogColor = RenderSettings.fogColor;
        originalFogMode = RenderSettings.fogMode;
        originalFogDensity = RenderSettings.fogDensity;
        originalFogStart = RenderSettings.fogStartDistance;
        originalFogEnd = RenderSettings.fogEndDistance;

        // Ensure collider is a trigger
        var box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & triggerLayers) == 0) return;
        occupants++;
        if (occupants == 1)
        {
            // First entrant: disable fog
            StartBlendDisableFog();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & triggerLayers) == 0) return;
        occupants = Mathf.Max(0, occupants - 1);
        if (occupants == 0)
        {
            // Last occupant left: restore fog
            StartBlendRestoreFog();
        }
    }

    private void StartBlendDisableFog()
    {
        if (blendRoutine != null) StopCoroutine(blendRoutine);
        blendRoutine = StartCoroutine(BlendFogToDisabled(blendSeconds));
    }

    private void StartBlendRestoreFog()
    {
        if (blendRoutine != null) StopCoroutine(blendRoutine);
        blendRoutine = StartCoroutine(BlendFogToOriginal(blendSeconds));
    }

    private IEnumerator BlendFogToDisabled(float seconds)
    {
        float t = 0f;

        bool startEnabled = RenderSettings.fog;
        Color startColor = RenderSettings.fogColor;
        FogMode startMode = RenderSettings.fogMode;
        float startDensity = RenderSettings.fogDensity;
        float startStartDist = RenderSettings.fogStartDistance;
        float startEndDist = RenderSettings.fogEndDistance;

        // If you want a hard cut, set seconds=0. Otherwise blend density/end distance to visually fade out.
        if (seconds <= 0f)
        {
            RenderSettings.fog = false;
            yield break;
        }

        // For linear mode, push end distance far; for exp modes, fade density to 0.
        while (t < seconds)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / seconds);

            if (startMode == FogMode.Linear)
            {
                // Blend end distance up and start distance down so fog thins out
                RenderSettings.fogStartDistance = Mathf.Lerp(startStartDist, 0f, k);
                RenderSettings.fogEndDistance = Mathf.Lerp(startEndDist, 100000f, k);
            }
            else
            {
                RenderSettings.fogDensity = Mathf.Lerp(startDensity, 0f, k);
            }
            yield return null;
        }

        RenderSettings.fog = false;
    }

    private IEnumerator BlendFogToOriginal(float seconds)
    {
        float t = 0f;

        // Re-enable fog at start so we can blend back
        RenderSettings.fog = true;
        RenderSettings.fogColor = originalFogColor;
        RenderSettings.fogMode = originalFogMode;

        // Starting points
        float startDensity = RenderSettings.fogDensity;
        float startStartDist = RenderSettings.fogStartDistance;
        float startEndDist = RenderSettings.fogEndDistance;

        if (seconds <= 0f)
        {
            RestoreOriginalFogInstant();
            yield break;
        }

        while (t < seconds)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / seconds);

            if (originalFogMode == FogMode.Linear)
            {
                RenderSettings.fogStartDistance = Mathf.Lerp(startStartDist, originalFogStart, k);
                RenderSettings.fogEndDistance = Mathf.Lerp(startEndDist, originalFogEnd, k);
            }
            else
            {
                RenderSettings.fogDensity = Mathf.Lerp(startDensity, originalFogDensity, k);
            }
            yield return null;
        }

        RestoreOriginalFogInstant();
    }

    private void RestoreOriginalFogInstant()
    {
        RenderSettings.fog = originalFogEnabled;
        RenderSettings.fogColor = originalFogColor;
        RenderSettings.fogMode = originalFogMode;
        RenderSettings.fogDensity = originalFogDensity;
        RenderSettings.fogStartDistance = originalFogStart;
        RenderSettings.fogEndDistance = originalFogEnd;
    }
}
