using UnityEngine;

public class ChairMovement : MonoBehaviour
{
    public Vector3 targetPosition;  // target position the chair will move to
    public float speed = 2.0f;      // speed of the movement
    private Vector3 startPosition;  // initial position of the chair
    private bool shouldMove = false; // flag to trigger the movement

    void Start()
    {
        startPosition = transform.position; // store the initial position of the chair

        // automatically start moving after 5 seconds
        Invoke("StartMoving", 5f);
    }

    void Update()
    {
        if (shouldMove)
        {
            // move the chair towards the target position smoothly
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

            // if the chair is very close to the target position, stop moving
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                shouldMove = false; // Stop the movement
            }
        }
    }

    // triggers the chair to start moving
    public void StartMoving()
    {
        shouldMove = true;
    }

    // can reset the chair to its original position
    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}
