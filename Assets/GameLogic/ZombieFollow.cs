using UnityEngine;
using UnityEngine.AI;

public class ZombieFollow : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform target;
    private Animator animator;
    
    public float attackDistance = 1.5f; // Distance at which the zombie will attack
    public float attackCooldown = 2f; // Time between attacks
    private float lastAttackTime;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Ensure the NavMeshAgent is enabled
        if (!navMeshAgent.enabled)
        {
            Debug.LogWarning("NavMeshAgent is disabled on " + gameObject.name);
            return;
        }

        // Find the player (or target)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Player not found. Make sure it is tagged as 'Player'.");
        }

        // Ensure the zombie is on a valid NavMesh
        if (!navMeshAgent.isOnNavMesh)
        {
            Debug.LogError("Zombie is not placed on a valid NavMesh! Disabling NavMeshAgent.", gameObject);
            navMeshAgent.enabled = false; // Prevent further errors
        }
    }

    void Update()
    {
        if (target != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            if (distanceToPlayer > attackDistance)
            {
                // Continue chasing the player
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(target.position);
                animator.SetBool("IsAttacking", false);
            }
            else
            {
                // Stop moving and attack
                navMeshAgent.isStopped = true;
                TryAttack();
            }
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown) // Ensure attack cooldown
        {
            lastAttackTime = Time.time;
            animator.SetBool("IsAttacking", true); // Triggers attack animation
        }
    }

    // 👇 This function will be called at the right moment in the attack animation
    public void DealDamage()
    {
        if (target != null)
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage();
            }
        }
    }
}
