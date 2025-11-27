using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeeState
{
    Idle,
    Chasing,
    KnockedOut
}

public class BeeStates : MonoBehaviour
{
    public float LiftSpeed = 8f;
    public float MaxSpeed;
    public float FlyHeight;
    public float SightRange;
    public float DetectionRange;
    [Header("Flying Pattern")]
    public float SineFrequency = 3f;
    public float SineAmplitude = 0.5f;

    private Rigidbody Rigidbody;
    private GameObject Target;
    private Animator animator;

    private BeeState currentState = BeeState.Idle;
    private float Speed;
    private Collider[] hitColliders;
    private RaycastHit hit;

    void Start()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        Target = GameObject.FindGameObjectWithTag("Player");
        Speed = MaxSpeed;
        if (animator != null)
        {
            animator = GetComponent<Animator>();
            animator.SetBool("IsChasing", false);
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case BeeState.Idle:
                HandleIdle();
                break;
            case BeeState.Chasing:
                HandleChasing();
                break;
            case BeeState.KnockedOut:
                HandleKnockedOut();
                break;
        }

        DetectPlayer();
    }


    private void HandleIdle()
    {
        if (animator != null) animator.SetBool("IsChasing", false);

        // Slowly descend to ground
        Vector3 groundPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, groundPosition, Time.deltaTime);

        Rigidbody.linearVelocity = Vector3.zero;
    }



    private void HandleChasing()
    {
        if (Target == null)
        {
            currentState = BeeState.Idle;
            return;
        }

        if (animator != null) animator.SetBool("IsChasing", true);

        Rigidbody.useGravity = false; // Bee is flying

        float desiredHeight = Target.transform.position.y + FlyHeight;

        // If bee is too low, prioritize rising
        if (transform.position.y < desiredHeight - 0.5f)
        {
            Rigidbody.linearVelocity = Vector3.up * LiftSpeed;
            return;
        }

        // Normal chase with sine wave
        Vector3 targetPosition = new Vector3(Target.transform.position.x, desiredHeight, Target.transform.position.z);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float sineOffset = Mathf.Sin(Time.time * SineFrequency) * SineAmplitude;
        moveDirection.y += sineOffset;

        Rigidbody.linearVelocity = moveDirection * Speed;

        // Smooth rotation
        Vector3 lookDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }




    private void HandleKnockedOut()
    {
        Rigidbody.linearVelocity = Vector3.zero;
        if (animator != null) animator.SetBool("IsChasing", false);
        // Physics handles falling
    }


    public void KnockOut(float duration)
    {
        currentState = BeeState.KnockedOut;
        StartCoroutine(RecoverFromKnockOut(duration));
    }

    private IEnumerator RecoverFromKnockOut(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentState = BeeState.Idle;
    }


    private void DetectPlayer()
    {
        if (currentState == BeeState.KnockedOut) return;

        if (Target == null)
        {
            hitColliders = Physics.OverlapSphere(transform.position, DetectionRange);
            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Player"))
                {
                    Target = collider.gameObject;
                    currentState = BeeState.Chasing;
                    break;
                }
            }
        }
        else
        {
            Vector3 directionToTarget = Target.transform.position - transform.position;
            if (Physics.Raycast(transform.position, directionToTarget.normalized, out hit, SightRange))
            {
                if (!hit.collider.CompareTag("Player"))
                {
                    Target = null;
                    currentState = BeeState.Idle;
                }
            }
        }
    }

}

