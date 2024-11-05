using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    public Transform cameraTransform;
    [SerializeField]
    public Vector3 cameraOffset;
    [SerializeField]
    public float turnBuffer = 30f;
    [SerializeField]
    public float turnSpeed = 0.03f;
    [SerializeField]
    public bool lockPitch = false;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = cameraTransform.localPosition + cameraOffset;
        Vector3 cameraEuler = cameraTransform.rotation.eulerAngles;
        cameraEuler.z = 0f;

        if(lockPitch)
        {
            cameraEuler.x = 0f;
        }

        Quaternion targetQuat = Quaternion.Euler(cameraEuler);
        float angle = Quaternion.Angle(transform.rotation, targetQuat);
        
        if(angle > turnBuffer)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQuat, turnSpeed);
        }
    }
}
