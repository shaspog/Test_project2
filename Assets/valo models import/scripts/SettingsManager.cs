using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public GameObject arrowObject; // The arrow object that should be enabled/disabled
    private float defaultVolume = 1.0f; // Default volume level

    private void Awake()
    {
        // Make this GameObject persistent across scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Load saved volume and arrow state from PlayerPrefs
        float savedVolume = PlayerPrefs.GetFloat("AudioVolume", defaultVolume);
        bool isArrowOn = PlayerPrefs.GetInt("ArrowEnabled", 0) == 1;

        // Apply saved settings
        AudioListener.volume = savedVolume;
        arrowObject.SetActive(isArrowOn);
    }

    public void SetVolume(float volume)
    {
        // Set the AudioListener volume and save the new volume level
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("AudioVolume", volume);
        PlayerPrefs.Save();
    }

    public void ToggleArrow()
    {
        // Toggle the arrow object state
        bool isArrowCurrentlyOn = arrowObject.activeSelf;
        bool newArrowState = !isArrowCurrentlyOn;

        // Apply the new state and save it
        arrowObject.SetActive(newArrowState);
        PlayerPrefs.SetInt("ArrowEnabled", newArrowState ? 1 : 0);
        PlayerPrefs.Save();
    }
}
