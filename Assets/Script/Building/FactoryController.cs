using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryController : ABuildingBase
{
    [SerializeField] protected EUnitType _unitProduction = EUnitType.NONE;

    [SerializeField] private Transform _spawnUnit = null;
    [SerializeField] private Transform _robotContainer = null;

    #region METHODE

    private void SetProductionType()
    {
        //Spawn produced Unit 
        switch (_unitProduction)
        {
            case EUnitType.TANK:
                Debug.Log(_unitProduction + " Cost : " + RobotManager.Instance.RobotTank.UnitCost);

                RobotTank robotTank = Instantiate(RobotManager.Instance.RobotTank, _spawnUnit.position, Quaternion.identity, _robotContainer);
                robotTank.UnitFaction = _buildingFaction;

                BaseManager.Instance.AllyRessource -= RobotManager.Instance.RobotTank.UnitCost;

                break;

            case EUnitType.AIRCRAFT:
                Debug.Log(_unitProduction + " Cost : " + RobotManager.Instance.RobotAirCraft.UnitCost);

                RobotAirCraft roboAirCraft = Instantiate(RobotManager.Instance.RobotAirCraft, _spawnUnit.position, Quaternion.identity, _robotContainer);
                roboAirCraft.UnitFaction = _buildingFaction;

                BaseManager.Instance.AllyRessource -= RobotManager.Instance.RobotAirCraft.UnitCost;

                break;

            case EUnitType.MECHA:
                Debug.Log(_unitProduction + " Cost : " + RobotManager.Instance.RobotMecha.UnitCost);

                RobotMecha robotMecha = Instantiate(RobotManager.Instance.RobotMecha, _spawnUnit.position, Quaternion.identity, _robotContainer);
                robotMecha.UnitFaction = _buildingFaction;

                BaseManager.Instance.AllyRessource -= RobotManager.Instance.RobotMecha.UnitCost;

                break;

            case EUnitType.ANTIAIR:
                Debug.Log(_unitProduction + " Cost : " + RobotManager.Instance.RobotAntiAir.UnitCost);

                RobotAntiAir robotAntiAir = Instantiate(RobotManager.Instance.RobotAntiAir, _spawnUnit.position, Quaternion.identity, _robotContainer);
                robotAntiAir.UnitFaction = _buildingFaction;

                BaseManager.Instance.AllyRessource -= RobotManager.Instance.RobotAntiAir.UnitCost;

                break;

            default:
                Debug.Log(EUnitType.NONE);

                break;
        } 
    }

    #endregion METHODE

    #region ABSTARCT METHODE

    protected override void UpdateProductionTime()
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateRateProduction()
    {
        _productionRate += _buildingLevel; 
    }

    protected override void ChangeFaction()
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateBuildingCapture()
    {
        throw new System.NotImplementedException();
    }

    #endregion ABSTRACT METHODE
}

/*
    IEnumerator Production()
    {
        _productionTime += Time.deltaTime * _productionRate;

        yield return new WaitForSeconds(_productionComplete);
        if (BaseManager.Instance.RessourCount >= _productionCost)
        {
            ProductionType();
        }
        _productionTime = 0;
        Debug.Log("Production Complete");
        StartCoroutine(Production());
    }    

    public override void ProductionTime()
    {
        _productionTime += Time.deltaTime;

        //Time for built the unit
        switch (_unitProduction)
        {
            case EUnitType.TANK:
                if (_productionTime > RobotManager.Instance.RobotTank.UnitManufactureTime)
                {
                    Debug.Log(_unitProduction + " Production : " + RobotManager.Instance.RobotTank.UnitManufactureTime);
                    ProductionType();
                    _productionTime = 0;
                }                
                
                break;

            case EUnitType.AIRCRAFT:
                if (_productionTime > RobotManager.Instance.RobotAirCraft.UnitManufactureTime)
                {
                    Debug.Log(_unitProduction + " Production : " + RobotManager.Instance.RobotAirCraft.UnitManufactureTime);
                    ProductionType();
                    _productionTime = 0;
                }                
                
                break;

            case EUnitType.MECHA:
                if (_productionTime > RobotManager.Instance.RobotMecha.UnitManufactureTime)
                {
                    Debug.Log(_unitProduction + " Production : " + RobotManager.Instance.RobotMecha.UnitManufactureTime);
                    ProductionType();
                    _productionTime = 0;
                }
               
                break;

            case EUnitType.ANTIAIR:
                if (_productionTime > RobotManager.Instance.RobotAntiAir.UnitManufactureTime)
                {
                    Debug.Log(_unitProduction + " Production : " + RobotManager.Instance.RobotAntiAir.UnitManufactureTime);
                    ProductionType();
                    _productionTime = 0;
                }                
                
                break;

            default:
                Debug.Log(EUnitType.NONE);
                
                break;
        }
    }    

    protected override void BuildingCaptured()
    {
        //Si unité alliè > unité ennemie capture en faveur des alliès
        //Si unité alliè < unité ennemie capture en faveur des ennemeis

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
    }

*/