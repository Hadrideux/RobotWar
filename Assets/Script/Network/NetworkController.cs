using UnityEngine;

public class NetworkController : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        networkManager = NetworkManager.Instance;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NetworkSwitchState()
    {

    }
     /*
    public void IncreaseNetworkLoad(int load)
    {
        networkManager.NetworkLoad += load; 
    }
    public void DecreaseNetworkLoad(int load)
    {
        networkManager.NetworkLoad -= load;
    }

    public void IncreaseNetworkHeat(float heat)
    {
        networkManager.NetworkhHeat += heat;
    }
    public void DecreaseNetworkHeat(float heat)
    {
        networkManager.NetworkhHeat -= heat;
    }
     */
}
