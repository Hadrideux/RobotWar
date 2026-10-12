using UnityEditor;
using UnityEngine;

public class PlayerDebugTool : EditorWindow
{
    private Vector2 scrollPos = Vector2.zero;

    // --- Escouade joueur ---
    private AUnitClass playerUnitPrefab = null;
    private Transform squadSpawnPoint = null;
    private int squadSize = 3;

    // --- Charge Réseaux Joueur ---
    private int networkLoad = 0;

    // --- Bâtiment à "frapper" ---
    private ABuildClass targetBuilding = null;

    [MenuItem("Tools/Player Debug Tool")]
    public static void OpenWindow()
    {
        GetWindow<PlayerDebugTool>("Player Debug Tool");
    }

    //Rafraîchit la fenêtre ~10 fois/seconde → les infos restent "live" en Play Mode
    void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.Label("Player Debug Tool", EditorStyles.largeLabel);

        //Hors Play Mode, les singletons n'existent pas → on s'arrête là
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Lance le Play Mode pour utiliser cet outil.", MessageType.Info);
            GUILayout.EndScrollView();
            return;
        }

        DrawPlayerStatus();
        DrawNetworkLoad();
        DrawSpawnSquad();
        DrawDefense();

        GUILayout.EndScrollView();
    }

    #region DRAW
    private void DrawPlayerStatus()
    {
        DrawSeparator();
        GUILayout.Label("État du joueur", EditorStyles.boldLabel);

        PlayerInteraction playerInteraction = PlayerInteraction.Instance;
        EditorGUILayout.LabelField("État", playerInteraction.EPlayerState.ToString());

        DrawSeparator();
        RequisitionManager requisitionManager = RequisitionManager.Instance;
        EditorGUILayout.LabelField("Stock réquisition", requisitionManager.GetRequisitionData(EFactionType.ALLY).ToString());

        NetworkManager networkManager = NetworkManager.Instance;
        EditorGUILayout.LabelField("Charge du réseaux", networkManager.GetNetworkData(EFactionType.ALLY).CurrentLoad.ToString()); 
        EditorGUILayout.LabelField("Pourcentage de charge du réseaux", networkManager.GetNetworkData(EFactionType.ALLY).PercentLoad.ToString()); 
    }

    private void DrawSpawnSquad()
    {
        DrawSeparator();
        GUILayout.Label("Escouade joueur", EditorStyles.boldLabel);

        GUILayout.Space(4);
        playerUnitPrefab = (AUnitClass)EditorGUILayout.ObjectField("Prefab unité joueur", playerUnitPrefab, typeof(AUnitClass), false);
        squadSpawnPoint = (Transform)EditorGUILayout.ObjectField("Point d'apparition", squadSpawnPoint, typeof(Transform), true);
        squadSize = EditorGUILayout.IntSlider("Taille escouade", squadSize, 1, 10);

        if (GUILayout.Button("Faire apparaître une escouade joueur") && playerUnitPrefab != null && squadSpawnPoint != null)
        {
            SpawnPlayerSquad();
        }
    }

    private void DrawDefense()
    {
        DrawSeparator();
        GUILayout.Label("Défense", EditorStyles.boldLabel);

        // ⑤ ObjectField : glisser-déposer un objet de la scène (true) ou un prefab (false)
        targetBuilding = (ABuildClass)EditorGUILayout.ObjectField("Bâtiment Joueur", targetBuilding, typeof(ABuildClass), true);
        if (GUILayout.Button("Simuler un coup sur ce bâtiment") && targetBuilding != null)
        {
            BuildManager.Instance.BuildignAttacked(targetBuilding);
        }
    }

    private void DrawNetworkLoad()
    {
        NetworkData networkManager = NetworkManager.Instance.GetNetworkData(EFactionType.ALLY);

        DrawSeparator();
        GUILayout.Label("Charge réseau joueur", EditorStyles.boldLabel);

        GUILayout.Space(4);
        networkLoad = EditorGUILayout.IntSlider("Charge", networkLoad, 0, networkManager.MaxLoad * 2);
        networkManager.CurrentLoad = networkLoad;
        
    }
    #endregion DRAW
    #region ACTIONS
    private void SpawnPlayerSquad()
    {
        for (int i = 0; i < squadSize; i++)
        {
            Vector3 offset = new Vector3(i * 2f, 0f, 0f);
            AUnitClass unit = Instantiate(playerUnitPrefab, squadSpawnPoint.position + offset, Quaternion.identity);
            unit.FactionObject = EFactionType.ALLY;
            UnitManager.Instance.UnitAvailable(unit);
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
