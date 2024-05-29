using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentRotationDirection : MonoBehaviour
{
    public NavMeshAgent agent;
    public float straightTolerance = 3f;  // Tolerance angle in degrees to consider as "straight"

    void Update()
    {
        if (agent.hasPath)
        {
            // Get the agent's current forward direction
            Vector3 currentForward = transform.forward;

            // Calculate the desired direction
            Vector3 desiredDirection = (agent.steeringTarget - transform.position).normalized;

            // Calculate the signed angle between the current forward direction and the desired direction
            float angle = Vector3.SignedAngle(currentForward, desiredDirection, Vector3.up);

            // Determine the rotation direction
            if (Mathf.Abs(angle) <= straightTolerance)
            {
                Debug.Log("The agent is moving straight.");
            }
            else if (angle > 0)
            {
                Debug.Log("The agent is rotating to the right.");
            }
            else if (angle < 0)
            {
                Debug.Log("The agent is rotating to the left.");
            }
        }
    }
}
