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

    [System.Serializable]
    public class DifficultyLevel
    {
        public List<AnomalyPair> anomalyPairs; //list for new pairs
        public int maxActiveAnomalies; //max allowed
    }

    // Anomaly Pair List
    public List<AnomalyPair> anomalyPairs;
    // Difficulty Level List
    public List<DifficultyLevel> difficultyLevels;
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

    // Difficulty Elements 
    public float difficultyIncreaseSpeed = 40f;
    private float difficultyTimer = 0f;
    private int currentDifficultyIndex = 0;


    //UI Elements
    public Slider anomalyTimerSlider;
    public float maxTime = 100f;
    private float currentTime;

    //Cooldown for scanner
    public float scanCooldown = 3f;
    private float lastScanTime;

    //ScanShader reference
    public Material ScanMaterial;


    //Audio elements
    [Header("Audio")]
    public AudioClip successScanClip;
    public AudioClip failedScanClip;
    public AudioClip scanningScanClip;

    private AudioSource successScanSource;
    private AudioSource failedScanSource;
    private AudioSource scanningScanSource;

    public GameObject gameOverScreen;


    private void Start()
    {
        if (difficultyLevels.Count > 0)
        {
            RandomizeAnomalies(difficultyLevels[0].anomalyPairs);
        }
        //Timer parts
        currentTime = maxTime;
        anomalyTimerSlider.maxValue = maxTime;
        anomalyTimerSlider.value = maxTime;
        //ScannerCooldown
        lastScanTime = scanCooldown; // scan from the get-go
        //Difficulty timer 
        difficultyTimer = 0f;

        //Spawn random anomalies
        StartCoroutine(SpawnAnomalies());

        successScanSource = gameObject.AddComponent<AudioSource>();
        successScanSource.clip = successScanClip;

        failedScanSource = gameObject.AddComponent<AudioSource>();
        failedScanSource.clip = failedScanClip;

        scanningScanSource = gameObject.AddComponent<AudioSource>();
        scanningScanSource.clip = scanningScanClip;
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

        if (scanningScanSource.isPlaying && Time.time >= lastScanTime + scanCooldown)
        {
            scanningScanSource.Stop();
        }

        //Update Timer Function
        UpdateTimer();

        //Update Difficulty Timer
        UpdateDifficulty();
    }

    #region GameOver UI Elements

    void GameOverCut()
    {
        Debug.Log("Game Over"); //Change Later to cutscene or transition all of it to a different script
        gameOverScreen.SetActive(true);
    }
    #endregion 

    #region SanityBar Timer

    void UpdateTimer()
    {
        

        int activeAnomalies = CurrentlyActiveAnomalies();

        int anomaliesLimit = 3;
        int timeInSeconds = Mathf.FloorToInt(currentTime);
        int addedAnomalies = (timeInSeconds / 10) * 2;
        anomaliesLimit += addedAnomalies;

        int maxAnomaliesAllowed = Mathf.Min(anomaliesLimit, difficultyLevels[currentDifficultyIndex].maxActiveAnomalies);


        if (activeAnomalies > 0)
        {
            currentTime -= Time.deltaTime * activeAnomalies;
            anomalyTimerSlider.value = currentTime;

            if (currentTime <= 0)
            {
                GameOverCut(); // trigger cutscene for game OVER!
            }
        }
    }

    int CurrentlyActiveAnomalies()
    {
        int count = 0; //starts the game with 0 count 

        foreach (var pair in anomalyPairs)
        {
            if (pair.anomalyObject.activeSelf)
            {
                count++; //count = count + 1
            }
        }

        if (currentDifficultyIndex < difficultyLevels.Count)
        {
            foreach (var pair in difficultyLevels[currentDifficultyIndex].anomalyPairs)
            {
                if (pair.anomalyObject.activeSelf)
                {
                    count++;
                }
            }
        }

        return count; //return count as total active anomalies
    }

    #endregion

    #region RNG Anomaly

    void RandomizeAnomalies(List<AnomalyPair> pairs)
    {
        if (pairs.Count == 0 || spawnLocations.Count == 0) return; //making sure list stays full!!

        int maxSpawnCount = Mathf.Min(difficultyLevels[currentDifficultyIndex].maxActiveAnomalies, pairs.Count);

        //truly rng anomaly placement
        List<Transform> availableLocations = new List<Transform>(spawnLocations);
      


        ShuffleList(pairs);

        for (int I = 0; I < maxSpawnCount; I++)
        {
            AnomalyPair pair = pairs[I];
            Transform spawnLocation = pair.specificSpawnLocation != null ? pair.specificSpawnLocation : GetRandomSpawnLocation(ref availableLocations);

            pair.anomalyObject.transform.position = spawnLocation.position;
            pair.anomalyObject.transform.rotation = spawnLocation.rotation;

            //Enable and disable counterparts

            pair.anomalyObject.SetActive(true);
            pair.normalObject.SetActive(false);

        }

        if (maxSpawnCount >= pairs.Count)
        {
            ShuffleList(pairs);
        }
    }

    private void ShuffleList(List<AnomalyPair> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            AnomalyPair temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
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

     IEnumerator AnomalyDelayTime(float delayTime)
    {
        //delay of time
        yield return new WaitForSeconds(delayTime);
    }

    void DelayAnomaly(float delayTime)
    {
        StartCoroutine(AnomalyDelayTime(delayTime));
    }

    IEnumerator SpawnAnomalies()
    {
        while (true) //indefinete loop
        {
            RandomizeAnomalies(anomalyPairs);
            yield return new WaitForSeconds(2f);

            if (currentDifficultyIndex < difficultyLevels.Count)
            {
                // Spawn Difficult Anomalies!
                RandomizeAnomalies(difficultyLevels[currentDifficultyIndex].anomalyPairs);
                yield return new WaitForSeconds(40f);
            }
            else
            {
                currentDifficultyIndex = 0; // reset to cycle 
            }
        }
    }

    #endregion

    #region Difficulty
    void UpdateDifficulty()
    {
        difficultyTimer += Time.deltaTime;

        if (difficultyTimer >= difficultyIncreaseSpeed)
        {
            IncreaseDifficulty();
            Debug.Log("Difficulty UPPed! Watch Out Gamers");
            difficultyTimer = 0f; //resets the timer and starts counting down again till next difficulty
        }
    }

    void IncreaseDifficulty()
    {
        if (currentDifficultyIndex < difficultyLevels.Count - 1)
        {
            currentDifficultyIndex++;
            RandomizeAnomalies(difficultyLevels[currentDifficultyIndex].anomalyPairs);
        }
    }

    #endregion

    void ShootRay()
    {
        Ray ray = new Ray(rightController.position, rightController.forward);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, rayLength, layerMask))
        {
            //Apply shader mat 
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
            if (hitRenderer != null)
            {
                Material originalMaterial = hitRenderer.material;
                hitRenderer.material = ScanMaterial;
                StartCoroutine(ScanShader(hitRenderer, originalMaterial, hit.collider.gameObject));
            }      
        }
    }

    //coroutine to keep shader effect the same length as cooldown
    IEnumerator ScanShader(Renderer targetRenderer, Material originalMaterial, GameObject hitObject)
    {
        scanningScanSource.Play();

        yield return new WaitForSeconds(scanCooldown);
        targetRenderer.material = originalMaterial;

        AnomalyScan(hitObject);
    }
    
    void AnomalyScan(GameObject hitObject)
    {
        //anomaly track
        bool foundAnomalyTag = false;

        foreach (var pair in anomalyPairs)
        {
            if (hitObject.CompareTag(Tag) && hitObject == pair.anomalyObject)
            {
                ProcAnomalyScan(pair);
                foundAnomalyTag = true; //AnomalyTag scanned
                break; //exit loop after scanning 
            }
        }


        if (!foundAnomalyTag && currentDifficultyIndex < difficultyLevels.Count)
        {
            foreach (var pair in difficultyLevels[currentDifficultyIndex].anomalyPairs)
            {
                if (hitObject.CompareTag(Tag) && hitObject == pair.anomalyObject)
                {
                    ProcAnomalyScan(pair);
                    foundAnomalyTag = true;
                    break;
                }
            }
        }

        if (!foundAnomalyTag)
        {
            /*play unsuccessful scan audio
            falseScan.Play();
            */

            NonAnomalyScanPunish();
            Debug.Log("Non anomaly scanned" + hitObject.name);
        }
    }

    void ProcAnomalyScan(AnomalyPair pair)
    {
        Debug.Log("Scanned anomaly" + pair.anomalyObject.name);
      
        successScanSource.Play();
        
        //replace with non anomaly counterpart 
        pair.anomalyObject.SetActive(false);
        pair.normalObject.transform.position = pair.anomalyObject.transform.position;
        pair.normalObject.transform.rotation = pair.anomalyObject.transform.rotation;
        pair.normalObject.SetActive(true);

        // Refill anomaly bar by adding extra time for each scanned anomaly
        currentTime = Mathf.Min(currentTime + 5f, maxTime);
        anomalyTimerSlider.value = currentTime;
    }

    //Punishment for scanning wrong object
    void NonAnomalyScanPunish()
    {
        // deduct time for non anomaly scan
        float deductionAmount = 5f;
        currentTime = Mathf.Max(currentTime - deductionAmount, 0); //prevent going below 0
        anomalyTimerSlider.value = currentTime;
        failedScanSource.Play();

        if (currentTime <= 0)
        {
            //gameOVer
            GameOverCut(); //Trigger cutscene for Game OVER!!
        }
    }


}