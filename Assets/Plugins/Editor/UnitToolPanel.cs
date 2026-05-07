using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UnitToolPanel : EditorWindow
{

    [SerializeField] private GameObject unitSpawnPoint = null;

    [MenuItem("Tools/UnitToolPanel")]
    public static void ShowWindow()
    {
        GetWindow<UnitToolPanel>("Outil de controlle d'unit dans la scène");
    }
    void OnGUI()
    {
        GUILayout.Label("Unit Tool", EditorStyles.boldLabel);

        if (GUILayout.Button("Freeze unit"))
        {
            Debug.Log("Gêle les unité de la scène");
        }

        if (GUILayout.Button("Handle unit move"))
        {
            Debug.Log("Active/Désactive les daplcement des unité");
        }
        if (GUILayout.Button("Peacefully unit"))
        {
            Debug.Log("Active/Désactive les arme des unité");
        }
        if (GUILayout.Button("Spawn unit"))
        {
            Debug.Log("Spawn d'unité");
        }
    }
    
}
