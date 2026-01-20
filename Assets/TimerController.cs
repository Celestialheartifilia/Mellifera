using UnityEngine;
using UnityEngine.UI;
using TMPro; // Ensure this is at the top!

public class TimerController : MonoBehaviour
{
    public float StartTime = 10f;
    public float TimeLeft;

    public TMP_Text TimerText;
    public Button StartButton;

    private bool isTimerRunning = false;

    void Update()
    {
        if (isTimerRunning)
        {
            // FIX 1: Check "TimeLeft" (the float), not "TimerText" (the UI)
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;

                // 1. Calculate minutes and seconds
                float minutes = Mathf.FloorToInt(TimeLeft / 60);
                float seconds = Mathf.FloorToInt(TimeLeft % 60);

                // 2. Format the string as "00:00"
                // "D2" means Decimal with 2 digits (adds a leading zero)
                TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                StopTimer();
            }
        }
    }

    public void StartClicked()
    {
        TimeLeft = StartTime;
        isTimerRunning = true;
        StartButton.gameObject.SetActive(false);
        TimerText.gameObject.SetActive(true);
    }

    private void StopTimer()
    {
        isTimerRunning = false;
        TimeLeft = 0;
        StartButton.gameObject.SetActive(true);
        TimerText.gameObject.SetActive(false);
    }
}
