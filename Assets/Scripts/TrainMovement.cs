using UnityEngine;

public class TrainBehaviour : MonoBehaviour
{
    public Vector3 direction = Vector3.forward; // Change to Vector3.right, Vector3.up, etc.
    public float speed = 5f;
    public float hitForce = 1000f;
    public Vector3 forceDirection;
    private Rigidbody rb;


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

        Rigidbody rb = other.attachedRigidbody;

        if (rb != null)
        {
            rb.AddForce(forceDirection * hitForce, ForceMode.Impulse);
            Debug.Log($"Applied force to {other.name}");
        }
        // Check if the collided object has the EnemyHealth component


    }
}
