using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    [SerializeField] private ItemCollection _itemCollection;
    [SerializeField] private GameObject _itemList;

    [SerializeField] private GameObject _category;

    private void OnEnable()
    {
        SetItems();
    }

    private void OnDisable()
    {
        ClearItems();
    }

    public void ChangeCategory(GameObject obj)
    {
        _category.GetComponent<Image>().color = new Color(0.2509804f, 0.5921569f, 0.9176471f);
        _category = obj;
        obj.GetComponent<Image>().color = new Color(0.3333334f, 0.4358902f, 1);
        ClearItems();
        SetItems();
    }

    private void SetItems()
    {
        switch (_category.name)
        {
            case "Fish":
                for (int i = 0; i < _itemCollection.Fishes.Count; i++)
                {
                    GameObject item = _itemList.transform.GetChild(i).gameObject;
                    if (!item.activeSelf)
                    {
                        item.SetActive(true);
                        item.GetComponent<ItemSlot>().Fish = _itemCollection.Fishes[i];
                        item.GetComponent<Image>().sprite = _itemCollection.Fishes[i].Sprite;
                    }
                }
                break;
            case "Decor":
                for (int i = 0; i < _itemCollection.Decor.Count; i++)
                {
                    GameObject item = _itemList.transform.GetChild(i).gameObject;
                    if (!item.activeSelf)
                    {
                        item.SetActive(true);
                        item.GetComponent<ItemSlot>().Decor = _itemCollection.Decor[i];
                        item.GetComponent<Image>().sprite = _itemCollection.Decor[i].Sprites[i];
                    }
                }
                break;

        }
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
