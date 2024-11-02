using UnityEngine;
using UnityEngine.Events;

public class EnablePlaySoundDisable : MonoBehaviour
{
    public AudioSource audioSource;         // The audio source to play the sound
    public GameObject objectToEnable;       // The object to enable after the delay
    public float delayTime = 6.5f;          // Time to wait before executing the action (in seconds)
    public UnityEvent onActionComplete;      // Action to execute after the delay

    private void OnEnable()
    {
        if (audioSource != null && objectToEnable != null)
        {
            audioSource.Play();
            StartCoroutine(WaitAndExecuteAction());
        }
        else
        {
            Debug.LogWarning("AudioSource or objectToEnable not assigned.");
        }
    }

    private System.Collections.IEnumerator WaitAndExecuteAction()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        // Execute the specified action
        onActionComplete.Invoke();
    }
}
