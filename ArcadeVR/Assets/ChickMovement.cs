using UnityEngine;
using System.Collections;

public class ChickMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 0.5f;
    public float rotationSpeed = 2f;
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;
    public GameObject room; // Reference to your room object
    
    [Header("Optional Settings")]
    public bool debugMode = false;
    public float minDistanceFromWalls = 0.5f;

    private Vector3 targetPosition;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private float currentWaitTime;
    private Bounds roomBounds;
    private bool isMoving = false;
    private Animator animator;

    void Start()
    {
        // Try to get the animator if it exists
        animator = GetComponent<Animator>();

        if (room != null)
        {
            // Get the bounds of the room using its collider or renderer
            if (room.GetComponent<Collider>())
                roomBounds = room.GetComponent<Collider>().bounds;
            else if (room.GetComponent<Renderer>())
                roomBounds = room.GetComponent<Renderer>().bounds;
            
            SetNewRandomTarget();
        }
        else
        {
            Debug.LogWarning("Room reference not set for ChickMovement script!");
        }
    }

    void Update()
    {
        if (room == null) return;

        if (isWaiting)
        {
            HandleWaiting();
        }
        else
        {
            HandleMovement();
        }

        // Update animator if it exists
        if (animator != null)
        {
            animator.SetBool("IsWalking", isMoving);
        }
    }

    void HandleWaiting()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer >= currentWaitTime)
        {
            isWaiting = false;
            SetNewRandomTarget();
        }
        isMoving = false;
    }

    void HandleMovement()
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Keep the chick from flying

        if (direction.magnitude < 0.1f)
        {
            StartWaiting();
            return;
        }

        // Rotate towards target
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move forward
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        isMoving = true;

        // Optional: Check if we're too close to walls
        if (IsNearWalls())
        {
            SetNewRandomTarget();
        }
    }

    void SetNewRandomTarget()
    {
        if (room != null)
        {
            float randomX = Random.Range(
                roomBounds.min.x + minDistanceFromWalls, 
                roomBounds.max.x - minDistanceFromWalls
            );
            float randomZ = Random.Range(
                roomBounds.min.z + minDistanceFromWalls, 
                roomBounds.max.z - minDistanceFromWalls
            );
            targetPosition = new Vector3(randomX, transform.position.y, randomZ);

            if (debugMode)
            {
                Debug.DrawLine(transform.position, targetPosition, Color.red, 1f);
            }
        }
    }

    void StartWaiting()
    {
        isWaiting = true;
        waitTimer = 0f;
        currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
    }

    bool IsNearWalls()
    {
        float distanceFromBoundaryX = Mathf.Min(
            Mathf.Abs(transform.position.x - roomBounds.min.x),
            Mathf.Abs(transform.position.x - roomBounds.max.x)
        );
        float distanceFromBoundaryZ = Mathf.Min(
            Mathf.Abs(transform.position.z - roomBounds.min.z),
            Mathf.Abs(transform.position.z - roomBounds.max.z)
        );

        return distanceFromBoundaryX < minDistanceFromWalls || distanceFromBoundaryZ < minDistanceFromWalls;
    }

    void OnDrawGizmosSelected()
    {
        if (!debugMode || room == null) return;

        Gizmos.color = Color.yellow;
        if (roomBounds != null && roomBounds.size != Vector3.zero)
        {
            Gizmos.DrawWireCube(roomBounds.center, roomBounds.size);
        }
        
        // Draw target position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 0.1f);
    }
}