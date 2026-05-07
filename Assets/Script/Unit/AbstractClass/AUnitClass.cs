using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AUnitClass : MonoBehaviour
{
    #region ATTRIBUTS

    [SerializeField] protected NavMeshAgent navMeshAgent = null; 

    [SerializeField] protected UnitData unitData = null;
    [SerializeField] protected AmmoData ammoData = null;

    [Header("Health")]
    [SerializeField] protected float currentHealth = 0;
    [SerializeField] protected int currentArmor = 0;

    [Header("Unit Type")]
    [SerializeField] protected EFaction _unitFaction = EFaction.NONE;

    [Header("Unit Debug")]
    [SerializeField] protected bool freezeUnit = false;

    #endregion ATTRIBUTS

    #region PROPERTIES

    public EFaction UnitFaction
    {
        get => _unitFaction; 
        set => _unitFaction = value;
    }

    public UnitData UnitData => unitData;

    #endregion PROPERTIES

    #region METHODE

    public void HealthUpdate(float damage)
    {
        currentHealth = Mathf.Clamp(damage, 0, unitData.MaxHealth);
    }

    public void InitUnit()
    {
        navMeshAgent.speed = unitData.MaxSpeed;
        currentHealth = unitData.MaxHealth;
        currentArmor = unitData.Armor;
        NetworkManager.Instance.NetworkLoad += unitData.NetworkCost;
    }

    public void UnitDestroyed()
    {
        NetworkManager.Instance.NetworkLoad -= unitData.NetworkCost;
        Destroy(gameObject);
    }

    #endregion METHODE

    #region ABSTRACT METHODE

    //Gestion déplacement unité
    abstract public void MovementUnit();

    //Gestion dégat infligé par l'unité
    abstract public void AttackDamage();

    //Gestion dégat reçu par l'unité
    abstract public void TakeDamage();

    #endregion ABSTRACT METHODE
}