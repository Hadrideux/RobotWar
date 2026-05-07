using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManufactureController : ABuildClass
{
    
    #region ABSTRACT METHODE

    protected override void ChangeFaction()
    {
        switch (_buildingFaction)
        {
            case EFaction.ALLY:

                BaseManager.Instance.AllyRessource++;

                break;

            case EFaction.ENNEMY:

                BaseManager.Instance.EnnemyRessource++;

                break;

            default:

                break;
        }
    }

    protected override void UpdateBuildingCapture()
    {
        switch (_buildingType)
        {
            case EBuildingType.FACTORY:
                Debug.Log(_buildingType);

                break;

            case EBuildingType.PRODUCTION:
                Debug.Log(_buildingType);

                break;

            default:
                Debug.Log(_buildingType);

                break;
        }
    }
    protected override void UpdateRateProduction()
    {
        throw new System.NotImplementedException();
    }

    #endregion ASBTRACT METHODE
}

    