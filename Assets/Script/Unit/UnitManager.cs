using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitManager : Singleton<UnitManager>
{
    [SerializeField] private List<AUnitClass> playerActiveUnits = new List<AUnitClass>();
    [SerializeField] private List<AUnitClass> iaActiveUnits = new List<AUnitClass>();

    public List<AUnitClass> PlayerActiveUnits
    {
        get => playerActiveUnits;
        set => playerActiveUnits = value;
    }
    public List<AUnitClass> IAActiveUnits
    {
        get => iaActiveUnits;
        set => iaActiveUnits  = value;
    }

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
    public List<AUnitClass> GetElligibleUnits(EStatEffectedType effectType)
    {
        List<AUnitClass> elligibleUnits = new List<AUnitClass>();

        foreach (AUnitClass unit in playerActiveUnits)
        {
            if (unit.EffectComponent.IsEligibleFor(effectType))
            {
                elligibleUnits.Add(unit);
            }
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
                break;
            case EFactionType.IA:
                IAManager.Instance.RemoveUnit(unitDestroyed);
                break;
            default:
                break;
        }
    }
    public void UnitAvailable(AUnitClass unitProduced)
    {
        NetworkManager.Instance.GetNetworkData(unitProduced.FactionObject).CurrentLoad += unitProduced.UnitData.NetworkCost;
        
        switch (unitProduced.FactionObject)
        {
            case EFactionType.ALLY:
                playerActiveUnits.Add(unitProduced);
                break;
            case EFactionType.IA:

                if (onUnitProduced != null)
                {
                    onUnitProduced(unitProduced);
                }
                break;
            default:
                break;
        }
    }
    #endregion

}
