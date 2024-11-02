using UnityEngine;

public class SettingsLoader : MonoBehaviour
{
    public GameObject arrowObject; // Reference to the arrow in the gameplay scene

    private void Start()
    {
        // Load saved states
        bool isAudioOn = PlayerPrefs.GetInt("AudioEnabled", 1) == 1;
        bool isArrowOn = PlayerPrefs.GetInt("ArrowEnabled", 1) == 1;

        // Apply the audio state
        AudioListener.volume = isAudioOn ? 1.0f : 0.0f;

        // Enable or disable the arrow object based on the saved state
        arrowObject.SetActive(isArrowOn);
    }
}
