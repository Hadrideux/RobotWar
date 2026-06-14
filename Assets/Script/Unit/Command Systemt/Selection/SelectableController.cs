using System.Collections.Generic;
using UnityEngine;

public class SelectableController : MonoBehaviour
{
    #region ATTRIBUTS
    [SerializeField] private PlayerInteraction playerInteraction = null;

    [SerializeReference] private List<ISelectable> selectableObj = new List<ISelectable>();

    #endregion

    #region PROPERTY
    public IReadOnlyList<ISelectable> SelectableObj => selectableObj;
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
    public void ClickSelect(ISelectable unitToAdd, bool isShifted)
    {
        if(isShifted)
        {
            ShiftClickSelect(unitToAdd);
        }
        else
        {
            DeselectAll();
            selectableObj.Add(unitToAdd);
            unitToAdd.Select();
        }
    }

    public void ShiftClickSelect(ISelectable unitToAdd)
    {
        if (!selectableObj.Contains(unitToAdd))
        {
            selectableObj.Add(unitToAdd);
            unitToAdd.Select();
        }
        else
        {
            unitToAdd.Deselect();
            selectableObj.Remove(unitToAdd);
        }
    }

    public void DargSelect(ISelectable unitToAdd)
    {
        if (!selectableObj.Contains(unitToAdd))
        {
            selectableObj.Add(unitToAdd);
            unitToAdd.Select();
        }
    }

    public void DeselectAll()
    {
        foreach (ISelectable unit in selectableObj)
        {
            unit.Deselect();
        }
        selectableObj.Clear();
    }

    public void Deselect(ISelectable unitToAdd)
    {
        unitToAdd.Deselect();
    }      

    #endregion
}