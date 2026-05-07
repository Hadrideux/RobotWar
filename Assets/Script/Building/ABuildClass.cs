using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABuildClass : MonoBehaviour
{
    #region ATTRIBUTS

    [Header("Buildding Type")]
    [SerializeField] protected EBuildingType _buildingType = EBuildingType.NONE;
    [SerializeField] protected EFaction _buildingFaction = EFaction.NONE;
    
    [Header("Production")]
    [SerializeField] protected SpawnerComponent spawnerComponent = null;


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

    abstract protected void UpdateRateProduction();

    abstract protected void UpdateBuildingCapture();

    abstract protected void ChangeFaction();

    #endregion METHODE
}
