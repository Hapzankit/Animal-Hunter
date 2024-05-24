using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;


[RequireComponent(typeof(NavMeshAgent))]
public class Hippo : Animals
{
    public float nearPlayerDistance = 2;

    // Start is called before the first frame update
    void Start()
    {
        InitializeAnimal();

        player = GameObject.FindGameObjectWithTag("Player");
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
            case AnimalState.Attack:
                HandleAttackState();
                break;
            case AnimalState.Wounded:
                HandleWoundedState();
                break;
        }
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
       //IF animal needs to flee
        StartCoroutine(nameof(WaitToRun));
        Debug.Log("Ainmal runing");
       //IF animal needs to attack
       //StartCoroutine()

    }

    public override void HandleWoundedState()
    {
        Debug.Log("Animal is Wounded set the player destination");
        navMeshAgent.ResetPath();

        int randomNo = Random.Range(0, 100);

        //if randomNo is less than 50 then animal will attack otherwise it will Flee.
        bool ShouldAttack = randomNo < 90 ? true : false; 

        Vector3 targetPosition = Vector3.zero;

        if (ShouldAttack)
        {
            targetPosition = player.transform.position + player.transform.forward;
            GotoAttackMode = true;
        }
        else
        {
            GotoAttackMode = false ;
            targetPosition = GetRandomNavMeshPosition(transform.position, wanderDistance);
        }

        //Either Flee for Attack the player
        navMeshAgent.SetDestination(targetPosition);   
        Debug.Log("current Navmesh speed");
        SetNavMeshAgentSpeed(runSpeed);
        Debug.Log("after wounded Navmesh speed");
        SetState(AnimalState.Run);

    }

    public override void HandleAttackState()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 100);

        player.GetComponentInChildren<PlayerStats>().TakeDamage(20);

        StartCoroutine(WaitForPlayerMovement());

    }

    private IEnumerator WaitToMove()
    {
        float waitTime = Random.Range(idleTime/2, idleTime*2);
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
           
           
            if(Time.time - startTime >= maxWalkTime)
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
        // Wait until the path is completely calculated
        //yield return new WaitUntil(() => !navMeshAgent.pathPending);

        yield return new  WaitUntil(() => navMeshAgent.pathPending);

        if (GotoAttackMode)
        {
            StartCoroutine(WaitForAttack());
        }
        else
        {
            StartCoroutine(WaitForFlee());
        }

        
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

        animator.SetBool("IsIdle", newState == AnimalState.Idle);
        animator.SetBool("IsWalking", newState == AnimalState.Walk);
        animator.SetBool("IsRunning", newState == AnimalState.Run);
        animator.SetBool("IsAttacking", newState == AnimalState.Attack);
        animator.ResetTrigger("IsDead");
        
    }

    
}
