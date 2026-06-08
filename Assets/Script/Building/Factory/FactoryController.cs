using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryController : ABuildClass
{
    [Header("Component")]
    [SerializeField] private SpawnerComponent spawnerComponent= null;

    [SerializeField] protected EUnitType _unitProduction = EUnitType.NONE;


    [SerializeField] private AUnitClass unitToSpawn = null;

    [SerializeField] private float timeElapse = 0f;

    [SerializeField] private bool isProducing = false;


    #region METHODE
    #region MONO

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!placeableComponent.IsPlaced)
        {
            ProductionTimer();

        }
    }

    #endregion MONO
    #region ABSTARCT

    protected override void BuildDestroyed()
    {
        throw new System.NotImplementedException();
    }

    #endregion ABSTRACT 
    private void ProductFinish()
    {
        spawnerComponent.SpawnUnit(unitToSpawn);
    }


    private void ProductionTimer()
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

}