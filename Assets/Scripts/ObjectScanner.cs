using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    //Input reference for trigger
    public InputActionProperty rightHandTriggerAction; // Works like a charm :3


    //UI Elements
    public Slider anomalyTimerSlider;
    public float maxTime = 100f;
    private float currentTime;

    //Cooldown for scanner
    public float scanCooldown = 3f;
    private float lastScanTime;

    private void Start()
    {
        RandomizeAnomalies();
        //Timer parts
        currentTime = maxTime;
        anomalyTimerSlider.maxValue = maxTime;
        anomalyTimerSlider.value = maxTime;
        //ScannerCooldown
        lastScanTime = scanCooldown; // scan from the get-go
    }

    void Update()
    {
        if (rightHandTriggerAction.action.ReadValue<float>() > 0.1f) //trigger as float instead of input 
        {
            if (Time.time >= lastScanTime + scanCooldown)
            {
                ShootRay();
                lastScanTime = Time.time; // update cd
            }
            else
            {
                Debug.Log("ability on cd"); // Change later into UI pop-up
            }
        }
        //Update Timer Function
        UpdateTimer();
    }

    #region GameOver UI Elements

    void GameOverCut()
    {
        Debug.Log("Game Over"); //Change Later to cutscene or transition all of it to a different script
    }
    #endregion 

    #region SanityBar Timer

    void UpdateTimer()
    {
        if (AnyAnomaliesPresent())
        {
            currentTime -= Time.deltaTime;
            anomalyTimerSlider.value = currentTime;

            if (currentTime <= 0)
            {
                GameOverCut(); // trigger cutscene for game OVER!
            }
        }
    }

    bool AnyAnomaliesPresent()
    {
        foreach (var pair in anomalyPairs)
        {
            if (pair.anomalyObject.activeSelf)
            {
                return true; // Anomaly IS ACtive thus timer ticks
            }
        }

        return false;
    }

    #endregion

    #region RNG Anomaly

    void RandomizeAnomalies()
    {
        //truly rng anomaly placement
        List<Transform> availableLocations = new List<Transform>(spawnLocations);

        foreach ( var pair in anomalyPairs)
        {
            Transform spawnLocation = pair.specificSpawnLocation != null ? pair.specificSpawnLocation : GetRandomSpawnLocation(ref availableLocations); //new addition

            pair.anomalyObject.transform.position = spawnLocation.position;
            pair.anomalyObject.transform.rotation = spawnLocation.rotation;

            //Enable and disable counterparts

            pair.anomalyObject.SetActive(true);
            pair.normalObject.SetActive(false);

        }
    }

    Transform GetRandomSpawnLocation(ref List<Transform> availableLocations)
    {
        // new RNG to spawn anomalies randomly
        int randomIndex = Random.Range(0, availableLocations.Count);
        Transform chosenLocation = availableLocations[randomIndex];
        availableLocations.RemoveAt(randomIndex);
        return chosenLocation;
    }

    #endregion

    void ShootRay()
    {
        Ray ray = new Ray(rightController.position, rightController.forward);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, rayLength, layerMask))
        {
            //anomaly track
            bool foundAnomalyTag = false;

            foreach (var pair in anomalyPairs)
            {
                if (hit.collider.CompareTag(Tag) && hit.collider.gameObject == pair.anomalyObject)
                {              
                        Debug.Log("Scanned anomaly" + pair.anomalyObject.name);

                        //replace with non anomaly counterpart 
                        pair.anomalyObject.SetActive(false);
                        pair.normalObject.transform.position = pair.anomalyObject.transform.position;
                        pair.normalObject.transform.rotation = pair.anomalyObject.transform.rotation;
                        pair.normalObject.SetActive(true);

                        // Refill anomaly bar by adding extra time for each scanned anomaly
                        currentTime = Mathf.Min(currentTime + 5f, maxTime);
                        anomalyTimerSlider.value = currentTime;

                        foundAnomalyTag = true; //AnomalyTag scanned
                        break; //exit loop after scanning 
                    
                }
            }

            if (!foundAnomalyTag)
            {
                NonAnomalyScanPunish();
                Debug.Log("Non anomaly scanned" + hit.collider.gameObject.name);
            }
        }
    }
    
    //Punishment for scanning wrong object
    void NonAnomalyScanPunish()
    {
        // deduct time for non anomaly scan
        float deductionAmount = 5f;
        currentTime = Mathf.Max(currentTime - deductionAmount, 0); //prevent going below 0
        anomalyTimerSlider.value = currentTime;

        if (currentTime <= 0)
        {
            //gameOVer
            GameOverCut(); //Trigger cutscene for Game OVER!!
        }
    }

}