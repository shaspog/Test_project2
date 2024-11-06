using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesOpenOnSceneEnter : MonoBehaviour
{
    public CanvasGroup EyeClosingCanvas;
    public EyesClosing EyesClosing;

    public void SetTransparency(float alpha)
    {
        EyeClosingCanvas.alpha = Mathf.Clamp01(alpha); // Clamps the value between 0 and 1
    }
    // Start is called before the first frame update
    void Start()
    { 
        EyeClosingCanvas.alpha = 1f;
        EyesClosing.CloseEyes();

        StartCoroutine(AutoOpen());

    }
    IEnumerator AutoOpen()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(1.5f);
        EyesClosing.OpenEyes();
    }
}
