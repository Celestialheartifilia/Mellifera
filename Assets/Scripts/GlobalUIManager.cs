using TMPro;
using UnityEngine;

public class GlobalUIManager : MonoBehaviour
{
    //for the consistent UI
    //menu



    //timer
    [Header("Timer (4 minutes)")]
    public float timeRemaining = 240f;   // 4 mins
    public TMP_Text timerText;

    bool timerRunning;

    void Awake()
    {
        // Start timer immediately when scene loads
        timerRunning = true;
        UpdateTimerDisplay(timeRemaining); // show 04:00 instantly
    }

    void Update()
    {
        if (!timerRunning) return;

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay(timeRemaining);
        }
        else
        {
            timeRemaining = 0f;
            timerRunning = false;
            UpdateTimerDisplay(0f);

            Debug.Log("Time's up!");
            // call game over here if needed
        }
    }

    void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Optional controls if you ever need them
    public void PauseTimer() => timerRunning = false;
    public void ResumeTimer() => timerRunning = true;
    public void ResetTimer()
    {
        timeRemaining = 240f;
        timerRunning = true;
    }
}
