using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalZRotation : MonoBehaviour
{
    public float rotationSpeed = 30f;  // Speed of rotation in degrees per second

    void Update()
    {
        // Rotate only on the local Z-axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
