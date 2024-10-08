using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARtoVR : MonoBehaviour

{
    public CanvasGroup EyeClosingCanvas;
    public EyesClosing EyesClosing;
    public void SetTransparency(float alpha)
    {
        EyeClosingCanvas.alpha = Mathf.Clamp01(alpha); // Clamps the value between 0 and 1
    }
    private void Start()
    {
        EyeClosingCanvas.alpha = 0f;
    }
    void OnTriggerEnter(Collider Headboundary)
    {
     
            // Set the alpha (transparency) to 0 (fully transparent)
        EyeClosingCanvas.alpha = 1f;
        
        StartCoroutine(ARtoVRdelay());
    }

    IEnumerator ARtoVRdelay()
    {
        yield return new WaitForSeconds(1f);

        // Close the eyes first
        EyesClosing.CloseEyes();

        // Wait for the delay (e.g., 5 seconds)
        yield return new WaitForSeconds(5f);

        // Check if the eyes are closed before switching the scene
        if (EyesClosing.isClosed)
        {
            SceneManager.LoadScene(0);
        }
    }
}
