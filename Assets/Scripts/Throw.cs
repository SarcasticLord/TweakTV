using UnityEngine;

public class PickupAndThrow : MonoBehaviour
{
    public Transform holdPoint; // Where the object will be held
    public float throwForce = 500f;
    public float pickupRange = 3f;
    public LayerMask pickupLayer;

    private GameObject heldObject;
    private Rigidbody heldRigidbody;
    private bool isHolding = false;
    public Camera _camera;

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

    void TryPickup()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer))
        {
            GameObject targetObject = hit.collider.gameObject;
            Rigidbody rb = targetObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                heldObject = targetObject;
                heldRigidbody = rb;
                heldRigidbody.useGravity = false;
                heldRigidbody.linearVelocity = Vector3.zero;
                heldRigidbody.angularVelocity = Vector3.zero;
                heldRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

                isHolding = true;
            }
        }
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