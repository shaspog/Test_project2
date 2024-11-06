using UnityEngine;

public class Clock : MonoBehaviour
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

        if (clockHourHandTransform == null || clockMinuteHandTransform == null)
        {
            Debug.LogError("Hour or minute hand not found in the clock hierarchy.");
        }
    }

    private void Update()
    {
        // Update the in-game time
        day += Time.deltaTime / REAL_SECONDS_PER_INGAME_DAY;

        // Normalize the day value to a 0-1 range
        float dayNormalized = day % 1f;

        // Calculate the rotation for the hour and minute hands
        float rotationDegreesPerDay = 360f;
        float hoursPerDay = 24f;

        if (clockHourHandTransform != null)
        {
            clockHourHandTransform.localEulerAngles = new Vector3(0, 0, -dayNormalized * rotationDegreesPerDay);
        }

        if (clockMinuteHandTransform != null)
        {
            clockMinuteHandTransform.localEulerAngles = new Vector3(0, 0, -dayNormalized * rotationDegreesPerDay * hoursPerDay);
        }
    }
}
