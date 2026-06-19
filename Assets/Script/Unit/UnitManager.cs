using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{

    [SerializeField] private List<AUnitClass> activeUnits = new List<AUnitClass>();
    

    public List<AUnitClass> ActiveUnits
    {
        get => activeUnits;
        set => activeUnits = value;
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



    public List<AUnitClass> GetElligibleUnits(EStatEffectedType effectType)
    {
        List<AUnitClass> elligibleUnits = new List<AUnitClass>();

        foreach (AUnitClass unit in activeUnits)
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
        switch(unitDestroyed.ObjectFaction)
        {
            case EFactionType.ALLY:
                activeUnits.Remove(unitDestroyed);
                break;
            case EFactionType.IA:
                break;
            default:
                break;
        }
        if (onUnitDestroyed != null)
            onUnitDestroyed(unitDestroyed);


    }

    public void UnitProduced(AUnitClass unitProduced)
    {
        switch(unitProduced.ObjectFaction)
        {
            case EFactionType.ALLY:
                activeUnits.Add(unitProduced);
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
}
