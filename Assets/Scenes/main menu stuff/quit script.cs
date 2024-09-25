using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quitscript : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnApplicationQuit()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }
}
