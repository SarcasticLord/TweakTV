
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class RatController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    //[Header("Hit Reaction Settings")]
    //public float disableDuration = 2f; // Time to disable movement
    public GameObject hitParticle;
    //public GameObject unconsciousIndicator; // Prefab to hover above rat
    //public float indicatorHeight = 1.5f;

    private ChatDisplay chat;
    //private EnemyStates movementScript; // Reference to chase script
    //private GameObject indicatorInstance;
    //private Animator animator;

    private EnemyStates ratStates;
    void Start()
    {
        ratStates = gameObject.GetComponent<EnemyStates>();
        currentHealth = maxHealth;
        //movementScript = GetComponent<EnemyStates>(); // Replace with actual chase script type
    }

    // ------------------- HEALTH -------------------
    public void TakeDamage(int damageAmount)
    {
        GameObject chatObject = GameObject.Find("Chat");
        if (chatObject != null)
        {
            chat = chatObject.GetComponent<ChatDisplay>();
            //Chat
        }

        currentHealth -= damageAmount;
        Debug.Log($"Rat took damage: Current Health {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        //else
        //{
        //    ratStates.KnockOut();
        //}
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    // ------------------- HIT REACTION -------------------
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Train"))
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hitbox"))
        {
            TakeDamage(1);
            SpawnHitEffect();
            Debug.Log("Rat hit by weapon! Disabling movement temporarily.");
        }
    }

    private void SpawnHitEffect()
    {
        if (hitParticle != null)
        {
            Instantiate(hitParticle, transform.position, Quaternion.identity);
        }
    }

    // ------------------- KNOCKOUT LOGIC -------------------
    //private void KnockOut()
    //{
    //    if (movementScript != null) movementScript.enabled = false;

    //    Rigidbody rb = gameObject.GetComponent<Rigidbody>();

    //    if (rb != null)
    //    {
    //        rb.useGravity = true;
    //        rb.AddForce(new Vector3(0,45,0) * 7, ForceMode.Impulse);
    //        Debug.Log($"Applied force to {gameObject.name}");
    //    }
    //    // Spawn indicator above rat
    //    if (unconsciousIndicator != null && indicatorInstance == null)
    //    {
    //        indicatorInstance = Instantiate(unconsciousIndicator, transform.position + Vector3.up * indicatorHeight, Quaternion.identity);
    //        indicatorInstance.transform.SetParent(transform); // Follow rat
    //    }

    //    StartCoroutine(RecoverFromKnockOut());
    //}

    //private IEnumerator RecoverFromKnockOut()
    //{
    //    yield return new WaitForSeconds(disableDuration);

    //    if (movementScript != null) movementScript.enabled = true;

    //    Rigidbody rb = gameObject.GetComponent<Rigidbody>();
    //    if (rb != null) rb.useGravity = false;
    //    if (indicatorInstance != null)
    //    {
    //        Destroy(indicatorInstance);
    //    }
    //}

}
