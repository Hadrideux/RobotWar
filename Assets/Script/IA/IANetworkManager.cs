using UnityEngine;

public class IANetworkManager : Singleton<IANetworkManager>
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
        }
    }

    #endregion PROPERTIES
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
