using System;
using UnityEngine;

public class RequisitionManger : Singleton<RequisitionManger>
{
    #region ATTRIBUTS
    [SerializeField] private int requisitionStock = 0;
    #endregion  ATTRIBUTS

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

    public void AddRequisition(int amount)
    {
        requisitionStock += amount;

        if(onUpdateRequisition != null)
        {
            onUpdateRequisition(amount);
        }
    }

    public void RemoveRequisition(int amount) 
    {
        requisitionStock -= amount;

        if (onUpdateRequisition != null)
        {
            onUpdateRequisition(amount);
        }
    }

}
