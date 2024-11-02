using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anomaly_finder : MonoBehaviour
{
    public Transform pivotPoint;
    public float rotationSpeed = 15f;
    public float detectionRadius = 5f;

    private Vector3 targetDirection;
    private GameObject closestAnomaly;

    void Update()
    {
        FindClosestAnomaly();

        if (closestAnomaly != null)
        {
            // Calculate the direction from the pivot point to the closest anomaly
            targetDirection = closestAnomaly.transform.position - pivotPoint.transform.position;

            // Normalize the direction vector for consistent rotation
            targetDirection.Normalize();

            // Calculate the rotation around the Y-axis based on the target direction
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            // Smoothly rotate the pivot point
            pivotPoint.transform.rotation = Quaternion.Slerp(pivotPoint.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Enable the arrow model on all child mesh renderers
            foreach (MeshRenderer meshRenderer in pivotPoint.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.enabled = true;
            }
        }
        else
        {
            // Disable the arrow model on all child mesh renderers
            foreach (MeshRenderer meshRenderer in pivotPoint.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.enabled = false;
            }
        }
    }

    void FindClosestAnomaly()
    {
        GameObject[] anomalies = GameObject.FindGameObjectsWithTag("Anomaly");
        float closestDistance = Mathf.Infinity;
        closestAnomaly = null;

        foreach (GameObject anomaly in anomalies)
        {
            float distance = Vector3.Distance(pivotPoint.transform.position, anomaly.transform.position);
            if (distance < closestDistance && distance <= detectionRadius)
            {
                closestDistance = distance;
                closestAnomaly = anomaly;
            }
        }
    }
}
