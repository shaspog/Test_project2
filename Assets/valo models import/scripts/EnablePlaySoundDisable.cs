using UnityEngine;

public class EnablePlaySoundDisable : MonoBehaviour
{
    public AudioSource audioSource;     // The audio source to play the sound
    public GameObject objectToEnable;   // The object to enable after the sound is finished

    private void OnEnable()
    {
        if (audioSource != null && objectToEnable != null)
        {
            audioSource.Play();
            StartCoroutine(WaitForSoundAndDisable());
        }
        else
        {
            Debug.LogWarning("AudioSource or objectToEnable not assigned.");
        }
    }

    private System.Collections.IEnumerator WaitForSoundAndDisable()
    {
        // Wait for the audio to finish
        yield return new WaitForSeconds(audioSource.clip.length);

        // Disable this object and enable the other one
        objectToEnable.SetActive(true);
        gameObject.SetActive(false);
        
    }
}
