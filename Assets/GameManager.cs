using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int SelectedTime;
    public bool IsTimePicked;
    public bool IsInSession;
    public bool IsSessionSuccessful;
    private TimeSpan t;

    public TextMeshProUGUI Timer;
    public GameObject TimeButtons;
    public GameObject Instruction;
    public GameObject SessionFailed;
    public GameObject SessionSuccessful;
    public GameObject StartScreen;
    public GameObject Session;

    private float currentTime;

    public void SetTime(int time)
    {
        SelectedTime = time;
        IsTimePicked = true;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void StartSession()
    {
        if (IsTimePicked)
        {
            IsInSession = true;
            TimeButtons.SetActive(false);
            Timer.transform.parent.gameObject.SetActive(true);
            currentTime = SelectedTime;
            Timer.gameObject.SetActive(true);
            t = TimeSpan.FromSeconds(SelectedTime);
        }
    }

    private void Update()
    {
        if (IsInSession)
        {
            currentTime = currentTime - Time.deltaTime;
            t = TimeSpan.FromSeconds(currentTime);

            Timer.text = t.ToString(@"hh\:mm\:ss");

            if(currentTime <= 0)
            {
                IsInSession = false;
                SessionSuccessful.SetActive(true);
                Session.SetActive(false);
            }
        }
    }

    private void OnApplicationFocus(bool status)
    {
        if (!status)
        {
            if (IsInSession)
            {
                IsSessionSuccessful = false;
                IsInSession = false;

                SessionFailed.SetActive(true);
                Session.SetActive(false);
            }
        }
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            if (IsInSession)
            {
                IsSessionSuccessful = false;
                IsInSession = false;

                SessionFailed.SetActive(true);
                Session.SetActive(false);
            }
        }
    }
}
