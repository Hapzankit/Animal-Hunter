using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WildBeast : Animals
{

    // Start is called before the first frame update
    void Start()
    {
        InitializeAnimal();
    }

    public override void InitializeAnimal()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetNavMeshAgentSpeed(walkSpeed);

        currentState = AnimalState.Idle;
        UpdateState();
    }

    public override void UpdateState()
    {
        switch (currentState)
        {
            case AnimalState.Idle:
                HandleIdleState();
                break;
            case AnimalState.Walk:
                HandleWalkState();
                break;
            case AnimalState.Run:
                HandleRunState();
                break;
            case AnimalState.Death:
                HandleDeathState();
                break;
        }
    }

    public override void HandleAttackState()
    {

       // player.GetComponentInChildren<PlayerStats>().TakeDamage(20);

    }

    public override void HandleIdleState()
    {
        StartCoroutine(nameof(WaitToMove));
    }

    public override void HandleWalkState()
    {
        StartCoroutine(nameof(WaitToReachDestination));
    }

    public override void HandleRunState()
    {
        StartCoroutine(nameof(WaitToRun));
    }

    protected virtual void HandleDeathState()
    {
    }

    public override void HandleWoundedState()
    {

    }

    private IEnumerator WaitToMove()
    {
        float waitTime = Random.Range(idleTime / 2, idleTime * 2);
        yield return new WaitForSeconds(waitTime);

        Vector3 randomDestination = GetRandomNavMeshPosition(transform.position, wanderDistance);
        
        navMeshAgent.SetDestination(randomDestination);
        SetState(AnimalState.Walk);
    }

    private IEnumerator WaitToReachDestination()
    {
        float startTime = Time.time;

        while (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            if (Time.time - startTime >= maxWalkTime)
            {
                navMeshAgent.ResetPath();
                SetState(AnimalState.Idle);
                yield break;
            }

            yield return null;
        }

        // Destination has been reached
        SetState(AnimalState.Walk);
    }

    private IEnumerator WaitToRun()
    {
        float waitTime = Random.Range(idleTime / 2, idleTime * 2);
        yield return new WaitForSeconds(waitTime);

        Vector3 randomDestination = GetRandomNavMeshPosition(transform.position, wanderDistance);

        navMeshAgent.SetDestination(randomDestination);
        SetState(AnimalState.Run);
    }

    public override void SetState(AnimalState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
        OnStateChanged(newState);
    }

    public override void OnStateChanged(AnimalState newState)
    {
        UpdateState();

        // Switch based on the new state to set Animator parameters
        switch (newState)
        {
            case AnimalState.Idle:
                animator.SetBool("IsIdle", true);
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRunning", false);
                // Assuming "IsDead" is a trigger for the death animation
                // and should be reset when transitioning to other states
                animator.ResetTrigger("IsDead");
                break;
            case AnimalState.Walk:
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsIdle", false);
                animator.ResetTrigger("IsDead");
                break;
            case AnimalState.Run:
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsWalking", false); // Depending on your setup, you may not need this
                animator.SetBool("IsIdle", false);
                animator.ResetTrigger("IsDead");
                break;
        }
    }
}
