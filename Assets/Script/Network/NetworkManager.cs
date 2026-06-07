using System;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager>
{
    #region ATTRIBUTS
    [Header("Network Charge")]
    [SerializeField] private int defaultNetworkLoad = 100;
    [SerializeField] private int currentMaxLoad = 0;
    [SerializeField] private int currentLoad = 0;

    [SerializeField] private NetworkEffectData[] networkEffect = null;

    #endregion ATTRIBUTS

    #region PROPERTIES
    public int DefaultLoad => defaultNetworkLoad;

    public int CurrentMaxLoad
    {
        get => currentMaxLoad;
        set => currentMaxLoad = value;
    }
    public int CurrentLoad
    {
        get => currentLoad;
        set
        {
            currentLoad = value;

            if (onLoadChange != null)
                onLoadChange(currentLoad);

        }
    }
    public NetworkEffectData[] NetworkEffect => networkEffect;

    #endregion PROPERTIES

    #region EVENT
    private event Action<int> onLoadChange = null;
    public event Action<int> OnLoadChange
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

    private event Action<float> _onCoolingTriggered = null;
    public event Action<float> OnCoolingTriggered
    {
        add
        {
            _onCoolingTriggered -= value;
            _onCoolingTriggered += value;
        }
        remove
        {
            _onCoolingTriggered -= value;
        }
    }

    private event Action _onOverclockTriggered = null;
    public event Action OnOverclockTriggered
    {
        add
        {
            _onOverclockTriggered -= value;
            _onOverclockTriggered += value;
        }
        remove
        {
            _onOverclockTriggered -= value;
        }
    }
    #endregion EVENT

    public void UpdateNetworkLoad(int load)
    {
        currentLoad += load;
    }

    public void IncreaseNetworkLoad(int load)
    {
        currentMaxLoad += load;
    }
    public void DecreseNetworkLoad(int load)
    {
        currentMaxLoad -= load;
    }
}
