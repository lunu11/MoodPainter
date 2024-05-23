using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AquariumManager : MonoBehaviour
{
    [SerializeField] private TankSO _tankSO;
    [SerializeField] private ItemCollection _itemCollection;
    [SerializeField] private Save _save;

    [SerializeField] private GameObject _fish;
    [SerializeField] private GameObject _sand;
    [SerializeField] private List<GameObject> _decor = new List<GameObject>();
    [SerializeField] private GameObject _inventory;

    [SerializeField] private GameObject _decorPrefab;
    [SerializeField] private GameObject _buildMode;
    [SerializeField] private GameObject _tools;

    [SerializeField] private TextMeshProUGUI _countDown;
    [SerializeField] private TextMeshProUGUI _fishStatus;
    [SerializeField] private TextMeshProUGUI _moneyText;

    private int _money;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            LoadProgress();
            
            for (int i = 0; i < _tankSO.DecorID.Count; i++)
            {
                string id = _tankSO.DecorID[i].ID;
                for (int j = 0; j < _itemCollection.Decor.Count; j++)
                {
                    if (id == _itemCollection.Decor[j].Name)
                    {
                        DecorSO decorSO = _itemCollection.Decor[j];
                        GameObject decorItem = Instantiate(_decorPrefab, _sand.transform);
                        decorItem.transform.localPosition = _tankSO.DecorID[i].Position;
                        decorItem.GetComponent<ItemSlot>().Decor = _itemCollection.Decor[j];
                        decorItem.GetComponent<SpriteRenderer>().sprite = decorSO.Sprites[_tankSO.DecorID[i].SpriteOrder];
                        _decor.Add(decorItem);
                        break;
                    }
                }
            }
            for (int i = 0; i < _itemCollection.Fishes.Count; i++)
            {
                if (_tankSO.FishID == "Guppy") _fish.GetComponent<SpriteRenderer>().sprite = _itemCollection.DefaultGuppy.Sprite;
                if (_tankSO.FishID == _itemCollection.Fishes[i].Name)
                {
                    _fish.GetComponent<SpriteRenderer>().sprite = _itemCollection.Fishes[i].Sprite;
                }
            }

            if (!_tankSO.IsFishMatured && _tankSO.FishID != null)
            {
                _countDown.gameObject.SetActive(true);
                _countDown.text = _tankSO.FishMatureDate.ToString();
                if (DateTime.Now >= _tankSO.FishMatureDate)
                {
                    StartCoroutine(FishMature());
                }
            } else if (_tankSO.IsFishMatured)
            {
                TimeSpan timePassed = (DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("LastPlayTime")));
                TimeSpan remainingHappyDuration = _tankSO.HappyDuration - timePassed;
                _tankSO.HappyDuration = remainingHappyDuration;
                if (remainingHappyDuration.Seconds < 0)
                {
                    //FishHungry
                    _fishStatus.text = "Fish Status: Hungry";
                    _tankSO.FishStatus = "Hungry";
                    
                }

                if (remainingHappyDuration.Seconds < -86400)
                {
                    //FishDead
                    _fishStatus.text = "Fish Status: Dead";
                    _tankSO.FishStatus = "Dead";
                    _tankSO.IsFishAlive = false;
                }
            }
        } else
        {
            SaveProgress();            
        }
        _money = PlayerPrefs.GetInt("Money");
        _moneyText.text = "Money: "+_money.ToString()+"g";
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PlayerPrefs.SetString("LastPlayTime", DateTime.Now.ToString());
            SaveProgress();
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastPlayTime", DateTime.Now.ToString());
        SaveProgress();
    }


    public void SavePositions()
    {
        _tankSO.DecorID.Clear();
        for (int i = 0; i < _decor.Count; i++)
        {
            DecorSO currentDecor = _decor[i].GetComponent<ItemSlot>().Decor;
            _tankSO.DecorID.Add(new Item());
            _tankSO.DecorID[i].ID = currentDecor.Name;
            _tankSO.DecorID[i].Position = _decor[i].transform.localPosition;
        }
        SaveProgress();
    }
    public void RevertPositions()
    {
        for (int i = 0; i < _decor.Count; ++i)
        {
            _decor[i].transform.position = _tankSO.DecorID[i].Position;
        }
    }

    public void StartBuildMode(DecorSO decor)
    {
        if (decor == null)
        {
            _tools.SetActive(false);
            _buildMode.SetActive(true);
        } else
        {
            GameObject decorItem = Instantiate(_decorPrefab, _sand.transform);
            decorItem.GetComponent<SpriteRenderer>().sprite = decor.Sprites[0];
            decorItem.GetComponent<ItemSlot>().Decor = decor;
            _decor.Add(decorItem);
            _tools.SetActive(false);
            _buildMode.SetActive(true);
            _buildMode.GetComponent<BuildManager>()._selectedDecor = decorItem;
            _buildMode.GetComponent<BuildManager>().IsEligible = false;
        }
    }


    public void FeedFish()
    {
        if (_money < 10) return;
        if (_tankSO.IsFishMatured)
        {
            if (_tankSO.HappyDuration.Seconds >= 0)
            {
                _tankSO.HappyDuration += TimeSpan.FromHours(8);
            }
            else
            {
                _tankSO.HappyDuration = TimeSpan.FromHours(8);
            }
           
        }
        else
        {
            _tankSO.FishMatureDate -= TimeSpan.FromHours(8);
            TimeSpan timeSpan = _tankSO.FishMatureDate - DateTime.Now;
            Debug.Log(timeSpan);
            Debug.Log(_tankSO.FishMatureDate);
            if (timeSpan.TotalSeconds <= 0)
            {
                StartCoroutine(FishMature());
            }
            else
            {
                _countDown.gameObject.SetActive(true);
                _countDown.text = _tankSO.FishMatureDate.ToString();
            }
        }
        _fishStatus.text = "Fish Status: Happy";
        _tankSO.FishStatus = "Happy";
        _money -= 10;
        _moneyText.text = "Money: " + _money.ToString() + "g";
        PlayerPrefs.SetInt("Money", _money);
        SaveProgress();
    }
    public void GrowthBooster()
    {
        if (_tankSO.IsFishMatured) return;
        _tankSO.FishMatureDate -= TimeSpan.FromHours(8);
        SaveProgress();
    }

    public void ChangeFish(FishSO fish)
    {
        _tankSO.IsFishMatured = true;
        _tankSO.FishMatureDate = DateTime.MinValue;
        _tankSO.FishID = fish.Name;
        _tankSO.IsFishAlive = true;
        _tankSO.HappyDuration = TimeSpan.Zero;

        _fishStatus.text = "Fish Status: Hungry";
        _fish.GetComponent<SpriteRenderer>().sprite = fish.Sprite;
        SaveProgress();
    }
    public void NewGuppy(FishSO fish)
    {
        if (_money >= 50)
        {
            _money -= 50;
            _moneyText.text = "Money: " + _money.ToString() + "g";
            PlayerPrefs.SetInt("Money", _money);
            _tankSO.IsFishMatured = false;
            DateTime future = DateTime.Now.AddHours(24);
            _tankSO.FishMatureDate = future;
            _tankSO.FishID = fish.Name;
            _tankSO.IsFishAlive = true;

            _fish.GetComponent<SpriteRenderer>().sprite = fish.Sprite;
            SaveProgress();
        }
    }
    public void ReturnHome()
    {
        SaveProgress();
        SceneManager.LoadScene(0);
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetString("Tank", JsonUtility.ToJson(_tankSO));
        PlayerPrefs.SetString("Save", JsonUtility.ToJson(_save));
    }
    public void LoadProgress()
    {
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("Tank"), _tankSO);
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("Save"), _save);
    }

    IEnumerator FishMature()
    {
        yield return new WaitForSeconds(3f);
        int chosenFish = UnityEngine.Random.Range(0, _itemCollection.Fishes.Count);
        if (!_save.Fishes[chosenFish])
        {
            _save.Fishes[chosenFish] = true;
        }
        _fish.GetComponent<SpriteRenderer>().sprite = _itemCollection.Fishes[chosenFish].Sprite;
        _tankSO.FishID = _itemCollection.Fishes[chosenFish].Name;
        _tankSO.IsFishMatured = true;
        _tankSO.FishMatureDate = DateTime.MinValue;
        _countDown.gameObject.SetActive(false);
    }


}
