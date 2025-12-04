
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

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
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

        if (isHolding && heldObject != null)
        {
            // Sync position and rotation of the entire GameObject to holdPoint
            heldObject.transform.position = holdPoint.position;
            heldObject.transform.rotation = holdPoint.rotation;
        }
    }

    //Old Pickup Method
    //void TryPickup()
    //{
    //    Ray ray = new Ray(transform.position, transform.forward);
    //    if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer))
    //    {
    //        GameObject targetObject = hit.collider.gameObject;
    //        Rigidbody rb = targetObject.GetComponent<Rigidbody>();
    //        if (rb != null)
    //        {
    //            heldObject = targetObject;
    //            heldRigidbody = rb;
    //            heldRigidbody.useGravity = false;
    //            heldRigidbody.linearVelocity = Vector3.zero;
    //            heldRigidbody.angularVelocity = Vector3.zero;
    //            heldRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

    //            isHolding = true;
    //        }
    //    }
    //}
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
                heldRigidbody.useGravity = false;
                heldRigidbody.linearVelocity = Vector3.zero;
                heldRigidbody.angularVelocity = Vector3.zero;
                heldRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

                isHolding = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.forward * pickupRange * 0.5f, pickupRange);
    }

    void ThrowObject()
    {
        heldRigidbody.useGravity = true;
        heldRigidbody.constraints = RigidbodyConstraints.None;
        
        Vector3 throwDirection = _camera.transform.forward;
        heldRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);


        heldObject = null;
        heldRigidbody = null;
        isHolding = false;
    }
}