
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies a per-item highlight color using Emission via MaterialPropertyBlock.
/// Assign ItemData to read highlightColor.
/// </summary>
[DisallowMultipleComponent]
public class PickupHighlighter : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private ItemData itemData;

    [Header("Material/Shader")]
    [Tooltip("Emission color property for URP Lit / Standard shaders")]
    [SerializeField] private string emissionColorProperty = "_EmissionColor";

    [SerializeField, Range(0f, 10f)]
    private float emissionIntensity = 2f;

    private readonly List<Renderer> _renderers = new();
    private readonly Dictionary<Renderer, MaterialPropertyBlock> _mpbs = new();
    private bool _on;

    private void Awake()
    {
        GetComponentsInChildren(_renderers);
        foreach (var r in _renderers)
        {
            var mpb = new MaterialPropertyBlock();
            r.GetPropertyBlock(mpb);
            _mpbs[r] = mpb;
        }
        Apply(false);
    }

    public void Apply(bool on)
    {
        if (_on == on) return;
        _on = on;

        foreach (var kv in _mpbs)
        {
            var r = kv.Key;
            var mpb = kv.Value;

            if (on && itemData != null)
            {
                var c = itemData.highlightColor * Mathf.LinearToGammaSpace(emissionIntensity);
                mpb.SetColor(emissionColorProperty, c);

                foreach (var mat in r.sharedMaterials)
                    if (mat != null) mat.EnableKeyword("_EMISSION");
            }
            else
            {
                mpb.SetColor(emissionColorProperty, Color.black);
                foreach (var mat in r.sharedMaterials)
                    if (mat != null) mat.DisableKeyword("_EMISSION");
            }

            r.SetPropertyBlock(mpb);
        }
    }
}
