using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int SelectedTime;
    public bool IsTimePicked;
    public bool IsInSession;
    public bool IsSessionSuccessful;
    private TimeSpan t;
    private float timePassed;

    public TextMeshProUGUI Timer;
    public GameObject TimeButtons;
    public GameObject Instruction;
    public GameObject SessionFailed;
    public GameObject SessionSuccessful;
    public GameObject StartScreen;
    public GameObject Session;

    public GameObject DrawManager;

    private float currentTime;
    [SerializeField] private int _money;
    [SerializeField] private TextMeshProUGUI _moneyText;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Money"))
        {
            _money = 10;
        }

        _moneyText.text = _money.ToString();
    }
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
            StartScreen.SetActive(false);
            t = TimeSpan.FromSeconds(SelectedTime);
            timePassed = 0;
            IsSessionSuccessful = false;
        }
    }

    private void Update()
    {
        if (IsInSession)
        {
            timePassed = timePassed + Time.deltaTime;
            currentTime = currentTime - Time.deltaTime;
            t = TimeSpan.FromSeconds(currentTime);

            Timer.text = t.ToString(@"hh\:mm\:ss");

            if(currentTime <= 0)
            {
                IsInSession = false;
                IsSessionSuccessful = true;
                SessionSuccessful.SetActive(true);
                Session.SetActive(false);
                AddMoney();
            }
        }
    }

    public void AddMoney()
    {
        switch (currentTime)
        {
            case float value when value <= 0:
                _money += 50;
                PlayerPrefs.SetInt("Money", _money);

                break;
            case float value when value >= 0:
                if (timePassed >= 600)
                {

                }
                break;

        }
    }
    public void StartDrawing()
    {
        SceneManager.LoadScene(1);
    }
    public void CheckFish()
    {
        SceneManager.LoadScene(2);
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
