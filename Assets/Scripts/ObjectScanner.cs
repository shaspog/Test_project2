using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class ObjectScanner : MonoBehaviour
{

    [System.Serializable]
    public class AnomalyPair
    {
        public GameObject anomalyObject;
        public GameObject normalObject;
        public Transform specificSpawnLocation;
    }
    // Anomaly Pair List
    public List<AnomalyPair> anomalyPairs;
    // Spawn Location List
    public List<Transform> spawnLocations;
    // tag 
    public string Tag = "Anomaly";
    // raycast length
    public float rayLength = 20f;
    // layer mask to scan only scannable objects
    public LayerMask layerMask;
    // right controller position and rotation
    public Transform rightController;


    private void Start()
    {
        RandomizeAnomalies();
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed) // Change the trigger on grip instead
        {
            ShootRay();
        }
    }

    void RandomizeAnomalies()
    {
        foreach ( var pair in anomalyPairs)
        {
            Transform spawnLocation = pair.specificSpawnLocation != null ? pair.specificSpawnLocation : GetRandomSpawnLocation();
            pair.anomalyObject.transform.position = spawnLocation.position;
            pair.anomalyObject.transform.rotation = spawnLocation.rotation;

            //Enable and disable counterparts

            pair.anomalyObject.SetActive(true);
            pair.normalObject.SetActive(false);

        }
    }

    Transform GetRandomSpawnLocation()
    {
        return spawnLocations[Random.Range(0, spawnLocations.Count)];
    }

    void ShootRay()
    {
        Ray ray = new Ray(rightController.position, rightController.forward);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, rayLength, layerMask))
        {
            foreach (var pair in anomalyPairs)
            {
                if (hit.collider.CompareTag(Tag) && hit.collider.gameObject == pair.anomalyObject)
                {              
                        Debug.Log("Scanned anomaly" + pair.anomalyObject.name);

                        //replace with counterpart 
                        pair.anomalyObject.SetActive(false);
                        pair.normalObject.transform.position = pair.anomalyObject.transform.position;
                        pair.normalObject.transform.rotation = pair.anomalyObject.transform.rotation;
                        pair.normalObject.SetActive(true);

                        break; //exit loop after scanning 
                    
                }
            }
        }
    }
}