using UnityEngine;
using System.Collections;

public class BugCollision : MonoBehaviour
{
    public float disableDuration = 5f; // Time in seconds to disable the chase
    public float attackDuration = 1f;
    private BeeHealth beeHealth;
    private BeeChase chaseScript;
    public GameObject beeAttack;
    public GameObject hitParticle;
    private Transform self;
    private Rigidbody rb;
    public float offset;
    public bool isAttacking;

    void Start()
    {
        chaseScript = GetComponent<BeeChase>();
        beeHealth = GetComponent<BeeHealth>();
        rb = GetComponent<Rigidbody>();
        self = GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hitbox")) // Make sure your hitbox GameObject has this tag
        {
            beeHealth.BeeTakeDamage(1);
            SpawnHitEffect();
            Debug.Log("Bug hit by weapon! Disabling chase temporarily.");
            StartCoroutine(Cooldown(disableDuration));
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Player is within attack range!");
            chaseScript.enabled = false;
            isAttacking = true;
            Attack(attackDuration);
        
    }


    private void SpawnHitEffect()
    {
        if (hitParticle != null)
        {
            Instantiate(hitParticle, transform.position, Quaternion.identity);
        }
    }
    public void Attack(float cooldown)
    {
        rb.linearVelocity = new Vector3(0, 0, 0);
        rb.useGravity = false;
        Debug.Log("Attacking");
        Vector3 spawnPosition = self.transform.position + self.transform.forward * offset;
        GameObject beeHitbox = Instantiate(beeAttack);
        Debug.Log("Spawned Hitbox");
        beeHitbox.transform.position = spawnPosition;
        beeHitbox.transform.SetParent(self.transform, true);
        Destroy(beeHitbox, 0.5f);
        StartCoroutine(Cooldown(cooldown));
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
            chaseScript.enabled = true;
            rb.useGravity = true;
        }
    }
}

