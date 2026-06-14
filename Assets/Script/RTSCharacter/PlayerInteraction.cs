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

    [SerializeField] private LayerMask interactionMask = new LayerMask();
    [SerializeField] private LayerMask groundMask = new LayerMask();

    #endregion ATTRIBUTS

    #region PROPERTIES
    public LayerMask GroundMask => groundMask;
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

    private event Action onCancelAction = null;
    public event Action OnCancelAction
    {
        add
        {
            onCancelAction -= value;
            onCancelAction += value;
        }

        remove
        {
            onCancelAction -= value;
        }
    }

    private event Action<Vector2> onDragStarted = null;
    public event Action<Vector2> OnDragStarted
    {
        add
        {
            onDragStarted -= value;
            onDragStarted += value;
        }

        remove
        {
            onDragStarted -= value;
        }
    }
    private event Action<Vector2> onDragUpdate = null;
    public event Action<Vector2> OnDragUpdate
    {
        add
        {
            onDragUpdate -= value;
            onDragUpdate += value;
        }

        remove
        {
            onDragUpdate -= value;
        }
    }
    private event Action onDragReleased = null;
    public event Action OnDragReleased
    {
        add
        {
            onDragReleased -= value;
            onDragReleased += value;
        }

        remove
        {
            onDragReleased -= value;
        }
    }

    private event Action<Vector3> onMoveOrder = null;
    public event Action<Vector3> OnMoveOrder
    {
        add
        {
            onMoveOrder -= value;
            onMoveOrder += value;
        }
        remove
        {
            onMoveOrder -= value;
        }
    }

    private event Action<ISelectable, bool> onGameObjectSelected = null;
    public event Action<ISelectable, bool> OnGameObjectSelected
    {
        add
        {
            onGameObjectSelected -= value;
            onGameObjectSelected += value;
        }
        remove
        {
            onGameObjectSelected -= value;
        }
    }

    #endregion EVENT

    #region MONO
    void Update()
    {
        PlayerInput();            
    }
    #endregion MONO

    #region METHODE

    public void ChangePlayerState(EPlayerState newState)
    {
        currentState = newState;
    }

    public RaycastHit GetMouseWorlPosition(LayerMask interactionMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 5000f, interactionMask))
        {
            return raycastHit;
        }
        else
        {
            return new RaycastHit();
        }
    }

    private void PlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (currentState)
            {
                case EPlayerState.COMMAND:
                    if (onDragStarted != null)
                    {
                        onDragStarted(Input.mousePosition);
                    }

                    SelectionInput();

                    break;
                case EPlayerState.CONSTURCTION:
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
        else if (Input.GetMouseButton(0) && currentState == EPlayerState.COMMAND)
        {
            if (onDragUpdate != null)
            {
                onDragUpdate(Input.mousePosition);
            }
        } 
        else if (Input.GetMouseButtonUp(0) && currentState == EPlayerState.COMMAND)
        {
            if (onDragReleased != null)
            {
                onDragReleased();
            }
        }

        if(Input.GetMouseButtonDown(1) && currentState == EPlayerState.COMMAND)
        {
            RaycastHit hit = GetMouseWorlPosition(groundMask);

            if (onMoveOrder != null && hit.collider != null)
            {
                onMoveOrder(hit.point);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            CancelPlayerAction();
        }
    }

    private void SelectionInput()
    {
        RaycastHit hit = GetMouseWorlPosition(interactionMask);

        if(hit.collider != null)
        {
            GameObject obj = hit.collider.gameObject;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //Shift clicked
                if (onGameObjectSelected != null && obj.GetComponent<ISelectable>() != null)
                {
                    onGameObjectSelected(obj.GetComponent<ISelectable>(), true);
                }
            }
            else
            {
                //Normal clicked
                if (onGameObjectSelected != null && obj.GetComponent<ISelectable>() != null)
                {
                    onGameObjectSelected(obj.GetComponent<ISelectable>(), false);
                }
            }
        }
    }

    private void CancelPlayerAction()
    {
        if (onCancelAction != null)
        {
            onCancelAction();
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
