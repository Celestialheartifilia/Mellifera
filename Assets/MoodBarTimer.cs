using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class MoodBarTimer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Timer UI")]
    public Slider timerSlider;
    public Image handleImage; // Drag the "Handle" object from the Slider here
    public float gameTime;

    [Header("Mood Handle Sprites (3)")]
    public Sprite[] moodSprites; // Drag your 3 different mood icons here
    // Array to hold your 3 different Mood UI Panels
    public GameObject[] moodPopups;



    private float timeRemaining;
    private bool stopTimer;
    private int moodIndex = 0; // Tracks which popup to show next

    [Header("Scoring System")]
    // Array to hold 3 honeycombs
    public GameObject[] honeyCombSprites;

    public int scoreValue;

    void Start()
    {
        scoreValue = 10;
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
    {
        if (stopTimer) return;
        //// Subtract the time passed since the last frame
        timeRemaining -= Time.deltaTime;
        float timeElapsed = gameTime - timeRemaining;

        // Trigger 2nd mood at 26.67s score valuex2
        if (moodIndex == 1 && timeElapsed >= 26.67f)
        {
            scoreValue = scoreValue * 2;
            Console.WriteLine("Score times 2");
            TriggerNextMood();
        }

        // Trigger 3rd mood at 53.33s Score value x1
        if (moodIndex == 2 && timeElapsed >= 53.33f)
        {
            scoreValue = scoreValue * 1;
            TriggerNextMood();
        }

        // score nothing
        if (timeRemaining <= 0)
        {
            scoreValue = scoreValue + 0;
            stopTimer = true;
            timeRemaining = 0;
        }

        //else score value x4
        else
        {
            scoreValue = scoreValue * 4;
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