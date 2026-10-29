using System;
using UnityEngine;

public class RequisitionManager : Singleton<RequisitionManager>
{
    #region ATTRIBUTS

    [Header("Requisition Controller")]
    [SerializeField] private RequisitionData playerRequisition = new RequisitionData();    
    [SerializeField] private RequisitionData IARequisition = new RequisitionData();

    #endregion  ATTRIBUTS

    #region PROPERTIES
    #endregion PROPERTIES

    #region EVENT
    #endregion EVENT

    #region METHODES
    #region MONO
    #endregion MONO

    public RequisitionData GetRequisitionData(EFactionType faction)
    {
        switch(faction)
        {
            case EFactionType.ALLY:
                return playerRequisition;
            case EFactionType.IA:
                return IARequisition;
            default:
                break;
        }

        return null;        
    }
    #endregion METHODES

}
