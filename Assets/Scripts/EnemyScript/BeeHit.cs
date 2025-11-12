
using UnityEngine;
using System.Collections;

public class BugCollision : MonoBehaviour
{
    public float disableDuration = 5f; // Time to disable chase after being hit
    public float attackDuration = 1f;  // Duration of attack animation
    public float cooldownTime = 2f;    // Time between attacks

    private BeeHealth beeHealth;
    private BeeChase chaseScript;
    public GameObject beeAttack;
    public GameObject hitParticle;
    private Transform self;
    private Rigidbody rb;
    public float offset;

    //private bool isAttacking = false;
    private bool canAttack = true;

    void Start()
    {
        chaseScript = GetComponent<BeeChase>();
        beeHealth = GetComponent<BeeHealth>();
        rb = GetComponent<Rigidbody>();
        self = GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hitbox"))
        {
            beeHealth.BeeTakeDamage(1);
            SpawnHitEffect();
            Debug.Log("Bug hit by weapon! Disabling chase temporarily.");
            StartCoroutine(Cooldown(disableDuration));
        }
        else if (other.CompareTag("Player") && canAttack)
        {
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = false;
            Debug.Log("Player is within attack range!");
            chaseScript.enabled = false;
            Attack();
            StartCoroutine(Cooldown(disableDuration));
            canAttack = false;
        }
    }

    private void SpawnHitEffect()
    {
        if (hitParticle != null)
        {
            Instantiate(hitParticle, transform.position, Quaternion.identity);
        }
    }

    public void Attack()
    {
        if (canAttack)
        {
            Instantiate(beeAttack, self);
        }
    }

    private IEnumerator Cooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        canAttack = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //isAttacking = false;
            chaseScript.enabled = true;
            rb.useGravity = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Optional: You can use this to keep track of proximity
        if (other.CompareTag("Player"))
        {
            // Do nothing here unless you want to trigger something continuously
        }
    }
}
