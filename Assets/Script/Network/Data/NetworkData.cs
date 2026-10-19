using System;
using UnityEngine;

[Serializable]
public class NetworkData
{
    #region ATTRIBUTS
    [Header("Network Charge")]
    [SerializeField] private int maxLoad = 0;
    [SerializeField] private int currentLoad = 0;
    #endregion ATTRIBUTS

    #region PROPERTIES
    public int MaxLoad
    {
        get => maxLoad;
        set
        {
            maxLoad = value;
            if(onLoadChange != null)
            {
                onLoadChange(PercentLoad);
            }
        }
    }
    public int CurrentLoad
    {
        get => currentLoad;
        set
        {
            if(currentLoad == value)
            {
                return;
            }

            currentLoad = Mathf.Clamp(value, 0, int.MaxValue);
            
            if (onLoadChange != null)
            {
                onLoadChange(PercentLoad);
            }
        }
    }
    public float PercentLoad => (float)currentLoad / maxLoad;
    #endregion PROPERTIES

    #region EVENTS
    private event Action<float> onLoadChange = null;
    public event Action<float> OnLoadChange
    {
        add
        {
            onLoadChange -= value;
            onLoadChange += value;
        }
        remove
        {
            onLoadChange -= value;
        }
    }
    #endregion EVENTS    
}
