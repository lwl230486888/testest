// // // // using UnityEngine;

// // // // public class CameraController : MonoBehaviour
// // // // {
// // // //     public float moveSpeed = 5f;

// // // //     void Update()
// // // //     {
// // // //         // Get input from the user
// // // //         float moveHorizontal = Input.GetAxis("Horizontal");
// // // //         float moveVertical = Input.GetAxis("Vertical");

// // // //         // Create a movement vector
// // // //         Vector3 movement = new Vector3(moveHorizontal, 1.0f, moveVertical) * moveSpeed * Time.deltaTime;

// // // //         // Move the camera
// // // //         transform.Translate(movement);
// // // //     }
// // // // }

// // using UnityEngine;

// // public class CameraController : MonoBehaviour
// // {
// //     public float maxSpeed = 20f; // Maximum speed of the camera
// //     public float acceleration = 2f; // Acceleration rate
// //     public float deceleration = 1f; // Deceleration rate

// //     private Vector3 targetPosition; // Target position for the camera
// //     private float currentSpeed = 0f; // Current speed of the camera
// //     private bool isMoving = false; // Check if the camera is moving

// //     private void Start()
// //     {
// //         // Set the target position when the game starts
// //         targetPosition = new Vector3(-1, 3.3f, 16);
// //         isMoving = true; // Start moving towards the target position
// //     }

// //     private void Update()
// //     {
// //         if (isMoving)
// //         {
// //             // Calculate distance to target
// //             float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

// //             // Accelerate until halfway
// //             if (distanceToTarget > Vector3.Distance(transform.position, targetPosition) / 2f)
// //             {
// //                 currentSpeed += acceleration * Time.deltaTime; // Accelerate
// //             }
// //             // Decelerate after halfway
// //             else
// //             {
// //                 currentSpeed -= deceleration * Time.deltaTime; // Decelerate
// //             }

// //             // Clamp speed to max speed
// //             currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

// //             // Move the camera
// //             transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

// //             // Check if the camera reached the target
// //             if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
// //             {
// //                 transform.position = targetPosition; // Snap to target
// //                 isMoving = false; // Stop moving
// //             }
// //         }
// //     }
// // }

// // // using UnityEngine;

// // // public class CameraController : MonoBehaviour
// // // {
// // //     public Vector3 targetPosition;
// // //     public float totalTime = 3.55f; // Total time to reach the target
// // //     public float normalAcceleration = 2f; // Normal acceleration rate
// // //     public float normalDeceleration = 2f; // Normal deceleration rate
// // //     public float slowDecelerationRate = 0.01f; // Slow deceleration rate

// // //     private float elapsedTime = 0f; // Time elapsed since the start
// // //     private float currentSpeed = 0f; // Current speed of the camera
// // //     private bool isMoving = false; // Check if the camera is moving

// // //     private void Start()
// // //     {
// // //         targetPosition = new Vector3(-1, 3.3f, 16);
// // //         isMoving = true; // Start moving towards the target position
// // //     }

// // //     private void Update()
// // //     {
// // //         if (isMoving)
// // //         {
// // //             elapsedTime += Time.deltaTime; // Update elapsed time

// // //             // Calculate fraction of the journey completed
// // //             float t = elapsedTime / totalTime;

// // //             // Calculate distance to target
// // //             float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

// // //             if (t < 0.25f) // First 25% of the time: Normal acceleration
// // //             {
// // //                 currentSpeed += normalAcceleration * Time.deltaTime;
// // //             }
// // //             else if (t >= 0.25f && t < 0.75f) // Next 50% of the time: Normal deceleration
// // //             {
// // //                 currentSpeed -= normalDeceleration * Time.deltaTime;
// // //             }
// // //             else // Last 25% of the time: Slow deceleration
// // //             {
// // //                 currentSpeed -= slowDecelerationRate * Time.deltaTime;
// // //             }

// // //             // Clamp speed to ensure it doesn't go below zero
// // //             currentSpeed = Mathf.Max(currentSpeed, 0);

// // //             // Move the camera
// // //             transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

// // //             // Check if the camera reached the target
// // //             if (Vector3.Distance(transform.position, targetPosition) < 0.1f || elapsedTime >= totalTime)
// // //             {
// // //                 transform.position = targetPosition; // Snap to target
// // //                 isMoving = false; // Stop moving
// // //             }
// // //         }
// // //     }
// // // }
// using UnityEngine;

// public class CameraController : MonoBehaviour
// {
//     private Vector3 targetPosition; // Target position for the camera
//     private float totalTime; // Total time to reach the target
//     private float maxSpeed; // Maximum speed of the camera
//     private float acceleration; // Acceleration rate
//     private float deceleration; // Deceleration rate

//     private float elapsedTime = 0f; // Time elapsed since the start
//     private float currentSpeed = 0f; // Current speed of the camera
//     private bool isMoving = false; // Check if the camera is moving

//     private void Start()
//     {
//         targetPosition = new Vector3(-1, 3.3f, 16); // Set target position here
//         totalTime = 1f; // Set total time
//         maxSpeed = 20f; // Set maximum speed
//         acceleration = 4f; // Set acceleration rate
//         deceleration = 2f; // Set deceleration rate

//         isMoving = true; // Start moving towards the target position
//     }

//     private void Update()
//     {
//         if (isMoving)
//         {
//             elapsedTime += Time.deltaTime; // Update elapsed time

//             // Calculate the fraction of the journey completed
//             float t = elapsedTime / totalTime;

//             // Determine current speed based on time
//             if (t < 0.5f) // Accelerate for the first half of the time
//             {
//                 currentSpeed += acceleration * Time.deltaTime;
//                 // Remove clamping here to allow high speeds
//             }
//             else // Decelerate for the second half of the time
//             {
//                 currentSpeed -= deceleration * Time.deltaTime;
//                 currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed); // Clamp to max speed
//             }

//             // Ensure currentSpeed does not go below zero
//             currentSpeed = Mathf.Max(currentSpeed, 0);

//             // Move the camera
//             transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

//             // Check if the camera reached the target
//             if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
//             {
//                 transform.position = targetPosition; // Snap to target
//                 isMoving = false; // Stop moving
//             }
//         }
//     }
// }

using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 targetPosition; // Target position for the camera
    private float totalTime; // Total time to reach the target
    private float maxSpeed; // Maximum speed of the camera
    private float acceleration; // Acceleration rate
    private float deceleration; // Deceleration rate

    private float elapsedTime = 0f; // Time elapsed since the start
    private float currentSpeed = 0f; // Current speed of the camera
    private bool isMoving = false; // Check if the camera is moving
    private bool hasReachedHalfway = false; // Check if halfway has been reached
    private float initialDistance; // Initial distance to the target

    private void Start()
    {
        targetPosition = new Vector3(-1, 3.3f, 16); // Set target position here
        totalTime = 5f; // Set total time
        maxSpeed = 20f; // Set maximum speed
        acceleration = 7f; // Set acceleration rate
        deceleration = 7f; // Set deceleration rate

        initialDistance = Vector3.Distance(transform.position, targetPosition); // Calculate initial distance
        isMoving = true; // Start moving towards the target position
    }

    private void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime; // Update elapsed time

            // Stop movement if elapsed time exceeds 5 seconds
            if (elapsedTime > 5f)
            {
                isMoving = false; // Stop moving
                Debug.Log("Movement stopped after 5 seconds.");
                return; // Exit the update to avoid further processing
            }
            // Calculate the distance to the target
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            float halfwayDistance = initialDistance / 2;

            // Log distances for debugging
            Debug.Log($"Distance to Target: {distanceToTarget}, Halfway Distance: {halfwayDistance}");

            // Determine current speed based on distance
            if (distanceToTarget > halfwayDistance) // Accelerate until halfway
            {
                currentSpeed += acceleration * Time.deltaTime;
            }
            else // Decelerate after halfway
            {
                if (!hasReachedHalfway)
                {
                    Debug.Log("Reached halfway point!"); // Log when halfway is reached
                    hasReachedHalfway = true; // Ensure this log only happens once
                }
                currentSpeed -= deceleration * Time.deltaTime;
                
                currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed); // Clamp to max speed
            }

            // Ensure currentSpeed does not go below zero
            currentSpeed = Mathf.Max(currentSpeed, 0);

            // Move the camera
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

            // Check if the camera reached the target
            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition; // Snap to target
                isMoving = false; // Stop moving
            }
        }
    }
}