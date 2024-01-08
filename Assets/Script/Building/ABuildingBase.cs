using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABuildingBase : MonoBehaviour
{
    [Header("Building Type")]
    [SerializeField] protected EBuildingType _buildingType = EBuildingType.NONE;
    [SerializeField] protected EBuildingFaction _buildingFaction = EBuildingFaction.NONE;

    [Header("Building Production")]
    [SerializeField] protected int _productionRate = 1;
    [SerializeField] protected float _productionTime = 0;

    [Header("Tier Level")]
    [SerializeField] protected int _tierLevel = 0;

    [Header("Capture")]
    [SerializeField] protected float _captureTime = 0;

    abstract public void ProductionType();

    abstract public void ProductionTime();

    abstract public void IncreseRate();

    abstract public void BuildingCaptured();

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
