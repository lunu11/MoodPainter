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

    private float currentTime;
    [SerializeField] private int _money;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _fishStatus;
    [SerializeField] private TextMeshProUGUI _successText;
    [SerializeField] private TankSO _tankSO;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Money"))
        {
            _money = 100;
            PlayerPrefs.SetInt("Money", _money);
        }
        _money = PlayerPrefs.GetInt("Money");
        _moneyText.text = "Money: " + _money.ToString() + "g";
        _fishStatus.text = "Fish Status: " + _tankSO.FishStatus;
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
        float multiplier = SelectedTime / 600;
        
        switch (currentTime)
        {
            case float value when value <= 0:
                _money += Mathf.RoundToInt(50 * multiplier);
                PlayerPrefs.SetInt("Money", _money);
                _successText.text = "Well done! <br><br>You've received " + _money + "g";
                break;
            case float value when value >= 0:
                if (timePassed >= 600)
                {
                    multiplier = timePassed / 600;
                    _money += Mathf.RoundToInt(50 * multiplier);
                    PlayerPrefs.SetInt("Money", _money);
                    _successText.text = "Well done! <br><br>You've received " + _money + "g";
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
    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
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
