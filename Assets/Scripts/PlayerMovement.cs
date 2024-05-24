using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    [SerializeField]
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero; // Current movement direction

    // Set the movement boundaries along the x-axis
    private float minX = 75f;
    private float maxX = 85f;

    public bool isMoving;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (moveDirection != Vector3.zero)
        {
            Move();
        }
    }

    public void StartMovingLeft()
    {
        moveDirection = Vector3.left;
        isMoving = true;
    }

    public void StartMovingRight()
    {
        moveDirection = Vector3.right;
        isMoving = true;
    }

    public void StopMoving()
    {
        moveDirection = Vector3.zero;
        isMoving = false;

    }

    private void Move()
    {
        Vector3 movement = moveDirection * speed * Time.deltaTime;

        // Calculate new position before actually moving
        Vector3 newPosition = transform.position + movement;

        // Clamp the new position's x before moving to ensure it remains within the specified range
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        // Calculate the final movement vector after clamping
        Vector3 clampedMovement = newPosition - transform.position;

        // Move the character controller
        characterController.Move(clampedMovement);
    }
}
