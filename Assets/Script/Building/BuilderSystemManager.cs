using System;
using UnityEngine;

public class BuilderSystemManager : Singleton<BuilderSystemManager>
{
    #region ATTRIBUTS

    [Header("Production")]
    [SerializeField] private ABuildClass selectedBuild = null;



    #endregion ATTRIBUTS

    #region PROPERTIES
    #endregion PROPERTIES

    #region EVENT

    private event Action<ABuildClass> onBuildSelected = null;
    public event Action<ABuildClass> OnBuildSelected
    {
        add
        {
            onBuildSelected -= value;
            onBuildSelected += value;
        }

        remove
        {
            onBuildSelected -= value;
        }
    }

    #endregion EVENT

    #region METHODES

    #region MONO
    void Start()
    {
        PlayerInteraction.Instance.OnConfirmPlacement += BuildCanceled;
    }
    #endregion MONO
    public void SelectedBuild(ABuildClass prefab)
    {
        selectedBuild = prefab;

        if (onBuildSelected != null)
        {
            onBuildSelected(prefab);
        }
    }

    public void BuildCanceled()
    {
        selectedBuild = null;
    }
    #endregion METHODES
}