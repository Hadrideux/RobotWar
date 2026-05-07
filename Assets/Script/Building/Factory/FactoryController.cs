using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryController : ABuildClass
{
    [SerializeField] protected EUnitType _unitProduction = EUnitType.NONE;

    [SerializeField] private AUnitClass unitToSpawn = null;

    [SerializeField] private float timeElapse = 0f;

    [SerializeField] private bool isProducing = false;

    #region MONO

    // Start is called before the first frame update
    void Start()
    {
        ChangeFaction();
        ProductionRemaining();
    }

    // Update is called once per frame
    void Update()
    {
        ProductionRemaining();
    }

    #endregion MONO

    #region METHODE
    private void ProductFinish()
    {
        spawnerComponent.SpawnUnit(unitToSpawn);
    }


    protected void ProductionRemaining()
    {
        if (!isProducing) return;
        else
        {
            if (timeElapse >= unitToSpawn.UnitData.ProductionTime)
            {
                ProductFinish();

                timeElapse = 0;
            }
            else
            {
                timeElapse += Time.deltaTime;
            }
        }
    }

    #endregion METHODE

    #region ABSTARCT METHODE



    #endregion METHODE

    #region ABSTARCT METHODE

    protected override void UpdateRateProduction()
    {
    }

    protected override void ChangeFaction()
    {

    }

    protected override void UpdateBuildingCapture()
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

    #endregion ABSTRACT METHODE
}