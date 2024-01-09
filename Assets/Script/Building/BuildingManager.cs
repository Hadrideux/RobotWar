using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    #region ATTRIBUTS

    [Header("Building Count")]
    [SerializeField] private List<ManufactureController> _manufactureCount = new List<ManufactureController>();
    [SerializeField] private List<FactoryController> _factoryCount = new List<FactoryController>();

    #endregion ATTRIBUTS

    /*
    #region PROPERTIES

    public List<ManufactureController> ManufactureCount
    {
        get => _manufactureCount;
        set
        {
            _manufactureCount = value;
        }
    }

    public List<FactoryController> FactoryCount
    {
        get => _factoryCount;
        set
        {
            _factoryCount = value;              
        }
    }

    #endregion PROPERTIES
    */

    #region EVENT

    private event Action _onManufactureAdd = null;
    public event Action OnManufactureAdd
    {
        add
        {
            _onManufactureAdd -= value;
            _onManufactureAdd += value;
        }
        remove
        {
            _onManufactureAdd -= value;
        }
    }
    private event Action _onFactoryAdd = null;
    public event Action OnFactoryAdd
    {
        add
        {
            _onFactoryAdd -= value;
            _onFactoryAdd += value;
        }
        remove
        {
            _onFactoryAdd -= value;
        }
    }

    #endregion EVENT
    
    #region MONO

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion MONO
}