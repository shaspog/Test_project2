using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityShader : MonoBehaviour
{
    public Material sanityShaderMat;
    public Slider sanitySlider;
    public RenderTexture renderTexture; // Add this line

    private void Start()
    {
        // Ensure the camera has the render texture assigned
        Camera.main.targetTexture = renderTexture;

        // Assign the Render Texture to the shader's _MainTex
        sanityShaderMat.SetTexture("_MainTex", renderTexture);
    }

    private void Update()
    {
        float sanity = sanitySlider.value;

        // Adjust shader properties based on sanity level
        sanityShaderMat.SetFloat("_VignetteIntensity", Mathf.Lerp(0.2f, 1.0f, 1 - sanity / 100));
        sanityShaderMat.SetFloat("_GrainIntensity", sanity <= 15 ? Mathf.Lerp(0, 0.5f, 1 - sanity / 15) : 0);
        sanityShaderMat.SetFloat("_MaskRadius", sanity <= 5 ? Mathf.Lerp(1, 0.1f, 1 - sanity / 5) : 1);
    }
}
