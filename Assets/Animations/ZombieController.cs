using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private Animator animator;
    private bool isVaulting = false;
    private bool isDead = false;
    private Rigidbody rb;
    
    private NavMeshAgent agent;

    public int health = 2; // Zombie dies after 2 hits

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        rb.isKinematic = true; // Disable physics at the start

        // Randomize the start time of the walking animation
        animator.Play("WalkingZombie", 0, Random.Range(0f, 1f)); // Start at a random point (0% to 100%)
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the zombie collided with a vaultable object
        Debug.Log("Collided with: " + other.name);
        if (other.CompareTag("Vaultable") && !isVaulting && !isDead)
        {
            StartVaulting();
        }
    }

    void StartVaulting()
    {
        isVaulting = true;
        animator.SetBool("IsVaulting", true);
        Debug.Log("Set IsVaulting to true");

        float animationLength = GetAnimationLength("Vault");
        Debug.Log("Vault Animation Length: " + animationLength);

        // Fallback to a fixed duration if the length is 0
        Invoke(nameof(StopVaulting), animationLength > 0 ? animationLength : 1.5f);
    }

    void StopVaulting()
    {
        isVaulting = false;
        animator.SetBool("IsVaulting", false);
    }

    float GetAnimationLength(string animationName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 0f; // Default if animation not found
    }

    public void TakeDamage()
    {
        if (isDead) return;

        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Disable NavMeshAgent to stop movement. Check if the agent is valid before stopping it
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.enabled = false; // Fully disable NavMeshAgent so physics takes over
        }

        // Snap the zombie to the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        {
            transform.position = hit.point; // Move zombie down to the ground
        }


        // Pick a random death animation
        int randomDeathAnim = Random.Range(1, 3); // Generates 1 or 2
        animator.SetTrigger("Die" + randomDeathAnim); 

        // Disable physics to prevent weird air deaths
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        Destroy(gameObject, 5f); // Remove zombie after 5 seconds
    }
    
}
