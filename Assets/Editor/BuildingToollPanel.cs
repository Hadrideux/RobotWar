using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BuildingToollPanel : EditorWindow
{
    [MenuItem("Tools/BuildingToolPanel")]
    public static void ShowWindow()
    {
        GetWindow<BuildingToollPanel>("BCT : Building Creator Tool");
    }

    //Dossier de sortie des assets
    private string OUTPUT_FOLDER = "Assets/Script/Building/Data/BuildingSO";

    private string buildName = "Building Name";
    private EBuildType buildType = EBuildType.NONE;

    private int durability = 0;
    private int armor = 0;

    private float buildingTime = 0;
    private int buildingCost = 0;

    void OnGUI()
    {
        DrawTitle();
        OUTPUT_FOLDER = EditorGUILayout.TextField("Dossier de création", OUTPUT_FOLDER);

        DrawUnitData();
        DrawSeparator();

        if (GUILayout.Button("Create Building"))
        {
            Debug.Log("Création du batiment: " + buildName);
            CreateUnit();
        }
    }

    #region DRAW INSPECTOR
    public void DrawTitle()
    {
        GUILayout.Space(6);
        GUILayout.Label("Build Creator Tool", EditorStyles.largeLabel);
        GUILayout.Space(2);
    }

    public void DrawIdentification()
    {
        GUILayout.Label("Build identification", EditorStyles.boldLabel);

        buildName = EditorGUILayout.TextField("Build name", buildName);
        buildType = (EBuildType)EditorGUILayout.EnumPopup("Build type", buildType);
    }

    public void DrawUnitData()
    {
        DrawSeparator();
        DrawIdentification();

        DrawSeparator();
        DrawBuildStat();

        DrawSeparator();
        DrawBuildFabrication();
    }

    private void DrawBuildFabrication()
    {
        GUILabel("Build Identifaction");
        buildingTime = EditorGUILayout.FloatField("Production Time", buildingTime);
        GUILayout.Space(2);
        buildingCost = EditorGUILayout.IntField("Production Cost", buildingCost);
    }

    private void DrawBuildStat()
    {
        GUILabel("Build stat");
        durability = EditorGUILayout.IntField("Build Durability", durability);
        GUILayout.Space(2);
        armor = EditorGUILayout.IntField("Build Armor", armor);

    }

    private void DrawSeparator()
    {
        GUILayout.Space(4);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(2);
    }

    private void GUILabel(string label)
    {
        GUILayout.Label("Build stat", EditorStyles.boldLabel);
        GUILayout.Space(4);
    }
    #endregion DRAW INSPECTOR

    #region CREATE ASSET
    public void CreateUnit()
    {
        DrawSeparator();

        //Création du scriptableObject du nouveaux Bâtiment
        BuildData buildData = ScriptableObject.CreateInstance<BuildData>();

        buildData.BuildName = buildName;
        buildData.BuildType = buildType;

        buildData.MaxDurability = durability;
        buildData.Armor = armor;

        buildData.BuildingTime = buildingTime;
        buildData.BuildingCost = buildingCost;

        AssetPathCreation(buildData);
    }

    private void AssetPathCreation(BuildData buildData)
    {
        string fileName = buildName.Replace(" ", "_");
        string path = AssetDatabase.GenerateUniqueAssetPath($"{OUTPUT_FOLDER}/{fileName}_SO.asset");

        Debug.Log($"unitName = {buildName}");
        Debug.Log($"fileName = {fileName}");
        Debug.Log($"path = {path}");

        AssetDatabase.CreateAsset(buildData, path);
        AssetDatabase.SaveAssets();
    }
    #endregion CREATE ASSET
}