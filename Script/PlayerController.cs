using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerBody;
    [SerializeField] GameObject middleRay;

    // Target position, updated through movement input, be careful when editing this as most of the code works based upon it
    private Vector3 target;

    // Adjustable roation and movement variables
    [SerializeField] float rotationSpeed = 6.0f;
    [SerializeField] float movementSpeed = 2.0f;
    [SerializeField] float gridSize = 8.0f;

    // Collision detection variables
    private float detectWallDistance;

    public bool isMoving;
    public bool isRotating;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the directional raycast distance to detect walls with small error correction amount
        detectWallDistance = gridSize + 0.1f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // This state allows the player to imput movement
        if (isMoving == false && isRotating == false)
        {
            // Sets a movement target within a raycast collision detection system
            CollisionCheckedMovement();

            // Sets a rotation tartget
            SetRotationTarget();

        }
    }

    private void Update()
    {
        // Carries out movement to targets whilst adjusting the y axis height, to allow climbing and descending of stairs and falling due to gravity
        if (isMoving == true)
        {
            HeightCheckedMovementAnimation();
        }

        // Carries out rotation to targets whilst adjusting the final rotation vector to 90o to remove rounding errors
        if (isRotating == true)
        {
            ErrorCorrectionRotation();
        }
       
    }

    void CollisionCheckedMovement()
    {
        RaycastHit wallHit;

        // Remember to delete the "wall" tag check from the Physics.Raycast code or set your walls to have the tag "Walls"

        if (Input.GetKey(KeyCode.W))
        {
            Ray forwardRay = new Ray(middleRay.transform.position, transform.forward);

            
            if (Physics.Raycast(forwardRay, out wallHit, detectWallDistance) && wallHit.transform.CompareTag("Walls") && wallHit.distance <= detectWallDistance)
            {
                isMoving = false;
            }

            else
            {
                SetTargetForward();
            }

        }

        if (Input.GetKey(KeyCode.S))
        {
            Ray backwardRay = new Ray(middleRay.transform.position, -transform.forward);

            if (Physics.Raycast(backwardRay, out wallHit, detectWallDistance) && wallHit.transform.CompareTag("Walls") && wallHit.distance <= detectWallDistance)
            {
                 isMoving = false;
            }

            else
            {
                SetTargetBackward();
            }

        }

        if (Input.GetKey(KeyCode.A))
        {
            Ray leftRay = new Ray(middleRay.transform.position, -transform.right);
            if (Physics.Raycast(leftRay, out wallHit, detectWallDistance) && wallHit.transform.CompareTag("Walls") && wallHit.distance <= detectWallDistance)
            {
                isMoving = false;
            }

            else
            {
                SetTargetLeft();
            }

        }

        if (Input.GetKey(KeyCode.D))
        {
            Ray rightRay = new Ray(middleRay.transform.position, transform.right);
            if (Physics.Raycast(rightRay, out wallHit, detectWallDistance) && wallHit.transform.CompareTag("Walls") && wallHit.distance <= detectWallDistance)
            {
                isMoving = false;
            }

            else
            {
                SetTargetRight();
            }

        }
    }

    void SetTargetForward()
    { 
        Vector3 forward = playerBody.position + transform.forward * gridSize;
        target = forward;
        isMoving = true;
    }

    void SetTargetBackward()
    {
       Vector3 backward = playerBody.position + transform.forward * -gridSize;
        target = backward;
        isMoving = true;
    }

    void SetTargetLeft()
    {
        Vector3 left = playerBody.position + transform.right * -gridSize;
        target = left;
        isMoving = true;
    }

    void SetTargetRight()
    {
        Vector3 right = playerBody.transform.position + transform.right * gridSize;
        target = right;
        isMoving = true;
    }

    void SetRotationTarget()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            SetRotateTargetLeft();
        }

        if (Input.GetKey(KeyCode.E))
        {
            SetRotateTargetRight();
        }
    }

    void SetRotateTargetLeft()
    {
        isRotating = true;
        target = transform.right * -gridSize;
    }

    void SetRotateTargetRight()
    {
        isRotating = true;
        target = transform.right * gridSize;
    }

    void HeightCheckedMovementAnimation()
    {

            RaycastHit floorHit;

            Ray downRay = new Ray(middleRay.transform.position, -transform.up);

            if (Physics.Raycast(downRay, out floorHit))
            {
                SetTargetHeightToCurrentHeight(floorHit);
            }

            AnimatedMovement();

            HeightCorrectedMovementCheck(floorHit);

    }

    void SetTargetHeightToCurrentHeight(RaycastHit floorDistance)
    {
        target.y = playerBody.transform.position.y;

        if (floorDistance.distance < 2)
        {
            playerBody.transform.position += transform.up * gridSize * Time.deltaTime;
        }

        if (floorDistance.distance > 2)
        {
            playerBody.transform.position += -transform.up * gridSize * Time.deltaTime;
        }
    }

    void AnimatedMovement()
    {
        var step = Time.deltaTime * gridSize * movementSpeed;

        playerBody.position = Vector3.MoveTowards(playerBody.position, target, step);
    }

    void HeightCorrectedMovementCheck(RaycastHit floorDistance)
    {
        float movementDistance = Vector3.Distance(target, playerBody.transform.position);

        if (movementDistance == 0)
        {
            if (floorDistance.distance < 2.0)
            {
                playerBody.transform.position += transform.up * (2 - floorDistance.distance);
            }

            if (floorDistance.distance > 2.0)
            {
                playerBody.transform.position += -transform.up * (floorDistance.distance - 2);
            }
            isMoving = false;
        }
    }

    void ErrorCorrectionRotation()
    {
        var smoothRotation = Time.deltaTime * rotationSpeed;

        Quaternion quaternionTarget = Quaternion.LookRotation(target);
        transform.rotation = Quaternion.Lerp(transform.rotation, quaternionTarget, smoothRotation);
        float rotationAngle = Quaternion.Angle(transform.rotation, quaternionTarget);
        // corrects the inaccuracy of the rotation
        if (rotationAngle == 0f)
        {
            isRotating = false;
            transform.rotation = quaternionTarget;
        }
    }

    // Trouble shooting:
    // If the player is passing through walls occasionally:-
    // Set the wall detect distance amount higher than 0.1f;
    // or
    // Make sure that you set wall tags to "Walls" or remove that section of the code that checks for tags
}