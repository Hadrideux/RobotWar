using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ARobotBase : MonoBehaviour
{
    #region ATTRIBUTS

    [Header("Health")]
    [SerializeField] protected float _healthMax = 100;
    [SerializeField] protected float _healthMin = 0;
    [SerializeField] protected float _healthCurrent = 0;

    [Header("Unit")]
    [SerializeField] protected EUnitType _unitType = EUnitType.NONE;
    [SerializeField] protected EFaction _unitFaction = EFaction.NONE;

    [Header("Configuration")]
    [SerializeField] protected float _unitDamage = 0f;
    [SerializeField] protected float _unitRange = 0f;
    [SerializeField] protected float _unitSpeed = 0f;

    [Header("Production")]
    [SerializeField] protected int _unitCost = 0;
    [SerializeField] protected float _unitManufactureTime = 0;

    #endregion ATTRIBUTS

    #region PROPERTIES

    public int UnitCost => _unitCost;

    public float UnitManufactureTime => _unitManufactureTime;

    public EFaction UnitFaction
    {
        get => _unitFaction; 
        set => _unitFaction = value;
    }

    #endregion PROPERTIES

    #region METHODE

    public void HealthUpdate(float damage)
    {
        _healthCurrent = Mathf.Clamp(damage, _healthMin, _healthMax);
    }

    public void ProductionCost()
    {

    }

    #endregion METHODE

    #region ABSTRACT METHODE

    abstract public void MovementUnit();

    abstract public void AttackDamage();

    abstract public void TakeDamage();

    #endregion ABSTRACT METHODE
}