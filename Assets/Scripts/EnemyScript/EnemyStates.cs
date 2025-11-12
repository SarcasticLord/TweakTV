using UnityEngine;
using UnityEngine.AI;

public class EnemyStates : MonoBehaviour
{
    enum EnemyState { Wander, Chase, Attack }
    EnemyState currentState;

    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float wanderRadius = 5f;
    public float idleTime = 2f;

    public float wanderSpeed;
    public float chaseSpeed;
    public float attackSpeed; // Standing still

    private NavMeshAgent agent;
    private Vector3 wanderTarget;
    private float idleTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Wander;
        SetNewWanderTarget();
    }

    void Update()
    {
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

        if (distanceToPlayer > attackRange)
        {
            currentState = EnemyState.Chase;
        }
        Debug.Log("Attacking Player");
        // For now, just stand still (attack animation can go here later)
    }

    void SetNewWanderTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        wanderTarget = hit.position;
        agent.SetDestination(wanderTarget);
    }

}
