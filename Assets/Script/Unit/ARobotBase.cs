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
    [SerializeField] protected EUnitFaction _unitFaction = EUnitFaction.NONE;

    [Header("Configuration")]
    [SerializeField] protected float _unitDamage = 0f;
    [SerializeField] protected float _unitRange = 0f;
    [SerializeField] protected float _unitSpeed = 0f;

    [Header("Production")]
    [SerializeField] protected int _unitCost = 0;
    [SerializeField] protected int _unitProductionTime = 0;

    #endregion ATTRIBUTS

    #region METHODE

    public void HealthUpdate(float damage)
    {
        _healthCurrent = Mathf.Clamp(damage, _healthMin, _healthMax);
    }

    #endregion METHODE

    #region ABSTRACT METHODE

    abstract public void ProductionCost();

    abstract public void MovementUnit();

    abstract public void Attack();

    abstract public void TakeDamage();

    #endregion ABSTRACT METHODE
}