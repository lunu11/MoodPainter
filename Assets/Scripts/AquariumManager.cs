using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquariumManager : MonoBehaviour
{
    [SerializeField] private TankSO TankSO;
    [SerializeField] private ItemCollection ItemCollection;

    [SerializeField] private GameObject _fish;
    [SerializeField] private List<GameObject> _decor = new List<GameObject>();

    [SerializeField] private GameObject _decorPrefab;


    private bool _isBuildMode;

    private void Awake()
    {
        for (int i = 0; i < TankSO.DecorID.Count; i++)
        {
            string id = TankSO.DecorID[i];
            for (int j = 0; j < ItemCollection.Decor.Count; j++)
            {
                if (id == ItemCollection.Decor[i].name)
                {
                    DecorSO decor = ItemCollection.Decor[i];
                    
                }
            }
        }
    }

    public void StartBuildMode()
    {
        if(!_isBuildMode)
        {
            _isBuildMode = true;

        }
    }

    private void Update()
    {
        if (_isBuildMode)
        { 
            if (Input.GetMouseButtonDown(0))
            {

            }
        }
    }

    public void Action(string action)
    {
        switch (action)
        {
            case "Fish":

                break;
            case "Decor":

                break;
        }
    }
}
