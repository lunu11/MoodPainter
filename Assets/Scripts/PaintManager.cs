using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PaintManager : MonoBehaviour
{
    [SerializeField] private GameObject _colorPicker;
    [SerializeField] private DrawDemo _drawDemo;
    [SerializeField] private Image _canvasColor;
    [SerializeField] private GameObject _intro;
    [SerializeField] private GameObject _submitBtn;
    [SerializeField] private GameObject _finish;
    [SerializeField] private TankSO _tankSO;

    private Image selectedButton;
    private int _money;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _fishStatus;


    private void Start()
    {
        if (PlayerPrefs.HasKey("Reminder"))
        {
            if(PlayerPrefs.GetInt("Reminder") == 1)
            {
                _intro.SetActive(false);
                _drawDemo.enabled = true;
            }
        }
        _money = PlayerPrefs.GetInt("Money");
        _moneyText.text = "Money: " + _money.ToString() + "g";
        _fishStatus.text = "Fish Status: " + _tankSO.FishStatus;
    }
    public void StartPainting()
    {
        _drawDemo.enabled = true;
        _submitBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Next Step";
    }

    public void NextStep()
    {
        if (_drawDemo.Step == 1) 
        {
            _drawDemo.Step = 2;
            _submitBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Finish";
        }
        else Finish();       
    }
    public void ButtonSelected(string button)
    {
        switch(button)
        {
            case "MM":
                SceneManager.LoadScene(0);
                break;
            case "Fish":
                SceneManager.LoadScene(2);
                break;
        }
    }

    private void Finish()
    {
        _finish.SetActive(true);
        _money += 50;
        PlayerPrefs.SetInt("Money", _money);
        _moneyText.text = _money.ToString();
    }

    public void DontRemindToggle(bool value)
    {
        PlayerPrefs.SetInt("Reminder",value ? 1 : 0);
    }
    public void CheckColorPicker()
    {
        if (_colorPicker.activeSelf)
        {
            _colorPicker.SetActive(false);
            _drawDemo.enabled = true;
        }
    }

    public void ChangeColor(Color color)
    {
        selectedButton.color = color;
        _drawDemo.ChangeColor(selectedButton.color);
    }

    public void ColorBtnSelected(Image button)
    {
        selectedButton = button;
        _colorPicker.GetComponent<FlexibleColorPicker>().startingColor = button.color;
        _colorPicker.GetComponent<FlexibleColorPicker>().color = button.color;
        _drawDemo.ChangeColor(button.color);
        _drawDemo.enabled = false;
        _colorPicker.SetActive(true);
    }
    public void CanvasButtonSelected(Image button)
    {
        _colorPicker.GetComponent<FlexibleColorPicker>().startingColor = button.color;
        _colorPicker.GetComponent<FlexibleColorPicker>().color = button.color;
        _colorPicker.SetActive(true);
    }
    public void ChangeCanvasColor(Color color)
    {
        _canvasColor.color = color;
    }
}
