using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SanityShader : MonoBehaviour
{
    public Material sanityShaderMat;
    public Slider sanitySlider;
    
    private void Update()
    {
        float sanity = sanitySlider.value;

        sanityShaderMat.SetFloat("_VignetteIntensity", Mathf.Lerp(0.2f, 1.0f, 1 - sanity / 100));
        sanityShaderMat.SetFloat("_GrainIntensity", sanity <= 15 ? Mathf.Lerp(0, 0.5f, 1 - sanity / 15) : 0);

        sanityShaderMat.SetFloat("_MaskRadius", sanity <= 5 ? Mathf.Lerp(1, 0.1f, 1 - sanity / 5) : 1);// tune later
    }
}
