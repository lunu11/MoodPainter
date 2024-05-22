using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Assets/Create Tank")]
public class TankSO : ScriptableObject
{
    public string FishID;
    public List<string> DecorID = new List<string>();
}

[Serializable]
public class Item
{
    public string ID;
    public int SpriteOrder;
    public Vector3 Position;
}