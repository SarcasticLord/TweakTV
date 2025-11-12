using UnityEngine;

public class BeeChase : MonoBehaviour
{
    public float MaxSpeed;
    private float Speed;

    private Collider[] hitColliders;
    private RaycastHit hit;

    public float SightRange;
    public float DetectionRange;
    public Rigidbody Rigidbody;
    public float FlyHeight;
    public GameObject Target;
    public Animator animator; //optional

    private bool SeePlayer;

    void Start()
    {
        Speed = MaxSpeed;
        if (animator != null)
        {
            animator = GetComponent<Animator>();
            animator.SetBool("IsChasing", false);
        }
    }

    void Update()
    {

        if (SeePlayer && Target != null)
        {
            if (animator != null)
            {
                animator.SetBool("IsChasing", true);
            }
            // Maintain fixed Y position
            Vector3 targetPosition = new Vector3(Target.transform.position.x, Target.transform.position.y + FlyHeight, Target.transform.position.z);
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            // Apply movement
            Vector3 move = moveDirection * Speed;
            Rigidbody.linearVelocity = move;

            // Smooth rotation to face target
            Vector3 lookDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f);
            }
        }
        else
        {
            // Stop movement when not chasing
            Rigidbody.linearVelocity = new Vector3(0, 0, 0);
            if(animator != null)
            {
                animator.SetBool("IsChasing", false );
            }
        }


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
}
