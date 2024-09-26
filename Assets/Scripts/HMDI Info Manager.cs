using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMDIInfoManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Is Device Active " + XRSettings.isDeviceActive);
        Debug.Log("Device Name is : " + XRSettings.loadedDeviceName);

        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("No Headset Plugged");
        }
        else if (XRSettings.isDeviceActive && XRSettings.loadedDeviceName == "Moch HMD"
            || XRSettings.loadedDeviceName == "MockHMDDisplay")
        {
            Debug.Log("Using Mock HMD");
        }
        else
        {
            Debug.Log("We Have a headset" + XRSettings.loadedDeviceName);
        }
    }

}


