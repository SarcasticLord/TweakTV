using UnityEngine;
using System.Collections;

public class BugHitReaction : MonoBehaviour
{
    public float disableDuration = 2f; // Time in seconds to disable the chase
    private BeeHealth beeHealth;
    private Chase chaseScript;
    public GameObject hitParticle;

    void Start()
    {
        chaseScript = GetComponent<Chase>();
        beeHealth = GetComponent<BeeHealth>();
        Debug.Log("");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hitbox")) // Make sure your hitbox GameObject has this tag
        {
            beeHealth.TakeDamage(1);
            SpawnHitEffect();
            Debug.Log("Bug hit by weapon! Disabling chase temporarily.");
            StartCoroutine(Knockdown());
        }
    }

    private void SpawnHitEffect()
    {
        if (hitParticle != null)
        {
            Instantiate(hitParticle, transform.position, Quaternion.identity);
        }
    }


    private IEnumerator Knockdown()
    {
        if (chaseScript != null)
        {
            chaseScript.enabled = false;
            yield return new WaitForSeconds(disableDuration);
            chaseScript.enabled = true;
            Debug.Log("Chase re-enabled.");
        }
    }
}

