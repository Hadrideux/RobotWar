using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManufactureController : ABuildingBase
{
    #region ABSTRACT METHODE

    protected override void ChangeFaction()
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateBuildingCapture()
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateProductionTime()
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateRateProduction()
    {
        throw new System.NotImplementedException();
    }

    #endregion ASBTRACT METHODE
}
/*
    [SerializeField] private float _productionComplete = 0f;
 *  public override void IncreseRate()
    {
        _productionRate += _tierLevel;
    }
   
    public override void ProductionTime()
    {
        _productionTime += Time.deltaTime * _productionRate;

        if (_productionTime > _productionComplete)
        {
            ProductionType();
            _productionTime = 0;
        }
    }

    public override void ProductionType()
    {
        switch (_buildingFaction)
        {
            case EFaction.ALLY:

                BaseManager.Instance.AllyRessource++;
                
                break;

            case EFaction.ENNEMY:

                BaseManager.Instance.EnnemyRessource++;
                
                break;

            default: 
                
                break;
        }
        
    }

    public override void BuildingCaptured()
    {
        switch (_buildingType)
        {
            case EBuildingType.FACTORY:                
                Debug.Log(_buildingType);
                
                break;

            case EBuildingType.PRODUCTION:                
                Debug.Log(_buildingType);
                
                break;

            default:
                Debug.Log(_buildingType);
                
                break;
        }
    }*/