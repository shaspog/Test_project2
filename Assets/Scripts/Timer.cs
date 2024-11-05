using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI invertedTimerText; 
    public GameObject anomalyClock; 
    public float countdownTime = 600f; // 10 minutes 
    private bool isCountingDown = true;

    void Start()
    {
        UpdateTimerText(countdownTime);
        invertedTimerText.gameObject.SetActive(false); 
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

        if (anomalyClock.activeSelf)
        {
            timerText.gameObject.SetActive(false); 
            invertedTimerText.gameObject.SetActive(true); 
            UpdateInvertedTimerText(countdownTime);
        }
        else
        {
            timerText.gameObject.SetActive(true); 
            invertedTimerText.gameObject.SetActive(false); 
        }
    }

    void UpdateTimerText(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void UpdateInvertedTimerText(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        invertedTimerText.text = $"{minutes:00}:{seconds:00}"; 
    }

    void TimerEnded()
    {
        Debug.Log("Countdown has ended.");
    }
}
