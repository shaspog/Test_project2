using UnityEngine;

public class ChairMovement : MonoBehaviour
{
    public Vector3 targetPosition;  // target position the chair will move to
    public float speed = 2.0f;      // speed of the movement
    private Vector3 startPosition;  // initial position of the chair
    private bool shouldMove = false; // flag to trigger the movement

    //Sultan
    public string requireTag = "Anomaly"; //tag 
    private bool isResetting = false; //reset transform 

    void Start()
    {
        startPosition = transform.position; // store the initial position of the chair

        // automatically start moving after 5 seconds - Updated Sultan InvokeRepeating calls every 30 second after the first 5 to move the object
        InvokeRepeating("ChanceStartMoving", 5f, 30f);
    }
 

    void Update()
    {
        if (shouldMove)
        {
            //Sultan
            Vector3 destination = isResetting ? startPosition : targetPosition;

            // move the chair towards the target position smoothly
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            // if the chair is very close to the target position, stop moving
            if (Vector3.Distance(transform.position, destination) < 0.01f)
            {
                //Sultan
              if (!isResetting)
                {
                    shouldMove = false;
                    Invoke("ReturnToOrigin", 2f);
                }
              else
                {
                    shouldMove = false;
                    isResetting = false;
                }
            }
        }
    }

    // triggers the chair to start moving
    public void ChanceStartMoving()
    {
        //Sultan
        if (gameObject.CompareTag(requireTag))
        {
            //Probability chance
            if (Random.value <= 0.25f)
            {
                StartMoving();
            }
        }
    }

    //Sultan
    public void StartMoving()
    {
        shouldMove = true;
    }

    //Sultan
    public void ReturnToOrigin()
    {
        isResetting = true;
        shouldMove = true;
    }


    // can reset the chair to its original position
    public void ResetPosition()
    {
        transform.position = startPosition;
        //sultan
        shouldMove = false;
        isResetting = false;
    }
}
