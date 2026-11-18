using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class NetworkUIController : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private NetworkManager networkManager = null;
    [Space(5)]
    [Header("HUD")]
    [SerializeField] private UIDocument HUDInterface = null;

    #region MONO
    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.Instance;
        networkManager.GetNetworkData(EFactionType.ALLY).OnLoadChange += UpdateNetworkLoad;

        HUDInterface.rootVisualElement.Q<RadialProgress>("NetworkLoad").Progress = 0;
    }

    private void OnDestroy()
    {
        networkManager.GetNetworkData(EFactionType.ALLY).OnLoadChange -= UpdateNetworkLoad;
    }
    private void OnApplicationQuit()
    {
        networkManager.GetNetworkData(EFactionType.ALLY).OnLoadChange -= UpdateNetworkLoad;
    }

    #endregion MONO
    public void UpdateNetworkLoad(float loadValue)
    {
        HUDInterface.rootVisualElement.Q<RadialProgress>("NetworkLoad").Progress = loadValue;
    }
}
