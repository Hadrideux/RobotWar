using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{

    [SerializeField] private List<AUnitClass> activeUnits = new List<AUnitClass>();

    public List<AUnitClass> ActiveUnits
    {
        get => activeUnits;
        set => activeUnits = value;
    }

    private event Action<AUnitClass> _onUnitDestroyed;
    public event Action<AUnitClass> OnUnitDestroyed
    {
        add
        {
            _onUnitDestroyed -= value;
            _onUnitDestroyed += value;
        }
        remove
        {
            _onUnitDestroyed -= value;
        }
    }

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
        UnitManager.Instance.ActiveUnits.Remove(unitDestroyed);

        if (_onUnitDestroyed != null)
            _onUnitDestroyed(unitDestroyed);
    }
}
