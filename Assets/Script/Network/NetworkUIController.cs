using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NetworkUIController : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager = null;

    [Header("Network Load UI")]
    [SerializeField] private Image loadImage = null;

    #region MONO
    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.Instance;
        networkManager.GetNetworkData(EFactionType.ALLY).OnLoadChange += UpdateNetworkLoad;

        loadImage.fillAmount = 0;
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
        loadImage.fillAmount = Mathf.Clamp(loadValue, 0, 1);
    }
}
