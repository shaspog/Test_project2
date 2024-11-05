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

    
    private void Awake()
    {
        // Find the hour and minute hands within the clock's hierarchy
        clockHourHandTransform = transform.Find("hourHand");
        clockMinuteHandTransform = transform.Find("minuteHand");   
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
}
