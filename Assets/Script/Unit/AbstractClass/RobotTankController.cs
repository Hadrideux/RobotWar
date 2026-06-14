
using UnityEngine;

public class RobotTankController : AUnitClass
{
    #region ATTRIBUTS
    #endregion

    #region METHODE
    #region MONO

    void Update()
    {
        
    }

    #endregion
    #region ABSTRACT

    public override void MovementUnit(Vector3 destination)
    {
        if (!isFreezeUnit)
        {
            navMeshAgent.SetDestination(destination);
        }
    }

    public override void TakeDamage(AmmoData hitData)
    {
        switch(hitData.AmmoType)
        {
            case (EAmmoType.PHYSIQUE):
                HealthUpdate(hitData.Damage);

                if(currentHealth <= 0)
                {
                    UnitDestroyed();
                }
                break;

            default:
                break;
        }
    }

    #endregion  ABSTRACT

    #endregion


}
