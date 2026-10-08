using Sirenix.OdinInspector;
using UnityEngine;

public class IANetworkController : MonoBehaviour
{
    #region ATTRIBUTS
    [ShowInInspector] private NetworkData network = null;
    #endregion ATTRIBUTS

    #region PROPERTIES

    #endregion PROPERTIES
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        network = NetworkManager.Instance.GetNetworkData(EFactionType.IA);
    }
}
