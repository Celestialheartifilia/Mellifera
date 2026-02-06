using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MoodBarTimer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Slider timerSlider;
    public Image handleImage; // Drag the "Handle" object from the Slider here
    public float gameTime;

    public Sprite[] moodSprites; // Drag your 3 different mood icons here
    // Array to hold your 3 different Mood UI Panels
    public GameObject[] moodPopups;

    private float timeRemaining; 
    private bool stopTimer;
    private int moodIndex = 0; // Tracks which popup to show next


    void Start()
    {
        stopTimer = false;
        timeRemaining = gameTime;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;

        // Hide all popups at the start
        foreach (GameObject popup in moodPopups)
        {
            if (popup != null) popup.SetActive(false);
        }
        // Trigger the FIRST mood immediately at start
        TriggerNextMood();
    }

    // Update is called once per frame
    void Update()
    {  if (stopTimer) return;
        //// Subtract the time passed since the last frame
        timeRemaining -= Time.deltaTime;
        float timeElapsed = gameTime - timeRemaining;
        // Trigger 2nd mood at 26.67s
        if (moodIndex == 1 && timeElapsed >= 26.67f)
        {
            TriggerNextMood();
        }

        // Trigger 3rd mood at 53.33s
        if (moodIndex == 2 && timeElapsed >= 53.33f)
        {
            TriggerNextMood();
        }

        if (timeRemaining <= 0)
        {
            stopTimer = true;
            timeRemaining = 0;
        }

        timerSlider.value = timeRemaining;
    }


    void TriggerNextMood()
    {
        if (moodIndex < moodSprites.Length)
        {
            // CHANGE THE HANDLE IMAGE
            if (handleImage != null && moodSprites[moodIndex] != null)
            {
                handleImage.sprite = moodSprites[moodIndex];
            }

            // POPUP LOGIC
            if (moodIndex < moodPopups.Length)
            {
                if (moodIndex > 0) moodPopups[moodIndex - 1].SetActive(false);
                moodPopups[moodIndex].SetActive(true);
            }

            moodIndex++;
        }
    }
    // Call this from your UI Buttons to close whatever mood is open
    public void CloseCurrentMood()
    {
        foreach (GameObject popup in moodPopups)
        {
            popup.SetActive(false);
        }
        // Time.timeScale = 1;
    }
}
