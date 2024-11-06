using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class clock : MonoBehaviour
{
    private const float REAL_SECONDS_PER_INGAME_DAY = 60f;

    private Transform clockHourHandTransform;
    private Transform clockMinuteHandTransform;
    private float day;

    private Rigidbody clockRigidbody;  // Reference to the clock's Rigidbody
    private XRGrabInteractable grabInteractable;  // Reference to the XR Grab Interactable component

    private void Awake()
    {
        // Find the hour and minute hands within the clock's hierarchy
        clockHourHandTransform = transform.Find("hourHand");
        clockMinuteHandTransform = transform.Find("minuteHand");

        // Get the Rigidbody component attached to the clock
        clockRigidbody = GetComponent<Rigidbody>();

        // Get the XRGrabInteractable component
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to the grab and release events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void Update()
    {
        // Update the in-game time, regardless of whether the clock is held or not
        day += Time.deltaTime / REAL_SECONDS_PER_INGAME_DAY;

        // Normalize the day value to a 0-1 range
        float dayNormalized = day % 1f;

        // Calculate the rotation for the hour and minute hands
        float rotationDegreesPerDay = 360f;
        float hoursPerDay = 24f;

        // Apply the rotation to the hour and minute hands relative to the clock's local space
        clockHourHandTransform.localEulerAngles = new Vector3(0, 0, -dayNormalized * rotationDegreesPerDay);
        clockMinuteHandTransform.localEulerAngles = new Vector3(0, 0, -dayNormalized * rotationDegreesPerDay * hoursPerDay);
    }

    // Called when the clock is grabbed
    private void OnGrab(SelectEnterEventArgs args)
    {
        // Enable gravity when the clock is grabbed
        if (clockRigidbody != null)
        {
            clockRigidbody.useGravity = true;
        }
    }

    // Called when the clock is released
    private void OnRelease(SelectExitEventArgs args)
    {
        // Ensure gravity remains enabled after release
        if (clockRigidbody != null)
        {
            clockRigidbody.useGravity = true;  // Force gravity to remain enabled
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to avoid memory leaks
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}
