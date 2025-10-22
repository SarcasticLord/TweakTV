using UnityEngine;
using System.Collections;

public class RatHitReaction : MonoBehaviour
{
    public float disableDuration = 2f; // Time in seconds to disable the chase
    public float attackDuration = 1f;
    private RatHealth ratHealth;
    private RatChase chaseScript;
    public GameObject hitParticle;
    public GameObject hitbox;
    private Transform self;
    private Rigidbody rb;
    public float offset;
    public bool isAttacking = false;

    void Start()
    {
        chaseScript = GetComponent<RatChase>();
        ratHealth = GetComponent<RatHealth>();
        rb = GetComponent<Rigidbody>();
        self = GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hitbox")) // Make sure your hitbox GameObject has this tag
        {
            ratHealth.TakeDamage(1);
            SpawnHitEffect();
            Debug.Log("Bug hit by weapon! Disabling chase temporarily.");
            StartCoroutine(Cooldown(disableDuration));
        }
        else if (other.CompareTag("Player") && !isAttacking)
        {
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = false;
            Debug.Log("Player is within attack range!");
            chaseScript.enabled = false;
            isAttacking = true;
            StartCoroutine(Attack());
        }
    }
    private void SpawnHitEffect()
    {
        if (hitParticle != null)
        {
            Instantiate(hitParticle, transform.position, Quaternion.identity);
        }
    }
    public IEnumerator Attack()
    {
        rb.linearVelocity = Vector3.zero;
        rb.useGravity = false;

        Debug.Log("Attacking");
        Vector3 spawnPosition = self.position + self.forward * offset;
        GameObject ratHitbox = Instantiate(hitbox, spawnPosition, Quaternion.identity);
        ratHitbox.transform.SetParent(self, true);
        Destroy(ratHitbox, 0.5f);
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
        Debug.Log("Attack Finished");
    }
    private IEnumerator Cooldown(float duration)
    {
        if (chaseScript != null)
        {
            chaseScript.enabled = false;
            yield return new WaitForSeconds(duration);
            chaseScript.enabled = true;
            Debug.Log("Chase re-enabled.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAttacking = false;
            chaseScript.enabled = true;
            rb.useGravity = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAttacking = true;
        }
    }
}

