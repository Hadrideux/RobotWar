using System;
using System.Diagnostics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    #region ATTRIBUTS

    [SerializeField] private EGameState currentGameState = EGameState.NONE;
    #endregion ATTRIBUTS
    #region PROPERTIES

    #endregion PROPERTIES

    #region EVENTS

    #endregion EVENTS
    #region METHODES
    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion MONO

    public void HandleGameState(EGameState state)
    {
        currentGameState = state;

        switch(currentGameState)
        {
            case EGameState.INMENU:
                break;
            case EGameState.PLAY:
                break;
            case EGameState.PAUSE:
                break;
            default:
                break;
        }
    }
    #endregion METHODES
}

public enum EGameState
{
    NONE,
    INMENU,
    PLAY,
    PAUSE,

}
