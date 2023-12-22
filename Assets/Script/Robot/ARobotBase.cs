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
