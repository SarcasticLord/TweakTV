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

    void Start()
    {
        chaseScript = GetComponent<BeeChase>();
        beeHealth = GetComponent<BeeHealth>();
        rb = GetComponent<Rigidbody>();
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
            Attack(attackDuration);
        }
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
        GameObject spawnedHitbox = GameObject.Instantiate(beeAttack, spawnPosition, Quaternion.identity);
        Cooldown(cooldown);


        //Vector3 spawnPosition = new Vector3(0f,0f,0f);
        //GameObject spawnedHitbox = GameObject.Instantiate(data.hitbox, spawnPosition, Quaternion.identity);
        //spawnedHitbox.transform.SetParent(worldObject.transform, true);
        //Debug.Log($"Spawned hitbox for {data.itemName} at {spawnPosition}");

        //WeaponHitbox hitboxScript = spawnedHitbox.GetComponent<WeaponHitbox>();
        //if (hitboxScript != null)
        //{
        //    hitboxScript.forceDirection = user.transform.forward;
        //}

        //GameObject.Destroy(spawnedHitbox, 0.2f);
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
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Attack(attackDuration);
            Debug.Log("Continuing Attack");
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

