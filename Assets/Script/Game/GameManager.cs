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

    private event Action<EGameState> onGameStateChange = null;
    public event Action<EGameState> OnGameStateChanged
    {
        add
        {
            onGameStateChange -= value;
            onGameStateChange += value;
        }
        remove
        {
            onGameStateChange -= value;
        }
    }

    #endregion EVENTS
    #region METHODES
    #region MONO
    public void Start()
    {
        currentGameState = EGameState.INMENU;
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

            case EGameState.QUIT: 
                break;

            case EGameState.VICTORY: 
                break;

            case EGameState.DEFEAT: 
                break;
            
            default:
                break;
        }
    }

    public void OnGameState(EGameState state)
    {
        currentGameState = state;

        if (onGameStateChange != null)
        {
            onGameStateChange(state);
        }

        HandleGameState(state);
    }
    #endregion METHODES
}

public enum EGameState
{
    NONE,
    INMENU,
    PLAY,
    PAUSE,
    QUIT,
    VICTORY,
    DEFEAT,

}
