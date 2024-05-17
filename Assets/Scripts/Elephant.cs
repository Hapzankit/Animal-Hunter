using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Elephant : Animals
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

    public override void HandleWoundedState()
    {

        //Either Flee for Attack the player
        //navMeshAgent.SetDestination(player.transform.position);    //For now only adding Attack state
        //navMeshAgent.speed *= 2;
        //SetState(AnimalState.Run);

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

    private IEnumerator WaitToMove()
    {
        float waitTime = Random.Range(idleTime / 2, idleTime * 2);
        yield return new WaitForSeconds(waitTime);

        Vector3 randomDestination = GetRandomNavMeshPosition(transform.position, wanderDistance);
        //Debug.Log("New Destination" + randomDestination);
        navMeshAgent.SetDestination(randomDestination);
        SetState(AnimalState.Walk);
    }

    private IEnumerator WaitToReachDestination()
    {
        yield return new WaitUntil(() => navMeshAgent.pathPending);
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
        {
            //Debug.Log("getting same as previous state " + newState);
            return;
        }

        currentState = newState;
        //Debug.Log("changing the state " + newState.ToString());
        OnStateChanged(newState);
    }


    public override void OnStateChanged(AnimalState newState)
    {
        UpdateState();

        //Debug.Log("animation state " + newState.ToString());

        // Switch based on the new state to set Animator parameters
        switch (newState)
        {
            case AnimalState.Idle:
                animator.SetBool("IsIdle", true);
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRunning", false);
                animator.ResetTrigger("IsDead");
                // Debug.Log("animation changing to Idle");

                // Assuming "IsDead" is a trigger for the death animation
                // and should be reset when transitioning to other states
                break;

            case AnimalState.Walk:
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsIdle", false);
                animator.ResetTrigger("IsDead");
                //Debug.Log("animation changing to Walk");
                break;

            case AnimalState.Run:
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsWalking", false); // Depending on your setup, you may not need this
                animator.SetBool("IsIdle", false);
                animator.ResetTrigger("IsDead");
                // Debug.Log("animation changing to Run");
                break;
        }
    }
}
