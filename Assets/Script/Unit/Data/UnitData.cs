using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : ScriptableObject
{
    #region ATTRBIUTS

    [Header("Unit glodal info")]
    [SerializeField] private string unitName = "";
    [SerializeField] private EUnitType unitType = EUnitType.NONE;

    [Header("Unit spec info")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private int armor = 1;
    [SerializeField] private float maxSpeed = 100;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private float viewDistance = 0f;

    [Header("Unit production info")]
    [SerializeField] private float productionTime = 0;
    [SerializeField] private int requisitionCost = 0;
    [SerializeField] private int networkCost = 0;

    #endregion ATTRIBUTS

    #region PROPERTIES
    public string UnitName
    {
        get => unitName;
        set => unitName = value;
    }
    public EUnitType UnitType
    {
        get => unitType;
        set => unitType = value;
    }


    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }
    public int Armor
    {
        get => armor;
        set => armor = value;
    }
    public float MaxSpeed
    {
        get => maxSpeed;
        set => maxSpeed = value;
    }
    public float AttackRange
    {
        get => attackRange;
        set => attackRange = value;
    }
    public float ViewDistance
    {
        get => viewDistance;
        set => viewDistance = value;
    }

    public float ProductionTime
    {
        get => productionTime;
        set => productionTime = value;
    }
    public int RequisitionCost
    {
        get => requisitionCost;
        set => requisitionCost = value;
    }
    public int NetworkCost
    {
        get => networkCost;
        set => networkCost = value;
    }
    #endregion PROPERTIES
}
