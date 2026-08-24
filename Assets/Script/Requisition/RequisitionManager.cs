using System;
using UnityEngine;

public class RequisitionManager : Singleton<RequisitionManager>
{
    #region ATTRIBUTS
    [SerializeField] private int requisitionStock = 0;
    #endregion  ATTRIBUTS

    #region PROPERTIES

    public int RequisitionStock
    {
        get => requisitionStock;
        set
        {
            requisitionStock = value;
            OnUpdateRequisitionEvent(value);
        }
    }
    #endregion

    #region EVENT

    private event Action<int> onUpdateRequisition;
    public event Action<int> OnUpdateRequisition
    {
        add
        {
            onUpdateRequisition -= value;
            onUpdateRequisition += value;
        }
        remove
        {
            onUpdateRequisition -= value;
        }
    }

    #endregion EVENT

    public void OnUpdateRequisitionEvent(int amount)
    {
        if (onUpdateRequisition != null)
        {
            onUpdateRequisition(amount);
        }
    }

}
