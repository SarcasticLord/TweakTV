using UnityEngine;
using System.Collections;

public class RatHitReaction : MonoBehaviour
{
    public float disableDuration = 2f; // Time in seconds to disable the chase
    public float attackDuration = 1f;
    private RatHealth ratHealth;
    public GameObject hitParticle;
    public float offset;

    void Start()
    {
        ratHealth = GetComponent<RatHealth>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Train"))
        {
            ratHealth.TakeDamage(1);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hitbox") || other.CompareTag("Train")) // Make sure your hitbox GameObject has this tag
        {
            ratHealth.TakeDamage(1);
            SpawnHitEffect();
            Debug.Log("Bug hit by weapon! Disabling chase temporarily.");
        }
    }
    private void SpawnHitEffect()
    {
        if (hitParticle != null)
        {
            Instantiate(hitParticle, transform.position, Quaternion.identity);
        }
    }
}

