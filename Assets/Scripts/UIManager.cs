using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : MonoBehaviour
{    
    public void ToggleUI(GameObject UI)
    {
        UI.SetActive(!UI.activeSelf);
    }
}
