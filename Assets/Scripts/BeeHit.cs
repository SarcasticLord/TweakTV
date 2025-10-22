using UnityEngine;
using System.Collections;

public class BugCollision : MonoBehaviour
{
    public float disableDuration = 2f; // Time in seconds to disable the chase
    public float attackDuration = 1f;
    private BeeHealth beeHealth;
    private BeeChase chaseScript;
    public GameObject beeAttack;
    public GameObject hitParticle;
    private Transform self;
    public float offset;

    void Start()
    {
        chaseScript = GetComponent<BeeChase>();
        beeHealth = GetComponent<BeeHealth>();
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
            // You can disable the chase script here if needed
            // GetComponent<YourChaseScript>().enabled = false;
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
        Vector3 spawnPosition = self.transform.position + self.transform.forward * offset;
        GameObject spawnedHitbox = GameObject.Instantiate(beeAttack, spawnPosition, Quaternion.identity);
        spawnedHitbox.transform.SetParent(self.transform, true);
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
}

