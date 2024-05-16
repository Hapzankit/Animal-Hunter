using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Animals : MonoBehaviour
{
    [Header("Wander")]
    public float wanderDistance = 50f;
    public float walkSpeed = 5f;
    public float maxWalkTime = 6f;

    [Header("Idle")]
    public float idleTime = 5f;

    public NavMeshAgent navMeshAgent;
    public AnimalState currentState = AnimalState.Idle;

    public Animator animator;

    public float health = 90;
    public string animalName;

    public bool isAlive;
    public abstract void InitializeAnimal();

    public abstract void UpdateState();

    public abstract void HandleIdleState();

    public abstract void HandleWalkState();

    public abstract void HandleRunState();

    public abstract void SetState(AnimalState newState);

    public abstract void OnStateChanged(AnimalState newState);

    public abstract void HandleAttackState();
    public abstract void HandleWoundedState();

    public GameObject player;
    public virtual Vector3 GetRandomNavMeshPosition(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navMeshHit;

        if (NavMesh.SamplePosition(randomDirection, out navMeshHit, distance, NavMesh.AllAreas))
        {
            return navMeshHit.position;
        }
        else
        {
            return GetRandomNavMeshPosition(origin, distance);
        }
    }



    public virtual void AnimalReadyAttack()
    {
        // Get the player's GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                // Set the destination of the NavMeshAgent to the player's position
                agent.SetDestination(player.transform.position);

                // Check if the animal is within attack range
                float attackRange = 2f; // Adjust as needed
                if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
                {
                    // Perform the attack action, such as reducing player's health
                    // For example:
                    PlayerStats playerStats = player.GetComponent<PlayerStats>();
                    if (playerStats != null)
                    {
                        playerStats.TakeDamage(20); // Adjust damage amount as needed
                    }

                    // You can also trigger animations or other effects here
                }
            }
        }
    }

    public IEnumerator WaitForPlayerMovement()
    {
        Vector3 previousPlayerPosition = player.transform.position;

        transform.LookAt(player.transform);

        while (currentState == AnimalState.Attack)
        {
            yield return null;

            // If the player moves during the attack, follow them
            if (Vector3.Distance(player.transform.position, previousPlayerPosition) > 0.1f)
            {
                // Set the destination to the player's position
                navMeshAgent.SetDestination(player.transform.position);
            }

            previousPlayerPosition = player.transform.position;
        }
    }

    IEnumerator AnimalAttack()
    {
        yield return new WaitUntil(() => navMeshAgent.pathPending);
        //Debug.Log("Path calculated");
        float attackRange = 2f;
        float startTime = Time.time;
        // Debug.Log("Distance Remaining " + (navMeshAgent.remainingDistance));

        while (navMeshAgent.remainingDistance > attackRange)
        {

        }

    }

    public enum AnimalState
    {
        Idle,
        Walk,
        Run,
        Death,
        Attack,
        Wounded
    }
}