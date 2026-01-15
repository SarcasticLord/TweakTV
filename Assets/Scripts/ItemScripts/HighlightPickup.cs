
using UnityEngine;

/// <summary>
/// Simple detector that highlights the item under crosshair within range.
/// Put this on the Player and set the LayerMask to your pickup items.
/// </summary>
public class PickupFocusDetector : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float range = 3f;
    [SerializeField] private LayerMask pickupLayer;

    private PickupHighlighter _last;

    private void Reset()
    {
        if (cam == null && Camera.main != null) cam = Camera.main.transform;
    }

    private void Update()
    {
        var ray = (cam != null ? new Ray(cam.position, cam.forward) :
                  new Ray(Camera.main.transform.position, Camera.main.transform.forward));

        if (Physics.Raycast(ray, out var hit, range, pickupLayer))
        {
            var highlighter = hit.collider.GetComponentInParent<PickupHighlighter>();
            if (highlighter != null)
            {
                if (_last != null && _last != highlighter) _last.Apply(false);
                highlighter.Apply(true);
                _last = highlighter;
                return;
            }
        }

        if (_last != null)
        {
            _last.Apply(false);
            _last = null;
        }
    }
}
