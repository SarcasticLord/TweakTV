
using EasyPeasyFirstPersonController;
using UnityEngine;

public class PickupAndThrow : MonoBehaviour
{
    public Transform holdPoint; // Where the object will be held
    public float throwForce = 500f;
    public float pickupAngle = 45f; // Cone angle in degrees
    public float pickupRange = 4f;
    public LayerMask pickupLayer;

    private GameObject heldObject;
    private Rigidbody heldRigidbody;
    private bool isHolding = false;
    public Camera _camera;

    void Update()
    {
        // If the held object was destroyed or missing, reset the hold state.
        if (isHolding && (heldObject == null || heldRigidbody == null))
        {
            ClearHoldState();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isHolding)
            {
                ThrowObject();
            }
            else
            {
                TryPickup();
            }
        }

        // Move/rotate the held object to the hold point (only if valid)
        if (isHolding && heldObject != null && heldRigidbody != null)
        {
            heldObject.transform.position = holdPoint.position;
            heldObject.transform.rotation = holdPoint.rotation;
            // Optionally also zero velocities to prevent jitter
            heldRigidbody.linearVelocity = Vector3.zero;
            heldRigidbody.angularVelocity = Vector3.zero;
        }
    }

    void TryPickup()
    {
        // Get all objects in a sphere
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange, pickupLayer);

        Collider closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            Vector3 directionToHit = (hit.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToHit);

            // Check if within cone angle
            if (angle <= pickupAngle)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = hit;
                }
            }
        }

        if (closest != null)
        {
            Rigidbody rb = closest.attachedRigidbody;
            if (rb != null)
            {
                heldObject = closest.gameObject;
                heldRigidbody = rb;

                // Prepare rigidbody for holding
                heldRigidbody.useGravity = false;
                heldRigidbody.linearVelocity = Vector3.zero;          // Use Rigidbody.velocity (not linearVelocity)
                heldRigidbody.angularVelocity = Vector3.zero;
                heldRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

                isHolding = true;
            }
        }
    }

    void ThrowObject()
    {
        // If the object got destroyed or missing, just clear and return
        if (heldObject == null || heldRigidbody == null)
        {
            ClearHoldState();
            return;
        }

        // Restore physics settings before throwing
        heldRigidbody.useGravity = true;
        heldRigidbody.constraints = RigidbodyConstraints.None;

        Vector3 throwDirection = _camera != null ? _camera.transform.forward : transform.forward;
        heldRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        ClearHoldState();
    }

    /// <summary>
    /// Safely clears the state of the currently held object.
    /// Does not assume the object or rigidbody exists.
    /// </summary>
    private void ClearHoldState()
    {
        // If the rb still exists, ensure it's usable again (gravity on, constraints none).
        if (heldRigidbody != null)
        {
            heldRigidbody.useGravity = true;
            heldRigidbody.constraints = RigidbodyConstraints.None;
        }

        heldObject = null;
        heldRigidbody = null;
        isHolding = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.forward * pickupRange * 0.5f, pickupRange);
    }
}
