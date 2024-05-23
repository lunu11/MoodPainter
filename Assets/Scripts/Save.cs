using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Assets/Create Save")]
public class Save : ScriptableObject
{
    public List<bool> Fishes = new List<bool>();
    public List<bool> Decor = new List<bool>();
}
