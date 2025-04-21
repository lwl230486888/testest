

using System.Collections; // Required for IEnumerator
using UnityEngine;

public class PickUpWithAnimation : MonoBehaviour
{
    public GameObject myHands; // Reference to the hands GameObject
    public GameObject rightArm;  // Reference to the right arm GameObject
    public GameObject rightElbow; // Reference to the right elbow GameObject
    public GameObject rightWrist; // Reference to the right wrist GameObject

    // Right Hand Fingers in the specified order
    public GameObject indexFinger1_R; // Right Hand Index Finger (Part 1)
    public GameObject indexFinger2_R; // Right Hand Index Finger (Part 2)
    public GameObject littleFinger1_R; // Right Hand Little Finger (Part 1)
    public GameObject littleFinger2_R; // Right Hand Little Finger (Part 2)
    public GameObject middleFinger1_R; // Right Hand Middle Finger (Part 1)
    public GameObject middleFinger2_R; // Right Hand Middle Finger (Part 2)
    public GameObject ringFinger1_R; // Right Hand Ring Finger (Part 1)
    public GameObject ringFinger2_R; // Right Hand Ring Finger (Part 2)
    public GameObject thumb0_R; // Right Hand Thumb

    private GameObject objectIWantToPickUp; // The GameObject to pick up
    private bool hasItem; // To check if the item is held

    private Quaternion initialThumbRotation;
    private Quaternion[] initialFingerRotations;

    void Start()
    {
        // Find the "Book" object in the scene
        objectIWantToPickUp = GameObject.Find("MyNewBook");

        if (objectIWantToPickUp != null) // Check if the object was found
        {
            // Capture current rotations
            initialThumbRotation = thumb0_R.transform.localRotation;

            initialFingerRotations = new Quaternion[8];
            initialFingerRotations[0] = indexFinger1_R.transform.localRotation;
            initialFingerRotations[1] = indexFinger2_R.transform.localRotation;
            initialFingerRotations[2] = littleFinger1_R.transform.localRotation;
            initialFingerRotations[3] = littleFinger2_R.transform.localRotation;
            initialFingerRotations[4] = middleFinger1_R.transform.localRotation;
            initialFingerRotations[5] = middleFinger2_R.transform.localRotation;
            initialFingerRotations[6] = ringFinger1_R.transform.localRotation;
            initialFingerRotations[7] = ringFinger2_R.transform.localRotation;

            //StartCoroutine(PerformPickUp()); // Start the pick-up sequence
        }
        else
        {
            Debug.LogWarning("Book object not found in the scene.");
        }
    }

    public void StartPerformPickUp()
    {
        StartCoroutine(PerformPickUp()); // Start the pick-up sequence
    }

    private IEnumerator PerformPickUp()
    {
        // Start both coroutines without waiting for one to finish
        StartCoroutine(MoveElbowAndWristAndPickUp()); // Move elbow and wrist
        yield return StartCoroutine(AnimationAndPickUp()); // Then animate the pick-up
    }

    // private IEnumerator MoveElbowAndWristAndPickUp()
    // {
    //     float elbowDuration = 1.3f; // Duration to move the elbow
    //     float targetElbowYRotation = 28.88f; // Target Y rotation for the elbow
    //     float timer = 0f;

    //     // Initial rotations
    //     Quaternion initialElbowRotation = rightElbow.transform.localRotation;
    //     Quaternion initialWristRotation = rightWrist.transform.localRotation;

    //     // Move elbow to target rotation
    //     while (timer < elbowDuration)
    //     {
    //         float t = timer / elbowDuration;

    //         // Rotate elbow to target rotation on Y-axis
    //         float newYRotation = Mathf.LerpAngle(initialElbowRotation.eulerAngles.y, targetElbowYRotation, t);
    //         rightElbow.transform.localRotation = Quaternion.Euler(initialElbowRotation.eulerAngles.x, newYRotation, initialElbowRotation.eulerAngles.z);

    //         // Rotate wrist to match the correct path on X-axis
    //         float newXRotation = Mathf.LerpAngle(initialWristRotation.eulerAngles.x, targetElbowYRotation, t);
    //         rightWrist.transform.localRotation = Quaternion.Euler(newXRotation, initialWristRotation.eulerAngles.y, initialWristRotation.eulerAngles.z);

    //         timer += Time.deltaTime;
    //         yield return null; // Wait for the next frame
    //     }

    //     // Ensure elbow and wrist are at target rotations
    //     rightElbow.transform.localRotation = Quaternion.Euler(initialElbowRotation.eulerAngles.x, targetElbowYRotation, initialElbowRotation.eulerAngles.z);
    //     rightWrist.transform.localRotation = Quaternion.Euler(targetElbowYRotation, initialWristRotation.eulerAngles.y, initialWristRotation.eulerAngles.z);
    // }
    private IEnumerator MoveElbowAndWristAndPickUp()
{
    float duration = 0.6f; // Duration to complete the rotation
    float timer = 0f;

    // Target rotations
    Quaternion targetArmRotation = Quaternion.Euler(-51.88f, 8.132f, -2.197f);
    Quaternion targetElbowRotation = Quaternion.Euler(-26.564f, -107.82f, 20.86f);
    Quaternion targetWristRotation = Quaternion.Euler(28.88f, -19.456f, -0.248f);

    // Initial rotations
    Quaternion initialArmRotation = rightArm.transform.localRotation;
    Quaternion initialElbowRotation = rightElbow.transform.localRotation;
    Quaternion initialWristRotation = rightWrist.transform.localRotation;

    // Rotate arm, elbow, and wrist simultaneously
    while (timer < duration)
    {
        float t = timer / duration;

        // Rotate arm
        rightArm.transform.localRotation = Quaternion.Slerp(initialArmRotation, targetArmRotation, t);

        // Rotate elbow
        rightElbow.transform.localRotation = Quaternion.Slerp(initialElbowRotation, targetElbowRotation, t);

        // Rotate wrist
        rightWrist.transform.localRotation = Quaternion.Slerp(initialWristRotation, targetWristRotation, t);

        timer += Time.deltaTime;
        yield return null; // Wait for the next frame
    }

    // Ensure final rotations are set correctly
    rightArm.transform.localRotation = targetArmRotation;
    rightElbow.transform.localRotation = targetElbowRotation;
    rightWrist.transform.localRotation = targetWristRotation;
}

    private IEnumerator AnimationAndPickUp()
    {
        float duration = 0.7f; // Total duration of the animation
        float thumbDuration = duration * 0.5f; // Duration for thumb rotation
        float timer = 0f;

        // Animate thumb and fingers together
        while (timer < duration)
        {
            float t = timer / duration;

            // Animate thumb to -85 degrees over the first half of the duration
            if (timer < thumbDuration)
            {
                float thumbT = timer / thumbDuration;
                Quaternion targetThumbRotation = Quaternion.Euler(-85f, -6.66f, -41.35f);
                thumb0_R.transform.localRotation = Quaternion.Slerp(initialThumbRotation, targetThumbRotation, thumbT);
            }
            else
            {
                // Use the remaining time to animate back to -73.85 degrees
                float thumbT = (timer - thumbDuration) / thumbDuration; // Normalize for the second half
                thumb0_R.transform.localRotation = Quaternion.Euler(Mathf.Lerp(-85f, -73.85f, thumbT), -6.66f, -41.35f);
            }

            // Animate fingers based on the time factor
            AnimateFingers(t);

            timer += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Final position and rotation after animation
        AnimateFingers(1f); // Ensure fingers are in the final position
        PickUp(); // Pick up the book

        yield return StartCoroutine(RotateArmAndElbowAfterPickUp());

    }

private IEnumerator RotateArmAndElbowAfterPickUp()
{
    float duration = 0.834f; // Duration for the new rotation
    float timer = 0f;

    // Target rotations
    Quaternion targetArmRotation = Quaternion.Euler(-59.48f, 14.27f, -23.08f);
    Quaternion targetElbowRotation = Quaternion.Euler(-47.85f, -0.967f, -80.05f);
    Quaternion targetWristRotation = Quaternion.Euler(4.618f, 40f, -2.845f);

    // Initial rotations
    Quaternion initialArmRotation = rightArm.transform.localRotation;
    Quaternion initialElbowRotation = rightElbow.transform.localRotation;
    Quaternion initialWristRotation = rightWrist.transform.localRotation;

    // Rotate arm, elbow, and wrist simultaneously
    while (timer < duration)
    {
        float t = timer / duration;

        // Rotate arm
        rightArm.transform.localRotation = Quaternion.Slerp(initialArmRotation, targetArmRotation, t);

        // Rotate elbow
        rightElbow.transform.localRotation = Quaternion.Slerp(initialElbowRotation, targetElbowRotation, t);

        // Rotate wrist
        rightWrist.transform.localRotation = Quaternion.Slerp(initialWristRotation, targetWristRotation, t);

        timer += Time.deltaTime;
        yield return null; // Wait for the next frame
    }

    // Ensure final rotations are set correctly
    rightArm.transform.localRotation = targetArmRotation;
    rightElbow.transform.localRotation = targetElbowRotation;
    rightWrist.transform.localRotation = targetWristRotation;
}

    private void AnimateFingers(float t)
    {
        // Set rotations for finger groups based on the animation progress (t)
        // Index fingers
        indexFinger1_R.transform.localRotation = Quaternion.Lerp(initialFingerRotations[0], Quaternion.Euler(-64.3f, 0, 0), t);
        indexFinger2_R.transform.localRotation = Quaternion.Lerp(initialFingerRotations[1], Quaternion.Euler(-31.6f, 0, 0), t);

        // Little fingers
        littleFinger1_R.transform.localRotation = Quaternion.Lerp(initialFingerRotations[2], Quaternion.Euler(-64.3f, 0, 0), t);
        littleFinger2_R.transform.localRotation = Quaternion.Lerp(initialFingerRotations[3], Quaternion.Euler(-14.3f, 0, 0), t);

        // Middle fingers
        middleFinger1_R.transform.localRotation = Quaternion.Lerp(initialFingerRotations[4], Quaternion.Euler(-64.3f, 0, 0), t);
        middleFinger2_R.transform.localRotation = Quaternion.Lerp(initialFingerRotations[5], Quaternion.Euler(-44.3f, 0, 0), t);

        // Ring fingers
        ringFinger1_R.transform.localRotation = Quaternion.Lerp(initialFingerRotations[6], Quaternion.Euler(-64.3f, 0, 0), t);
        ringFinger2_R.transform.localRotation = Quaternion.Lerp(initialFingerRotations[7], Quaternion.Euler(-31.6f, 0, 0), t);
    }

    void Update()
    {
        // Press "Q" to drop the item
        // if (Input.GetKeyDown(KeyCode.Q) && hasItem)
        // {
        //     Drop();
        // }
        //    if (Input.GetKeyDown(KeyCode.E))
        // {
        //    StartCoroutine(PerformPickUp()); // Start the pick-up sequence
        // }
    }

    private void PickUp()
    {
        if (objectIWantToPickUp != null) // Ensure there is an object to pick up
        {
            Rigidbody rb = objectIWantToPickUp.GetComponent<Rigidbody>(); // Try to get the Rigidbody

            if (rb != null) // Check if the Rigidbody is present
            {
                rb.isKinematic = true; // Disable physics
                objectIWantToPickUp.transform.parent = myHands.transform; // Make it a child of the hands
                
                // Set the position of the book after parenting
                objectIWantToPickUp.transform.localPosition = new Vector3(3e-05f, 0.00048f, -0.00108f); // Set the position
                
                // Set the rotation of the book
                objectIWantToPickUp.transform.localRotation = Quaternion.Euler(175.32f, 256.4f, 5f);
                
                hasItem = true; // Set hasItem to true
                Debug.Log("Picked up the book!"); // Log confirmation
            }
            else
            {
                Debug.LogError("Rigidbody not found on the book. Please add a Rigidbody component."); // Log an error
            }
        }
    }

    private void Drop()
    {
        if (objectIWantToPickUp != null) // Ensure there is an object to drop
        {
            Rigidbody rb = objectIWantToPickUp.GetComponent<Rigidbody>(); // Try to get the Rigidbody

            if (rb != null) // Check if the Rigidbody is present
            {
                rb.isKinematic = false; // Enable physics
                objectIWantToPickUp.transform.parent = null; // Remove it from being a child of the hands
                hasItem = false; // Reset hasItem to false
                Debug.Log("Dropped the object!"); // Log confirmation
            }
        }
    }
}