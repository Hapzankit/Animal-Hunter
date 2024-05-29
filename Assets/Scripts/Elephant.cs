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
            default:
                Debug.Log("Death happened");
                break;
        }
    }

    public override void HandleWoundedState()
    {
       // Debug.Log("Animal is Wounded set the player destination");
        navMeshAgent.ResetPath();

        int randomNo = Random.Range(0, 100);

        //if randomNo is less than 50 then animal will attack otherwise it will Flee.
        bool ShouldAttack = randomNo < 10 ? true : false;

        Vector3 targetPosition = Vector3.zero;

        if (ShouldAttack)
        {
            targetPosition = player.transform.position + player.transform.forward;
            GotoAttackMode = true;
        }
        else
        {
            GotoAttackMode = false;
            targetPosition = GetRandomNavMeshPosition(transform.position, wanderDistance);
        }

        //Either Flee for Attack the player
        navMeshAgent.SetDestination(targetPosition);
        //Debug.Log("current Navmesh speed");
        SetNavMeshAgentSpeed(runSpeed);
        //Debug.Log("after wounded Navmesh speed");
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

    private IEnumerator WaitToRun()
    {
        yield return new WaitUntil(() => navMeshAgent.pathPending);

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


    
}
