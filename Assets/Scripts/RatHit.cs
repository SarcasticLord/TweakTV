using UnityEngine;
using System.Collections;

public class RatHitReaction : MonoBehaviour
{
    public float disableDuration = 2f; // Time in seconds to disable the chase
    private RatHealth ratHealth;
    private RatChase chaseScript;
    public GameObject hitParticle;

    void Start()
    {
        chaseScript = GetComponent<RatChase>();
        ratHealth = GetComponent<RatHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hitbox")) // Make sure your hitbox GameObject has this tag
        {
            ratHealth.TakeDamage(1);
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

