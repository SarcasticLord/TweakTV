using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStates : MonoBehaviour
{
    public enum EnemyState { Wander, Chase, Attack, Victory, KnockedOut }
    EnemyState currentState;

    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    public GameObject hitParticle;
    public int knockOutChance;


    private Transform player;
    private PlayerHealth health;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float wanderRadius = 5f;
    public float idleTime = 2f;

    public float wanderSpeed;
    public float chaseSpeed;
    public float attackSpeed; // Standing still
    public GameObject hitbox;
    public float offset = 2;
    public float attackCooldown = 3f;

    private Animator animator;
    private NavMeshAgent agent;
    private Vector3 wanderTarget;
    private float idleTimer;
    private bool canAttack = true;

    public float knockOutDuration = 3f;
    public GameObject unconsciousIndicator;
    private GameObject indicatorInstance;

    private ChatDisplay chat;

    private AudioSource audioSource;



    private Rigidbody rb;

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Wander;
        SetNewWanderTarget();
        Debug.Log($"Starting Health: {currentHealth}");
    }

    void Update()
    {
        if (health.playerIsDead && currentState != EnemyState.Victory)
        {
            VictoryAnimation();
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Wander:
                //audioSource.Play();
                WanderBehavior(distanceToPlayer);
                break;

            case EnemyState.Chase:
                //audioSource.Play();
                ChaseBehavior(distanceToPlayer);
                break;

            case EnemyState.Attack:
                //audioSource.Play();
                AttackBehavior(distanceToPlayer);
                break;

            case EnemyState.Victory:
                //audioSource.Stop();
                break;

            case EnemyState.KnockedOut:
                //audioSource.Stop();
                KnockOut();
                break;

        }
    }

    void WanderBehavior(float distanceToPlayer)
    {
        agent.speed = wanderSpeed;
        if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Chase;
            return;
        }
        Debug.Log("Wandering");

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Debug.Log("Rat is idle...");
            animator.SetBool("IsChasing", false);
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTime)
            {
                SetNewWanderTarget();
                idleTimer = 0f;
            }
        }
    }

    void ChaseBehavior(float distanceToPlayer)
    {
        agent.speed = chaseSpeed;
        animator.SetBool("IsChasing", true);
        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attack;
            agent.ResetPath();
            return;
        }
        else if (distanceToPlayer > detectionRange)
        {
            currentState = EnemyState.Wander;
            SetNewWanderTarget();
            return;
        }
        Debug.Log("Chasing Player");
        agent.SetDestination(player.position);
    }

    void AttackBehavior(float distanceToPlayer)
    {
        agent.speed = attackSpeed; // Enemy stands still
        rb.linearVelocity = Vector3.zero;

        if (distanceToPlayer > attackRange)
        {
            currentState = EnemyState.Chase;
        }
        Debug.Log("Attacking Player");
        // For now, just stand still (attack animation can go here later)
        if (canAttack)
        {
            canAttack = false;
            SpawnHitbox();
            StartCoroutine(ResetAttackCooldown());
        }

    }

    void SetNewWanderTarget()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        wanderTarget = hit.position;
        agent.SetDestination(wanderTarget);
        animator.SetBool("IsChasing", true);
    }

    void VictoryAnimation()
    {
        currentState = EnemyState.Victory;
        agent.ResetPath(); // Stop movement
        rb.linearVelocity = Vector3.zero; // Ensure no physics movement
        animator.SetTrigger("PlayerDeath"); // Trigger victory animation
        Debug.Log("Victory! Rat is dancing!");
    }


    void SpawnHitbox()
    {
        Vector3 spawnPosition = gameObject.transform.position + gameObject.transform.forward * offset;
        GameObject spawnedHitbox = GameObject.Instantiate(hitbox, spawnPosition, Quaternion.identity);
        spawnedHitbox.transform.SetParent(gameObject.transform, true);
        GameObject.Destroy(spawnedHitbox, 2f);
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }


    public void KnockOut()
    {
        // Disable NavMeshAgent
        agent.isStopped = true;
        audioSource.Stop();

        //// Optional: Apply force for dramatic effect
        //rb.AddForce(new Vector3(0, 5, 0) * 1f, ForceMode.Impulse);

        // Spawn indicator
        if (unconsciousIndicator != null && indicatorInstance == null)
        {
            indicatorInstance = Instantiate(unconsciousIndicator, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            indicatorInstance.transform.SetParent(transform);
        }

        StartCoroutine(RecoverFromKnockOut());
    }

    IEnumerator RecoverFromKnockOut()
    {
        yield return new WaitForSeconds(knockOutDuration);

        // Remove indicator
        if (indicatorInstance != null) Destroy(indicatorInstance);

        // Restore NavMeshAgent
        agent.enabled = true;
        agent.isStopped = false;

        currentState = EnemyState.Wander; // Or Chase if player is near
    }

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
        else
        {
            int knockOut = UnityEngine.Random.Range(0, knockOutChance+1);
            Debug.Log($"KnockOut Value: {knockOut}");
            if(knockOut == 0)
            {
                currentState = EnemyState.KnockedOut;
            }
        }
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
            Destroy(other);
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
}
