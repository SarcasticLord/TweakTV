using UnityEngine;

public class TrainBehaviour : MonoBehaviour
{
    public Vector3 direction = Vector3.forward; // Change to Vector3.right, Vector3.up, etc.
    public float speed = 5f;
    public float hitForce = 1000f;
    public Vector3 forceDirection;
    private Rigidbody rb;
    public Transform startPoint;
    public Transform startPoint2;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }


    void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
        forceDirection = direction.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction.normalized * speed * Time.fixedDeltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Endpoint1"))
        {
            // Move to start point and rotate 180 degrees
            transform.position = startPoint.position;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 180, 0));
            Debug.Log("Train teleported to start point 1 and rotated.");
            return;
        }
        if (other.CompareTag("Endpoint2"))
        {
            // Move to start point and rotate 180 degrees
            transform.position = startPoint2.position;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 180, 0));
            Debug.Log("Train teleported to start point 2 and rotated.");
            return;
        }

        Rigidbody rb = other.attachedRigidbody;

        if (rb != null)
        {
            rb.AddForce(forceDirection * hitForce, ForceMode.Impulse);
            Debug.Log($"Applied force to {other.name}");
        }
        // Check if the collided object has the EnemyHealth component


    }
}
