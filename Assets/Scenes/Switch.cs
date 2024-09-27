using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    public Button button;
    public GameObject clock1;
    public GameObject clock2;

    // Update is called once per frame
    void Start() {
         clock1.SetActive(true);
        }

    void Update()
    {
       
        if (clock1.activeSelf){
            clock2.SetActive(true);
            clock1.SetActive(false);
            } else if (!clock1.activeSelf){
                clock2.SetActive(false);
                clock1.SetActive(true);
            }
    }
}
