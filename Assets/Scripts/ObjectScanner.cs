using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class ObjectScanner : MonoBehaviour
{
    public string Tag = "Anomaly";
    // raycast length
    public float rayLength = 20f;
    // layer mask to scan only scannable objects
    public LayerMask layerMask;
    // right controller position and rotation
    public Transform rightController; 
    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed) // Change the trigger on grip instead
        {
            ShootRay();
        }
    }

    void ShootRay()
    {
        Ray ray = new Ray(rightController.position, rightController.forward);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, rayLength, layerMask))
        {
            if (hit.collider.CompareTag(Tag))
            {
                Debug.Log("Anomaly Scanned: " + Tag);
            }
        }
    }
}