using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryController : ABuildingBase
{

    [SerializeField] protected int _productionCost = 0;

    #region MONO

    // Start is called before the first frame update
    void Start()
    {
        BuildingManager.Instance.OnFactoryAdd += IncreseRate;
        BuildingManager.Instance.FactoryCount.Add(this);
    }
    // Update is called once per frame
    void Update()
    { 
        
        ProductionRate();

    }
    private void OnDestroy()
    {
        BuildingManager.Instance.OnFactoryAdd -= IncreseRate;
    }
    private void OnApplicationQuit()
    {
        BuildingManager.Instance.OnFactoryAdd -= IncreseRate;
    }

    #endregion MONO

    #region METHODE ABRSTRACT
    public override void IncreseRate()
    {
        _productionRate += BuildingManager.Instance.FactoryCount.Count;
    }
    public override void ProductionType()
    {
        BaseManager.Instance.RessourCount -= _productionCost;        
    }
    public override void ProductionRate()
    {
        _productionTime += Time.deltaTime * _productionRate;

        if (_productionTime > _productionComplete)
        {
            if (BaseManager.Instance.RessourCount >= _productionCost)
            {
                ProductionType();
            }
            
            _productionTime = 0;
        }
    }
    public override void BuildingCaptured()
    {

    }

    

    #endregion METHODE ABSTRACT
}