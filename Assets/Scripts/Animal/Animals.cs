using JetBrains.Annotations;
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

    public RagdollOnOff ragdollOnOff;

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

    public abstract void HandleAttackState();

    public abstract void HandleWoundedState();

    public GameObject player;

    public WeakPoints weakPoints;

    public float updateInterval = 1f; // Time in seconds between updates
    private float timer;
    private bool checkIfPlayerMoving;

    public Transform[] DisappearWaypoints;

    public bool canAttack;


    public virtual void ReactToGunshot()
    {
        if (isAlive)
        {
            Debug.Log("ReactoGun called" + gameObject.name);

            StopAllCoroutines();
            currentState = AnimalState.Wounded;
            UpdateState();
        }

    }


    public void Update()
    {
        if (checkIfPlayerMoving)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (player != null)
                {
                    navMeshAgent.SetDestination(player.transform.position);
                }
                timer = updateInterval;
            }
        
        }

    }

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
            // If the player moves during the attack, follow them
            if (Vector3.Distance(player.transform.position, previousPlayerPosition) > 0.1f)
            {
                Debug.Log("Player Moved While Animal is Attacking");
                // Set the destination to the player's position
                navMeshAgent.ResetPath();
                navMeshAgent.SetDestination(player.transform.position);
                previousPlayerPosition = player.transform.position;
                //transform.LookAt(player.transform);
                SetState(AnimalState.Run);
                break;
            }

            yield return null;
        }
    }

    public virtual IEnumerator WaitForAttack()
    {

        //Debug.Log("Remaining Distance to Reach the player: " + navMeshAgent.pathEndPosition);
        //Debug.Log("Checking Distance to Reach the player: " + Vector3.Distance(transform.position, player.transform.position));

        checkIfPlayerMoving = true;

        // Continuously check if the agent is within attack range
        while (true)
        {

            Debug.Log("Remaining distance " + navMeshAgent.remainingDistance );
            float distance = Vector3.Distance(transform.position, player.transform.position);
            
            //if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance - 1)
            if(distance <= 4.5f)
            {
                Debug.Log("Starting to attack system");
                navMeshAgent.ResetPath();
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    checkIfPlayerMoving = false;
                    // Then Changing the state to attack player
                    //Debug.Log("Starting to attack system");
                    SetState(AnimalState.Attack);
                    break; // Break the loop if the attack starts
                }
            }
            yield return null; // Wait a frame before checking again
        }
    }

    private void SetFleePath(int currenRepetation, int maxRepetation)
    {
        int disappearWapointIndex = Random.Range(0, DisappearWaypoints.Length);
        Vector3 targetPosition = currenRepetation == maxRepetation - 1 ? DisappearWaypoints[disappearWapointIndex].position : GetRandomNavMeshPosition(transform.position, wanderDistance);
        Debug.Log("Setting New destination for run");
        navMeshAgent.SetDestination(targetPosition);
        //Debug.Log($"Repetition {currentRepetition} completed."); 
    }

    public virtual IEnumerator WaitForFlee(int maxRepetitions = 4)
    {
        int currentRepetation = 0;

        while (currentRepetation < maxRepetitions)
        {
            
            yield return new WaitUntil(() => !navMeshAgent.pathPending);

            
           // Debug.Log("Remaining Distance to Reach the player: " + navMeshAgent.pathEndPosition);
           // Debug.Log("Checking Distance to Reach the player: " + Vector3.Distance(transform.position, player.transform.position));

            // Continuously check if the agent is within attack range
            while (true)
            {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    //Debug.Log("Flee Loop ");
                    navMeshAgent.ResetPath();
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        currentRepetation++;
                        SetFleePath(currentRepetation, maxRepetitions);
                        break;
                    }
                }
                yield return null; 
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

    public virtual void OnStateChanged(AnimalState newState)
    {
        UpdateState();


        //Debug.Log("animation state " + newState.ToString());
        // Switch based on the new state to set Animator parameters

        if (newState == AnimalState.Death)
        {
            animator.SetFloat("Dead Animation", Random.Range(0, 2));
        }

        animator.SetBool("IsIdle", newState == AnimalState.Idle);
        animator.SetBool("IsWalking", newState == AnimalState.Walk);
        animator.SetBool("IsRunning", newState == AnimalState.Run);
        animator.SetBool("IsAttacking", newState == AnimalState.Attack);
        animator.SetBool("IsDead",  newState == AnimalState.Death);
    }

    public virtual IEnumerator WaitToMove()
    {
        float waitTime = Random.Range(idleTime / 2, idleTime * 2);
        yield return new WaitForSeconds(waitTime);
        Vector3 randomDestination = GetRandomNavMeshPosition(transform.position, wanderDistance);
        //Debug.Log("New Destination" + randomDestination);
        navMeshAgent.SetDestination(randomDestination);
        SetState(AnimalState.Walk);
    }

    public virtual IEnumerator WaitToReachDestination()
    {
        yield return new WaitUntil(() => !navMeshAgent.pathPending);
        //Debug.Log("Path calculated");
        float startTime = Time.time;
        // Debug.Log("Distance Remaining " + (navMeshAgent.remainingDistance));

        while (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            if (Time.time - startTime >= maxWalkTime)
            {
                navMeshAgent.ResetPath();
                //Debug.Log("walk time is over ");
                SetState(AnimalState.Idle);
                yield break;
            }
            yield return null;
        }
        // Destination has been reached
        SetState(AnimalState.Idle);
    }

    private void OnDestroy()
    {
        EventManager.OnGunshotFired -= ReactToGunshot;
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