using System;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{

    #region EVENTS
    private event Action<ABuildClass> onBuildingAttacked = null;
    public event Action<ABuildClass> OnBuildingAttacked
    {
        add
        {
            onBuildingAttacked -= value;
            onBuildingAttacked += value;
        }
        remove
        {
            onBuildingAttacked -= value;
        }
    }

    #endregion EVENTS

    #region METHODES
    public void BuildignAttacked(ABuildClass build)
    {
        if(onBuildingAttacked != null)
        {
            onBuildingAttacked(build);
        }
    }

    #endregion METHODES
}
