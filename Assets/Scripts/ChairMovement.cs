using UnityEngine;

public class ChairMovement : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    public float speed = 2.0f;      
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool shouldMove = false; 

    //Sultan
    public string requireTag = "Anomaly"; //tag 
    private bool isResetting = false; 

    void Start()
    {
        startPosition = transform.position; 
        startRotation = transform.rotation;

        // repeat every 5 seconds
        InvokeRepeating("ChanceStartMoving", 5f, 30f);
    }
 

    void Update()
    {
        if (shouldMove)
        {
            //Sultan
            Vector3 destination = isResetting ? startPosition : targetPosition;
            Quaternion destinationRotation = isResetting ? startRotation : Quaternion.Euler(targetRotation);

            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, destinationRotation, speed * Time.deltaTime * 20f);

            if (Vector3.Distance(transform.position, destination) < 0.01f && Quaternion.Angle(transform.rotation, destinationRotation) < 0.1f)
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
        transform.rotation = startRotation;
        //sultan
        shouldMove = false;
        isResetting = false;
    }
}
