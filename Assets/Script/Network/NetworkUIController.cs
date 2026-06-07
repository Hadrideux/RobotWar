using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NetworkUIController : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager = null;

    [Header("Network Load UI")]
    [SerializeField] private Image loadImage = null;

    /*
    [Header("Network Heat UI")]
    [SerializeField] private Image networkHeatImage = null;
    [SerializeField] private Color networkHeatColor = Color.green;
    [SerializeField] private float networkHeatGauge = 0f;
    [SerializeField] private float networkHeatStart = 100;
    */

    #region MONO
    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.Instance;
        networkManager.OnLoadChange += UpdateNetworkLoad;

        loadImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
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
