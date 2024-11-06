using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ARtoVR : MonoBehaviour

{
    public EyesClosing EyesClosing;


    void OnTriggerEnter(Collider Headboundary)
    {
        StartCoroutine(ARtoVRdelay());
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            // Disable the MeshRenderer to make the object invisible
            meshRenderer.enabled = false;
        }
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
            // Disable VR before loading the scene
            XRSettings.enabled = false;

            // Load the new scene
            SceneManager.LoadScene("Graybox");

            // Wait a moment for the new scene to load
            yield return new WaitForSeconds(0.5f);

            // Re-enable VR after the new scene is loaded
            XRSettings.enabled = true;
        }
    }
}
