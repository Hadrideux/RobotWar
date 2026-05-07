using System;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager>
{
    #region ATTRIBUTS
    [Header("Network Charge")]
    [SerializeField] private int maxNetworkLoad = 100;
    [SerializeField] private int currentNetworkLoad = 0;

    [Header("Network Heat")]
    [SerializeField] private int maxNetworkHeat = 100;
    [SerializeField] private int currenNetworktHeat = 0;

    #endregion ATTRIBUTS

    #region PROPERTIES
    public int MaxLoad
    {
        get => maxNetworkLoad;
        set => maxNetworkLoad = value;
    }
    public int NetworkLoad
    {
        get => currentNetworkLoad;
        set
        {
            currentNetworkLoad = Mathf.Clamp(value, 0, maxNetworkLoad);
            _onLoadChange();

        }
    }
    public int MaxHeat => maxNetworkHeat;
    public int NetworkhHeat
    {
        get => currenNetworktHeat;
        set
        {
            currenNetworktHeat = Mathf.Clamp(value, 0, maxNetworkHeat);
            _onHeatChange();

        }

    }
    #endregion PROPERTIES

    #region EVENT
    private event Action _onLoadChange = null;
    public event Action OnLoadChange
    {
        add
        {
            _onLoadChange -= value;
            _onLoadChange += value;
        }

        remove
        {
            _onLoadChange -= value;
        }
    }

    private event Action _onHeatChange = null;
    public event Action OnHeatChange
    {
        add
        {
            _onLoadChange -= value;
            _onLoadChange += value;
        }

        remove
        {
            _onLoadChange -= value;
        }
    }

    private event Action<float> _onOverHeatTriggered = null;
    public event Action<float> OnOverHeatTriggered
    {
        add
        {
            _onOverHeatTriggered -= value;
            _onOverHeatTriggered += value;
        }
        remove
        {
            _onOverHeatTriggered -= value;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
