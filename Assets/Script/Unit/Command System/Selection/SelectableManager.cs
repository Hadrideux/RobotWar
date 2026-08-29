using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectableManager : Singleton<SelectableManager>
{
    #region ATTRIBUTS
    [SerializeField] private PlayerInteraction playerInteraction = null;

    [SerializeReference] private List<ISelectable> selectableObj = new List<ISelectable>();

    #endregion

    #region PROPERTY
    public IReadOnlyList<ISelectable> SelectableObj => selectableObj;
    #endregion

    #region EVENT
    private event Action onUnitSelected = null;
    public event Action OnUnitSelected
    {
        add
        {
            onUnitSelected -= value;
            onUnitSelected += value;
        }
        remove
        {
            onUnitSelected -= value;
        }
    }
    private event Action<ABuildClass> onBuildingSelected = null;
    public event Action<ABuildClass> OnBuildingSelected
    {
        add
        {
            onBuildingSelected -= value;
            onBuildingSelected += value;
        }
        remove
        {
            onBuildingSelected -= value;
        }
    }
    private event Action onSelectionCleared = null;
    public event Action OnSelectionCleared
    {
        add
        {
            onSelectionCleared -= value;
            onSelectionCleared += value;
        }
        remove
        {
            onSelectionCleared -= value;
        }
    }
    #endregion

    #region METHODE
    #region MONO
    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInteraction = PlayerInteraction.Instance;

        playerInteraction.OnGameObjectSelected += ClickSelect;
        playerInteraction.OnCancelAction += DeselectAll;
    }
    private void OnDestroy()
    {
        playerInteraction.OnGameObjectSelected -= ClickSelect;
        playerInteraction.OnCancelAction -= DeselectAll;
    }
    private void OnApplicationQuit()
    {
        playerInteraction.OnGameObjectSelected -= ClickSelect;
        playerInteraction.OnCancelAction -= DeselectAll;
    }

    #endregion 

    public void ClickSelect(RaycastHit hit, bool isShifted)
    {
        if (hit.collider.GetComponent<ISelectable>() != null)
        {
            ISelectable selectionToAdd = hit.collider.GetComponent<ISelectable>();

            if (selectableObj.Count > 0 && selectionToAdd.SelectableType != selectableObj[0].SelectableType)
            {
                DeselectAll();
                SwitchSelectableObjList(selectionToAdd);
            }
            else if (isShifted && (selectableObj.Count == 0 || selectionToAdd.SelectableType == selectableObj[0].SelectableType))
            {
                ShiftClickSelect(selectionToAdd);
            }
            else
            {
                DeselectAll();
                SwitchSelectableObjList(selectionToAdd);
            }
        }
    }

    public void ShiftClickSelect(ISelectable selectionToAdd)
    {
        if (!selectableObj.Contains(selectionToAdd))
        {
            SwitchSelectableObjList(selectionToAdd);
        }
        else
        {
            selectionToAdd.Deselect();
            selectableObj.Remove(selectionToAdd);
        }
    }

    public void DargSelect(ISelectable selectionToAdd)
    {
        if (!selectableObj.Contains(selectionToAdd))
        {
            SwitchSelectableObjList(selectionToAdd);
        }
    }

    public void DeselectAll()
    {
        if (onSelectionCleared != null)
        {
            onSelectionCleared();
        }

        foreach (ISelectable selectionToAdd in selectableObj)
        {
            selectionToAdd.Deselect();
        }

        selectableObj.Clear();
    }

    public void Deselect(ISelectable selectionToAdd)
    {
        selectionToAdd.Deselect();
    }

    private void SwitchSelectableObjList(ISelectable selectionToAdd)
    {
        switch (selectionToAdd.SelectableType)
        {
            case ESelectableType.UNIT:
                selectableObj.Add(selectionToAdd);
                selectionToAdd.Select();

                if (onUnitSelected != null)
                {
                    onUnitSelected();
                }
                break;

            case ESelectableType.BUILDING:
                selectableObj.Add(selectionToAdd);
                selectionToAdd.Select();

                if (onBuildingSelected != null)
                {
                    onBuildingSelected(selectionToAdd as ABuildClass);
                }
                break;

            default:
                break;
        }
    }

    #endregion
}