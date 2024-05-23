using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Assets/Create Tank")]
public class TankSO : ScriptableObject
{
    public string FishID;
    public DateTime FishMatureDate;
    public bool IsFishMatured;
    public TimeSpan HappyDuration;
    public bool IsFishAlive;
    public string FishStatus;

    public List<Item> DecorID = new List<Item>();
}

[Serializable]
public class Item
{
    public string ID;
    public int SpriteOrder;
    public Vector3 Position;
}