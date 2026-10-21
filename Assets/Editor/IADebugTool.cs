using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IADebugTool : EditorWindow
{
    private Vector2 scrollPos = Vector2.zero;

    // --- Escouade IA ---
    private AUnitClass IAUnitPrefab = null;
    private Transform squadSpawnPoint = null;
    private int squadSize = 3;

    // --- Charge Réseaux IA ---
    private int networkLoad = 0;

    // --- Bâtiment à "frapper" ---
    private ABuildClass targetBuilding = null;

    [MenuItem("Tools/IA Debug")]
    public static void OpenWindow()
    {
        GetWindow<IADebugTool>("IA Debug");
    }

    // ① Rafraîchit la fenêtre ~10 fois/seconde → les infos restent "live" en Play Mode
    void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.Label("IA Debug Tool", EditorStyles.largeLabel);

        // ② Hors Play Mode, les singletons n'existent pas → on s'arrête là
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Lance le Play Mode pour utiliser cet outil.", MessageType.Info);
            GUILayout.EndScrollView();
            return;
        }

        DrawIAStatus();
        DrawNetworkLoad();
        DrawReinforcement();
        DrawSpawnSquad();
        DrawAttack();
        DrawDefense();
        DrawTime();

        GUILayout.EndScrollView();
    }

    #region DRAW
    private void DrawIAStatus()
    {
        DrawSeparator();
        GUILayout.Label("État de l'IA", EditorStyles.boldLabel);

        IAManager ia = IAManager.Instance;
        EditorGUILayout.LabelField("État", ia.CurrentState.ToString());
        EditorGUILayout.LabelField("Unités d'attaque", $"{ia.AttackUnits.Count} / {ia.AttackThreshold}");
        EditorGUILayout.LabelField("Unités de défense", $"{ia.DefenseUnits.Count} / {ia.DefenseThreshold}");
        EditorGUILayout.LabelField("Attaques lancées", ia.LaunchedAttackCount.ToString());
        EditorGUILayout.LabelField("Base attaquée", ia.IsBaseUnderAttack ? "Oui" : "Non");

        NetworkManager networkManager = NetworkManager.Instance;
        EditorGUILayout.LabelField("Charge du réseaux", networkManager.GetNetworkData(EFactionType.IA).CurrentLoad.ToString());
        EditorGUILayout.LabelField("Pourcentage de charge du réseaux", networkManager.GetNetworkData(EFactionType.IA).PercentLoad.ToString());

        RequisitionManager requisitionManager = RequisitionManager.Instance;
        EditorGUILayout.LabelField("Stock réquisition", requisitionManager.GetRequisitionData(EFactionType.ALLY).ToString());
    }
    private void DrawSpawnSquad()
    {
        DrawSeparator();
        GUILayout.Label("Escouade IA", EditorStyles.boldLabel);

        GUILayout.Space(4);
        IAUnitPrefab = (AUnitClass)EditorGUILayout.ObjectField("Prefab unité IA", IAUnitPrefab, typeof(AUnitClass), false);
        squadSpawnPoint = (Transform)EditorGUILayout.ObjectField("Point d'apparition", squadSpawnPoint, typeof(Transform), true);
        squadSize = EditorGUILayout.IntSlider("Taille escouade", squadSize, 1, 10);

        if (GUILayout.Button("Faire apparaître une escouade IA") && IAUnitPrefab != null && squadSpawnPoint != null)
        {
            SpawnIASquad();
        }
    }
    private void DrawNetworkLoad()
    {
        NetworkData networkManager = NetworkManager.Instance.GetNetworkData(EFactionType.IA);

        DrawSeparator();
        GUILayout.Label("Charge réseau IA", EditorStyles.boldLabel);

        GUILayout.Space(4);
        networkLoad = EditorGUILayout.IntSlider("Charge", networkManager.CurrentLoad, 0, networkManager.MaxLoad * 2);
        networkManager.CurrentLoad = networkLoad;

    }
    private void DrawReinforcement()
    {
        DrawSeparator();
        GUILayout.Label("Renforts", EditorStyles.boldLabel);

        if (GUILayout.Button("Forcer un renfort maintenant"))
        {
            // ③ Le spawner n'est pas un singleton → on le cherche dans la scène
            IAReinforcementSpawner spawner = FindAnyObjectByType<IAReinforcementSpawner>();
            spawner.ReinforcementSpawning();
        }
    }

    private void DrawAttack()
    {
        DrawSeparator();
        GUILayout.Label("Attaque", EditorStyles.boldLabel);

        if (GUILayout.Button("Détruire toutes les unités d'attaque"))
        {
            // ④ On parcourt une COPIE : détruire une unité la retire de la liste d'origine
            List<AUnitClass> units = new List<AUnitClass>(IAManager.Instance.AttackUnits);
            foreach (AUnitClass unit in units)
            {
                unit.UnitKilled();
            }
        }

        if (GUILayout.Button("Détruire toutes les unités de l'IA"))
        {
            // ④ On parcourt une COPIE : détruire une unité la retire de la liste d'origine
            List<AUnitClass> units = new List<AUnitClass>(UnitManager.Instance.IAActiveUnits);
            foreach (AUnitClass unit in units)
            {
                unit.UnitKilled();
            }
        }
    }

    private void DrawDefense()
    {
        DrawSeparator();
        GUILayout.Label("Défense", EditorStyles.boldLabel);

        // ⑤ ObjectField : glisser-déposer un objet de la scène (true) ou un prefab (false)
        targetBuilding = (ABuildClass)EditorGUILayout.ObjectField("Bâtiment IA", targetBuilding, typeof(ABuildClass), true);
        if (GUILayout.Button("Simuler un coup sur ce bâtiment") && targetBuilding != null)
        {
            BuildManager.Instance.BuildignAttacked(targetBuilding);
        }
    }

    private void DrawTime()
    {
        DrawSeparator();
        GUILayout.Label($"Vitesse du temps : x{Time.timeScale}", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("x1")) Time.timeScale = 1f;
        if (GUILayout.Button("x2")) Time.timeScale = 2f;
        if (GUILayout.Button("x4")) Time.timeScale = 4f;
        GUILayout.EndHorizontal();
    }

    #endregion DRAW
    #region ACTIONS
    private void SpawnIASquad()
    {
        for (int i = 0; i < squadSize; i++)
        {
            Vector3 offset = new Vector3(i * 2f, 0f, 0f);
            AUnitClass unit = Instantiate(IAUnitPrefab, squadSpawnPoint.position + offset, Quaternion.identity);
            unit.FactionObject = EFactionType.IA;
        }
    }
    private void DrawSeparator()
    {
        GUILayout.Space(4);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(2);
    }
    #endregion ACTIONS
}