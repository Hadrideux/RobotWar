using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NetworkUIController : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager = null;

    [Header("Network Load UI")]
    [SerializeField] private Image networkLoadImage = null;
    [SerializeField] private int networkLoadGauge = 0;

    [SerializeField] private int networkLoadStart = 100;

    [Header("Network Heat UI")]
    [SerializeField] private Image networkHeatImage = null;
    [SerializeField] private Color networkHeatColor = Color.green;
    [SerializeField] private float networkHeatGauge = 0f;

    [SerializeField] private float networkHeatStart = 100;


    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.Instance;

        networkManager.OnLoadChange += UpdateNetworkLoad;
        networkManager.OnHeatChange += UpdateNetworkHeat;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDestroy()
    {
        networkManager.OnLoadChange -= UpdateNetworkLoad;
        networkManager.OnHeatChange -= UpdateNetworkHeat;
    }

    void OnApplicationQuit()
    {
        networkManager.OnLoadChange -= UpdateNetworkLoad;
        networkManager.OnHeatChange -= UpdateNetworkHeat;
    }

    public void UpdateNetworkLoad()
    {
        int currentLoad = networkManager.NetworkLoad;
        networkLoadImage.fillAmount = Mathf.Clamp(currentLoad, 0, networkLoadStart);
    }
                      
    public void UpdateNetworkHeat()
    {
        int currentHeat = networkManager.NetworkhHeat;
        networkHeatImage.fillAmount = Mathf.Clamp(currentHeat, 0, networkHeatStart);
    }

}
