using UnityEngine;

public class SanityEffect : MonoBehaviour
{
    public Material sanityMaterial;
    [Range(0, 1)] public float sanityLevel = 1.0f;

    void Update()
    {
        sanityMaterial.SetFloat("_Sanity", sanityLevel);
    }
}