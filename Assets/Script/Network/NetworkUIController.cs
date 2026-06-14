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
        networkManager.OnLoadChange += UpdateNetworkLoad;

        loadImage.fillAmount = 0;
    }

    private void OnDestroy()
    {
        networkManager.OnLoadChange -= UpdateNetworkLoad;
    }
    private void OnApplicationQuit()
    {
        networkManager.OnLoadChange -= UpdateNetworkLoad;
    }

    #endregion MONO
    public void UpdateNetworkLoad(int loadValue)
    {
        float fill = (float)loadValue / networkManager.CurrentMaxLoad;
        loadImage.fillAmount = Mathf.Clamp(fill, 0, 1);
    }
}
