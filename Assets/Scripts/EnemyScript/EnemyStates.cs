using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStates : MonoBehaviour
{
    enum EnemyState { Wander, Chase, Attack, Victory }
    EnemyState currentState;

    public Transform player;
    public PlayerHealth health;
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


    private Rigidbody rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Wander;
        SetNewWanderTarget();
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
                WanderBehavior(distanceToPlayer);
                break;

            case EnemyState.Chase:
                ChaseBehavior(distanceToPlayer);
                break;

            case EnemyState.Attack:
                AttackBehavior(distanceToPlayer);
                break;
            case EnemyState.Victory:
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
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
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


}
