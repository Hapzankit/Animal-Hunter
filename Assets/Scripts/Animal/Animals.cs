using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class Animals : MonoBehaviour
{
    [Header("Wander")]
    public float wanderDistance = 50f;
    public float walkSpeed = 5f;
    public float runSpeed = 4f;
    public float maxWalkTime = 6f;

    [Header("Idle")]
    public float idleTime = 5f;

    public NavMeshAgent navMeshAgent;
    public AnimalState currentState = AnimalState.Idle;

    public Animator animator;

    public float health = 90;
    public string animalName;

    public bool GotoAttackMode;

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

    public WeakPoints weakPoints;

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

    public virtual IEnumerator WaitForAttack()
    {
        // Wait until the path is completely calculated
        yield return new WaitUntil(() => navMeshAgent.pathPending);

        Debug.Log("Remaining Distance to Reach the player: " + navMeshAgent.pathEndPosition);
        Debug.Log("Checking Distance to Reach the player: " + Vector3.Distance(transform.position, player.transform.position));

        // Continuously check if the agent is within attack range
        while (true)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance - 2)
            {
                navMeshAgent.ResetPath();
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    // Then Changing the state to attack player
                    Debug.Log("Starting to attack system");
                    SetState(AnimalState.Attack);
                    break; // Break the loop if the attack starts
                }
            }
            yield return null; // Wait a frame before checking again
        }
    }

    public virtual IEnumerator WaitForFlee(int maxRepetitions = 3)
    {
        int currentRepetition = 0;

        while (currentRepetition < maxRepetitions)
        {
            // Wait until the path is completely calculated
            yield return new WaitUntil(() => !navMeshAgent.pathPending);

            Debug.Log("Remaining Distance to Reach the player: " + navMeshAgent.pathEndPosition);
            Debug.Log("Checking Distance to Reach the player: " + Vector3.Distance(transform.position, player.transform.position));

            // Continuously check if the agent is within attack range
            while (true)
            {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    navMeshAgent.ResetPath();
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        currentRepetition++;
                        Vector3 targetPosition = GetRandomNavMeshPosition(transform.position, wanderDistance);
                        navMeshAgent.SetDestination(targetPosition);
                        Debug.Log($"Repetition {currentRepetition} completed.");
                        break; // Break the loop if the agent reaches its destination
                    }
                }
                yield return null; // Wait a frame before checking again
            }
        }

        // Transition to idle state after completing the repetitions
        SetNavMeshAgentSpeed(3f);
        SetState(AnimalState.Idle);
        Debug.Log("Transitioning to Idle state.");
    }

    public void SetNavMeshAgentSpeed(float speedAmount)
    {
        navMeshAgent.speed = speedAmount;
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