using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Assets/Create Item Collection"))]
public class ItemCollection : ScriptableObject
{
    public FishSO DefaultGuppy;
    public List<FishSO> Fishes = new List<FishSO>();
    public List<DecorSO> Decor = new List<DecorSO>();
}
