using UnityEngine;
using UnityEngine.AI;

public class ZombieFollow : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform target;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

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
    }

    void Update()
    {
        if (target != null)
        {
            // Make the zombie follow the target
            navMeshAgent.SetDestination(target.position);
        }
    }
}
