using UnityEngine;
using UnityEditor;

// ensure class initializer is called whenever scripts recompile
[InitializeOnLoadAttribute]
public static class PauseStateChanged
{
    // register an event handler when the class is initialized
    static PauseStateChanged()
    {
        EditorApplication.pauseStateChanged += LogPauseState;
    }

    private static void LogPauseState(PauseState state)
    {
        Debug.Log(state);
    }
}