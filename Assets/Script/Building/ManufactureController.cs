using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManufactureController : ABuildingBase
{
    #region MONO

    // Start is called before the first frame update
    void Start()
    {
        BuildingManager.Instance.OnManufactureAdd += IncreseRate;
        BuildingManager.Instance.ManufactureCount.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        ProductionRate();
    }

    private void OnDestroy()
    {
        BuildingManager.Instance.OnManufactureAdd -= IncreseRate;
    }
    private void OnApplicationQuit()
    {
        BuildingManager.Instance.OnManufactureAdd -= IncreseRate;
    }

    #endregion MONO

    #region METHODE ABRSTRACT
    public override void IncreseRate()
    {
        for (int i = 0; i < BuildingManager.Instance.ManufactureCount.Count; i++)
        {
            _productionRate = BuildingManager.Instance.ManufactureCount.Count;
        }
    }
   
    public override void ProductionRate()
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
        BaseManager.Instance.RessourCount++;
    }


    public override void BuildingCaptured()
    {

    }


    #endregion METHODE ABSTRACT
}
