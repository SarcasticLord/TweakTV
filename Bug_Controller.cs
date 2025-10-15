using UnityEngine;

public class Chase : MonoBehaviour
{
    public float MaxSpeed;
    private float Speed;

    private Collider[] hitColliders;
    private RaycastHit hit;

    public float SightRange;
    public float DetectionRange;

    public Rigidbody Rigidbody;
    public GameObject Target;

    private bool SeePlayer;

    void Start()
    {
        Speed = MaxSpeed;
    }

    void Update()
    {
        if (!SeePlayer)
        {
            // Detect player within DetectionRange
            hitColliders = Physics.OverlapSphere(transform.position, DetectionRange);
            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Player"))
                {
                    Target = collider.gameObject;
                    SeePlayer = true;
                    break;
                }
            }
        }
        else
        {
            if (Target == null)
            {
                SeePlayer = false;
                return;
            }

            Vector3 directionToTarget = Target.transform.position - transform.position;

            // Check if player is in sight (line of sight) using Raycast
            if (Physics.Raycast(transform.position, directionToTarget.normalized, out hit, SightRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    // Player is seen, keep chasing
                    // Nothing to do here, just continue chasing
                }
                else
                {
                    // Something else blocking the view -> lose sight
                    SeePlayer = false;
                    Target = null;
                }
            }
            else
            {
                // Raycast didn't hit anything within sight range -> lose sight
                SeePlayer = false;
                Target = null;
            }
        }
    }

    void FixedUpdate()
    {
        if (SeePlayer && Target != null)
        {
            Vector3 direction = (Target.transform.position - transform.position).normalized;

            // Move towards player on the XZ plane only
            Vector3 move = new Vector3(direction.x * Speed, Rigidbody.velocity.y, direction.z * Speed);

            Rigidbody.velocity = move;

            // Rotate to face target smoothly
            Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
            if (lookDirection != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.fixedDeltaTime * 5f);
        }
        else
        {
            // Stop movement when not chasing
            Rigidbody.velocity = new Vector3(0, Rigidbody.velocity.y, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Optional: Reset chasing if you collide with player
        if (collision.gameObject.CompareTag("Player"))
        {
            SeePlayer = false;
            Target = null;
            Rigidbody.velocity = Vector3.zero;
        }
    }
}
