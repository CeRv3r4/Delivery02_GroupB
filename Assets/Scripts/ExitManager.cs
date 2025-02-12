using UnityEngine;
using UnityEditor;

public class ExitManager : MonoBehaviour
{
    public void OnExit()
    {

        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();

    }
}
