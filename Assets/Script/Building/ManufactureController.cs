using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManufactureController : ABuildingBase
{
    [SerializeField] private float _productionComplete = 0f;
    
    #region MONO

    // Start is called before the first frame update
    void Start()
    {
        /*BuildingManager.Instance.OnManufactureAdd += IncreseRate;
        BuildingManager.Instance.ManufactureCount.Add(this);*/
    }

    // Update is called once per frame
    void Update()
    {
        ProductionTime();
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
    }

    #endregion METHODE ABSTRACT
}
