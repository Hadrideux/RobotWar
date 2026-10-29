using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitManager : Singleton<UnitManager>
{
    #region ATTRIBUTS

    [Header("Active Unit")]
    [SerializeField] private List<AUnitClass> playerActiveUnits = new List<AUnitClass>();
    [Space(2)]
    [SerializeField] private List<AUnitClass> iaActiveUnits = new List<AUnitClass>();

    #endregion  ATTRIBUTS

    #region PROPERTIES
    public List<AUnitClass> PlayerActiveUnits
    {
        get => playerActiveUnits;
        set => playerActiveUnits = value;
    }
    public List<AUnitClass> IAActiveUnits
    {
        get => iaActiveUnits;
        set => iaActiveUnits = value;
    }

    #endregion PROPERTIES

    #region EVENTS

    private event Action<AUnitClass> onUnitDestroyed;
    public event Action<AUnitClass> OnUnitDestroyed
    {
        add
        {
            onUnitDestroyed -= value;
            onUnitDestroyed += value;
        }
        remove
        {
            onUnitDestroyed -= value;
        }
    }

    private event Action<AUnitClass> onUnitProduced = null;
    public event Action<AUnitClass> OnUnitProduced
    {
        add
        {
            onUnitProduced -= value;
            onUnitProduced += value;
        }
        remove
        {
            onUnitProduced -= value;
        }
    }

    #endregion EVENTS

    #region METHODES
    public List<AUnitClass> GetElligibleUnits(EFactionType factionAffected, EStatEffectedType effectType)
    {
        List<AUnitClass> elligibleUnits = new List<AUnitClass>();

        switch (factionAffected)
        {
            case EFactionType.ALLY:
                foreach (AUnitClass unit in playerActiveUnits)
                {
                    if (unit.EffectComponent.IsEligibleFor(effectType))
                    {
                        elligibleUnits.Add(unit);
                    }
                }
                break;
            case EFactionType.IA:
                foreach (AUnitClass unit in iaActiveUnits)
                {
                    if (unit.EffectComponent.IsEligibleFor(effectType))
                    {
                        elligibleUnits.Add(unit);
                    }
                }
                break;

            default:
                break;
        }
        
        return elligibleUnits;
    }
    public void UnitDestroyed(AUnitClass unitDestroyed)
    {
        NetworkManager.Instance.GetNetworkData(unitDestroyed.FactionObject).CurrentLoad -= unitDestroyed.UnitData.NetworkCost;

        switch (unitDestroyed.FactionObject)
        {
            case EFactionType.ALLY:
                playerActiveUnits.Remove(unitDestroyed);
                SelectableManager.Instance.SelectableObj.Remove(unitDestroyed);
                break;
            case EFactionType.IA:
                iaActiveUnits.Remove(unitDestroyed);
                IAManager.Instance.RemoveUnit(unitDestroyed);
                break;
            default:
                break;
        }
    }
    public void UnitRegistering(AUnitClass unitProduced)
    {
        NetworkManager.Instance.GetNetworkData(unitProduced.FactionObject).CurrentLoad += unitProduced.UnitData.NetworkCost;

        switch (unitProduced.FactionObject)
        {
            case EFactionType.ALLY:
                playerActiveUnits.Add(unitProduced);
                break;
            case EFactionType.IA:
                iaActiveUnits.Add(unitProduced);                
                break;
            default:
                break;
        }
    }
    
    public void UnitProduced(AUnitClass unitProduced)
    {
        if (unitProduced.FactionObject != EFactionType.IA)
            return;

        if (onUnitProduced != null)
        {
            onUnitProduced(unitProduced);
        }
    }
    #endregion

}
