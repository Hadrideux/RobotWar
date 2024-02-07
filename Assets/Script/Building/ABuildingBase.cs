using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABuildingBase : MonoBehaviour
{
    #region ATTRIBUTS

    [Header("Buildding Type")]
    [SerializeField] protected EBuildingType _buildingType = EBuildingType.NONE;
    [SerializeField] protected EFaction _buildingFaction = EFaction.NONE;

    [Header("Building Level")]
    [SerializeField] protected int _buildingLevel = 0;
    
    [Header("Production")]
    [SerializeField] protected int _productionRate = 1;
    [SerializeField] protected float _productionTime = 0;


    [Header("Capture")]
    [SerializeField] protected float _captureTime = 0;

    #endregion ATTRIBUTS

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

    #region METHODE

    abstract protected void UpdateProductionTime();

    abstract protected void UpdateRateProduction();

    abstract protected void UpdateBuildingCapture();

    abstract protected void ChangeFaction();

    #endregion METHODE
}
