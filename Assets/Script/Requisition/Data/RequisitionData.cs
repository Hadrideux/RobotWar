using System;
using UnityEngine;

[Serializable]
public class RequisitionData
{
    [SerializeField] private int requisitionStock = 0;

    #region PROPERTIES

    public int RequisitionStock
    {
        get => requisitionStock;
        set
        {
            requisitionStock = Mathf.Clamp(value, 0, int.MaxValue);

            if (onUpdateRequisition != null)
            {
                onUpdateRequisition(requisitionStock);
            }
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
}
