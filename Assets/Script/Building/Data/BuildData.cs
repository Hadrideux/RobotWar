using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BuildData", menuName = "Scriptable Objects/BuildData")]
public class BuildData : ScriptableObject
{
    #region ATTRBIUTS

    [Header("Build glodal info")]
    [SerializeField] private string buildName = "";
    [SerializeField] private Sprite buildIcon = null;
    [SerializeField] private EBuildType buildType = EBuildType.NONE;

    [Header("Build spec info")]
    [SerializeField] private int maxDurability = 100;
    [SerializeField] private int armor = 0;

    [Header("Build production info")]
    [SerializeField] private float buildingTime = 0;
    [SerializeField] private int buildCost = 0;



    #endregion ATTRIBUTS

    #region PROPERTIES
    public string BuildName
    {
        get => buildName;
        set => buildName = value;
    }
    public EBuildType BuildType
    {
        get => buildType;
        set => buildType = value;
    }

    public int MaxDurability
    {
        get => maxDurability;
        set => maxDurability = value;
    }
    public int Armor
    {
        get => armor;
        set => armor = value;
    }

    public float BuildingTime
    {
        get => buildingTime;
        set => buildingTime = value;
    }
    public int BuildingCost
    {
        get => buildCost;
        set => buildCost = value;
    }
    public Sprite BuildingIcon
    {
        get => buildIcon;
        set => buildIcon = value;
    }

    #endregion PROPERTIES

}
