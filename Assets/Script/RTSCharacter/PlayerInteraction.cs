using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerInteraction : Singleton<PlayerInteraction>
{
    #region ATTRIBUTS

    [SerializeField] private EPlayerState currentState = EPlayerState.NONE;

    [SerializeField] private Camera cameraCharacter = null;

    #endregion ATTRIBUTS

    #region PROPERTIES
    #endregion PROPERTIES

    #region EVENT

    private event Action onConfirmPlacement = null;
    public event Action OnConfirmPlacement
    {
        add
        {
            onConfirmPlacement -= value;
            onConfirmPlacement += value;
        }

        remove
        {
            onConfirmPlacement -= value;
        }
    }
    private event Action onCancelPlacement = null;
    public event Action OnCancelPlacement
    {
        add
        {
            onCancelPlacement -= value;
            onCancelPlacement += value;
        }

        remove
        {
            onCancelPlacement -= value;
        }
    }

    #endregion EVENT

    #region MONO
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (currentState)
            {
                case EPlayerState.COMMAND:
                    break;
                case EPlayerState.CONSTURCTION:
                    Debug.Log("Confirm placement");
                    if (onConfirmPlacement != null)
                    {
                        onConfirmPlacement();
                    }
                    currentState = EPlayerState.COMMAND;
                    break;
                default:
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            switch (currentState)
            {
                case EPlayerState.COMMAND:
                    break;
                case EPlayerState.CONSTURCTION:
                    Debug.Log("Build Cancel");
                    if (onCancelPlacement != null)
                    {
                        onCancelPlacement();
                    }
                    currentState = EPlayerState.COMMAND;
                    break;
                default:
                    break;
            }
        }
            
    }
    #endregion MONO

    #region METHODE

    public void ChangePlayerState(EPlayerState newState)
    {
        currentState = newState;
    }

    public Vector3 GetMouseWorlPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    #endregion METHODE
}
public enum EPlayerState
{
    NONE,
    CONSTURCTION,
    COMMAND
}
