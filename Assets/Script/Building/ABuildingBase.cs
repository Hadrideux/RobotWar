using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABuildingBase : MonoBehaviour
{
    [Header("Building Type")]
    [SerializeField] protected EBuildingType _buildingType = EBuildingType.NONE;
    [SerializeField] protected EFaction _buildingFaction = EFaction.NONE;

    [Header("Building Production")]
    [SerializeField] protected EUnitType _unitProduction = EUnitType.NONE;
    [SerializeField] protected int _productionRate = 1;
    [SerializeField] protected float _productionTime = 0;

    [Header("")]
    [SerializeField] private Transform _spawnUnit = null;
    [SerializeField] private Transform _robotContainer = null;

    [Header("Tier Level")]
    [SerializeField] protected int _tierLevel = 0;

    [Header("Capture")]
    [SerializeField] protected float _captureTime = 0;

    abstract protected void ProductionType();

    abstract protected void ProductionTime();

    abstract protected void IncreseRate();

    abstract protected void BuildingCaptured();

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
