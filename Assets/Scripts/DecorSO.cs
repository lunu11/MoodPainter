using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Assets/Create Decor")]
public class DecorSO : ScriptableObject
{
    public string Name;
    public List<Sprite> Sprites = new List<Sprite>();

}
