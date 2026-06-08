using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class NetworkEffectCreatorTool : EditorWindow
{
    

    // --- Dossier de sortie des assets ---
    private string OUTPUT_FOLDER = "Assets/Script/Network/Data/EffectSO";

    private Vector2 scrollPos = Vector2.zero;

    private string effectName = "effect Name";
    
    private EStatEffectedType statAffected = EStatEffectedType.NONE;
    private ECategoryEffectType categoryEffect = ECategoryEffectType.NONE;

    private float thresholdEffect = 0f;

   
    private float statModifier = 0f;
    private int effectDuration = 0;
    private int effectCooldown = 0;
    
    private float procProbability = 0;

    [MenuItem("Tools/NetworkEffectCreatorTool")]
    public static void OpenWindow()
    {
        GetWindow<NetworkEffectCreatorTool>("NECT : NetworkEffectCreatorTool");
    }

    void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);

        DrawTitle();
        OUTPUT_FOLDER = EditorGUILayout.TextField("Dossier de création", OUTPUT_FOLDER);

        DrawIdentification();
        DrawStat();

        DrawSeparator();

        if (GUILayout.Button("Create Effect"))
        {
            Debug.Log("Création de l'effet : " + effectName);
            CreateEffect();
        }

        GUILayout.EndScrollView();
    }    

    #region DRAW INSPECTOR
    public void DrawTitle()
    {
        GUILayout.Space(6);
        GUILayout.Label("Network Effect Creator Tool", EditorStyles.largeLabel);
        GUILayout.Space(2);
    }
    public void DrawIdentification()
    {
        DrawSeparator();
        GUILayout.Label("Effect identification", EditorStyles.boldLabel);
        GUILayout.Space(4);

        effectName = EditorGUILayout.TextField("Effect name", effectName);
        GUILayout.Space(4);
        statAffected = (EStatEffectedType)EditorGUILayout.EnumPopup("Stat Affected", statAffected);
        GUILayout.Space(2);
        categoryEffect = (ECategoryEffectType)EditorGUILayout.EnumPopup("Effect Category", categoryEffect);
    }

    public void DrawStat()
    {
        DrawSeparator();
        GUILayout.Label("Effect stat", EditorStyles.boldLabel);
        GUILayout.Space(4);

        thresholdEffect = EditorGUILayout.Slider("Treshold effect", thresholdEffect, 0f, 1f);
        GUILayout.Space(2);

        statModifier = EditorGUILayout.Slider("Stat Modifier", statModifier, 0f, 2f);
        GUILayout.Space(2);

        procProbability = EditorGUILayout.Slider("Proc Probability", procProbability, 0f, 1f);
        GUILayout.Space(2);

        effectDuration = EditorGUILayout.IntField("Duration", effectDuration);
        GUILayout.Space(2);

        effectCooldown = EditorGUILayout.IntField("Cooldown", effectCooldown);
        GUILayout.Space(2);           
    }

    private void DrawSeparator()
    {
        GUILayout.Space(4);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(2);
    }
    #endregion DRAW INSPECTOR

    #region CREATE ASSET
    private void CreateEffect()
    {
        DrawSeparator();

        //Création du scriptableObject du nouvelle effet
        NetworkEffectData effectData = ScriptableObject.CreateInstance<NetworkEffectData>();

        effectData.EffectName = effectName;
        effectData.StatAffected = statAffected;
        effectData.CategoryEffect = categoryEffect;

        effectData.EffectDuration = effectDuration;
        effectData.Cooldown = effectCooldown;

        AssetPathCreation(effectData);
    }

    private void AssetPathCreation(NetworkEffectData networkEffectData)
    {
        string fileName = effectName.Replace(" ", "_");
        string path = AssetDatabase.GenerateUniqueAssetPath($"{OUTPUT_FOLDER}/{fileName}_SO.asset");

        Debug.Log($"unitName = {effectName}");
        Debug.Log($"fileName = {fileName}");
        Debug.Log($"path = {path}");

        AssetDatabase.CreateAsset(networkEffectData, path);
        AssetDatabase.SaveAssets();
    }

    #endregion CREATE ASSET
}
