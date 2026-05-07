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

    [SerializeField] protected float damage = 0f;
    [SerializeField] protected float attackRange = 0f;

    [Header("Unit production info")]
    [SerializeField] private int productionTime = 0;
    [SerializeField] private int requisitonCost = 0;
    [SerializeField] private int networkCost = 0;

    #endregion ATTRIBUTS

    #region PROPERTIES
    public EUnitType UnitType => unitType;

    public int ProductionTime => productionTime;
    public int RequisitonCost => requisitonCost;
    public int NetworkCost => networkCost;

    public float MaxHealth => maxHealth;
    public int Armor => armor;
    public float MaxSpeed => maxSpeed;

    #endregion PROPERTIES
}
