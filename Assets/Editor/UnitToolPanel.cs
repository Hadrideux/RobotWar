using System;
using UnityEditor;
using UnityEngine;


public class UnitToolPanel : EditorWindow
{
    

    //Dossier de sortie des assets
    private string OUTPUT_FOLDER = "Assets/Script/Unit/Data/UnitSO";

    private Vector2 scrollPos = Vector2.zero;

    private string unitName = "Unit Name";
    private EUnitType unitType = EUnitType.NONE;

    private float maxHealth = 100f;
    private float maxSpeed = 100;
    private int armor = 1;
    private float attackRange = 50f;
    private float viewDistance = 20f;


    private float productionTime = 0;
    private int productionCost = 0;
    private int networkCost = 0;       

    private Mesh unitBody = null;
    private Mesh turretBody = null;
    private Mesh weaponBody = null;

    [MenuItem("Tools/UnitToolPanel")]
    public static void OpenWindow()
    {
        GetWindow<UnitToolPanel>("UCT : Unit Creator Tool");
    }

    void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);

        DrawTitle();
        OUTPUT_FOLDER = EditorGUILayout.TextField("Dossier de création", OUTPUT_FOLDER);

        DrawUnitData();
        DrawSeparator();

        if (GUILayout.Button("Create Unit"))
        {            
            Debug.Log("Création de l'unité : " + unitName);
            CreateUnit();
        }

        GUILayout.EndScrollView();
    }

    #region DRAW INSPECTOR
    public void DrawTitle()
    {
        GUILayout.Space(6);
        GUILayout.Label("Unit Creator Tool", EditorStyles.largeLabel);
        GUILayout.Space(2);
    }

    public void DrawIdentification()
    {
        GUILayout.Label("Unit identification", EditorStyles.boldLabel);

        unitName = EditorGUILayout.TextField("Unit name", unitName);
        unitType = (EUnitType) EditorGUILayout.EnumPopup("Unit type", unitType);
    }

    public void DrawUnitData()
    {
        DrawSeparator();
        DrawIdentification();

        DrawSeparator();
        DrawUnitStat();

        DrawSeparator();
        DrawUnitFabrication();

        DrawSeparator();
        DrawUnitBody();
    }

    private void DrawUnitBody()
    {
        GUILabel("Unit Body");
        unitBody = (Mesh) EditorGUILayout.ObjectField("Unit Body", unitBody, typeof(Mesh), false);
        GUILayout.Space(2);
        turretBody = (Mesh) EditorGUILayout.ObjectField("Turret Body", turretBody, typeof(Mesh), false);
        GUILayout.Space(2);
        weaponBody = (Mesh) EditorGUILayout.ObjectField("Weapon Body", weaponBody, typeof(Mesh), false);
    }

    private void DrawUnitFabrication()
    {
        GUILabel("Unit Identifaction");
        productionTime = EditorGUILayout.FloatField("Production Time", productionTime);
        GUILayout.Space(2);
        productionCost = EditorGUILayout.IntField("Production Cost", productionCost);
        GUILayout.Space(2);
        networkCost = EditorGUILayout.IntField("Network Cost", networkCost);
    }

    private void DrawUnitStat()
    {
        GUILabel("Unit stat");
        maxHealth = EditorGUILayout.FloatField("Max Health", maxHealth);
        GUILayout.Space(2);
        armor = EditorGUILayout.IntField("Armors", armor);
        GUILayout.Space(2);
        maxSpeed = EditorGUILayout.FloatField("Max Speed", maxSpeed);
        GUILayout.Space(2);
        attackRange = EditorGUILayout.FloatField("Attack Range", attackRange);
        GUILayout.Space(2);
        viewDistance = EditorGUILayout.FloatField("View Distance", viewDistance);
    }

    private void DrawSeparator()
    {
        GUILayout.Space(4);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(2);
    }

    private void GUILabel(string label)
    {
        GUILayout.Label("Unit stat", EditorStyles.boldLabel);
        GUILayout.Space(4);
    }
    #endregion DRAW INSPECTOR

    #region CREATE ASSET
    public void CreateUnit()
    {
        DrawSeparator();

        //Création du scriptableObject de la nouvelle unité
        UnitData unitData = ScriptableObject.CreateInstance<UnitData>();

        unitData.UnitName = unitName;
        unitData.UnitType = unitType;
        unitData.MaxHealth = maxHealth;
        unitData.MaxSpeed = maxSpeed;
        unitData.AttackRange = attackRange;
        unitData.ViewDistance = viewDistance;

        unitData.ProductionTime = productionTime;
        unitData.ProductionCost = productionCost;
        unitData.NetworkCost = networkCost;

        AssetPathCreation(unitData);
    }

    private void AssetPathCreation(UnitData unitData)
    {
        string fileName = unitName.Replace(" ", "_");
        string path = AssetDatabase.GenerateUniqueAssetPath($"{OUTPUT_FOLDER}/{fileName}_SO.asset");

        Debug.Log($"unitName = {unitName}");
        Debug.Log($"fileName = {fileName}");
        Debug.Log($"path = {path}");

        AssetDatabase.CreateAsset(unitData, path);
        AssetDatabase.SaveAssets();
    }

    public void CreateAmmo()
    {

    }
    #endregion CREATE ASSET

}