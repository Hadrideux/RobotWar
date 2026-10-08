using System;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager>
{
    #region ATTRIBUTS
    [SerializeField] private NetworkEffectData[] networkEffect = null;
    [SerializeField] private NetworkData playerNetwork = new NetworkData();
    [SerializeField] private NetworkData IANetwork = new NetworkData();

    #endregion ATTRIBUTS

    #region PROPERTIES
    public NetworkEffectData[] NetworkEffect => networkEffect;
    #endregion PROPERTIES

    #region EVENTS
    
    #endregion EVENTS

    #region METHODES
    #region MONO
    #endregion MONO
    public NetworkData GetNetworkData(EFactionType faction)
    {
        if (faction == EFactionType.IA)
        {
            return IANetwork;
        }
        return playerNetwork;
    }
    #endregion METHODES
}
