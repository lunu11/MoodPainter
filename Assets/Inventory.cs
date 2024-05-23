using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemCollection _itemCollection;
    [SerializeField] private TankSO _tankSO;
    [SerializeField] private Save _save;
    [SerializeField] private GameObject _itemList;

    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private GameObject _newFish;
    [SerializeField] private GameObject _popup;
    [SerializeField] private AquariumManager _aquariumManager;

    private string _type;

    public void SetItems(string type)
    {
        switch (type)
        {
            case "Fish":
                _type = "Fish";
                _newFish.SetActive(true);
                _title.text = "Choose Fish";
                for (int i = 0; i < _itemCollection.Fishes.Count; i++)
                {
                    GameObject item = _itemList.transform.GetChild(i).gameObject;
                    if (!_save.Fishes[i]) continue;
                    if (!item.activeSelf)
                    {
                        item.SetActive(true);
                        item.GetComponent<ItemSlot>().Fish = _itemCollection.Fishes[i];
                        item.transform.GetChild(0).GetComponent<Image>().sprite = _itemCollection.Fishes[i].Sprite;
                    }
                }
                break;
            case "Decor":
                _type = "Decor";
                _newFish.SetActive(false);
                _title.text = "Choose Decor";
                for (int i = 0; i < _itemCollection.Decor.Count; i++)
                {
                    GameObject item = _itemList.transform.GetChild(i).gameObject;
                    if (!_save.Decor[i]) continue;
                    if (!item.activeSelf)
                    {
                        item.SetActive(true);
                        item.GetComponent<ItemSlot>().Decor = _itemCollection.Decor[i];
                        item.transform.GetChild(0).GetComponent<Image>().sprite = _itemCollection.Decor[i].Sprites[i];
                    }
                }
                break;

        }
        _popup.SetActive(false);
    }
    public void Selected(ItemSlot item)
    {
        if (_type == "Fish")
        {
            if (!_tankSO.IsFishMatured)
            {
                _popup.SetActive(true);
            }
            else
            {
                _aquariumManager.ChangeFish(item.Fish);
                gameObject.transform.parent.gameObject.SetActive(false);
            }
        } else
        {
            _aquariumManager.StartBuildMode(item.Decor);
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        ClearItems();                                                  
    }
    private void ClearItems()
    {
        {
            for (int i = 0; i < _itemList.transform.childCount; i++)
            {
                GameObject item = _itemList.transform.GetChild(i).gameObject;
                if (item.activeSelf)
                {
                    item.SetActive(false);
                    item.GetComponent<ItemSlot>().Decor = null;
                    item.GetComponent<ItemSlot>().Fish = null;
                }
            }
        }
    }
}
