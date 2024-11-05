using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float countdownTime = 600f; // 10 minutes 
    private bool isCountingDown = true;

    void Start()
    {
        UpdateTimerText(countdownTime);
    }

    void Update()
    {
        if (isCountingDown && countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            UpdateTimerText(countdownTime);

            if (countdownTime <= 0)
            {
                countdownTime = 0;
                isCountingDown = false;
                TimerEnded();
            }
        }
    }

    void UpdateTimerText(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void TimerEnded()
    {
        Debug.Log("Countdown has ended.");
    }
}
