using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderManager : Singleton<BuilderManager>
{
    #region ATTRIBUTS

    [Header("Production")]
    [SerializeField] private SpawnerComponent spawnerComponent = null;

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

    #region MONO

    void Start()
    {
        PlayerInteraction.Instance.OnConfirmPlacement += BuildCanceled;
    }


    #endregion MONO
    public void SelectedBuild(ABuildClass prefab)
    {
        selectedBuild = prefab;
        PlayerInteraction.Instance.ChangePlayerState(EPlayerState.CONSTURCTION);
        if (onBuildSelected != null)
        {
            onBuildSelected(prefab);
        }
    }

    public void BuildCanceled()
    {
        selectedBuild = null;
    }

    

}