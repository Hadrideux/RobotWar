
using UnityEngine;

public class RobotTankController : AUnitClass
{
    #region ATTRIBUTS
    #endregion

    #region METHODE
    #region MONO
    #endregion
    #region ABSTRACT

    public override void TakeDamage(AmmoData hitData)
    {
        switch (hitData.AmmoType)
        {
            case (EAmmoType.PHYSIQUE):
                HealthUpdate(hitData.Damage);

                if (currentHealth <= 0)
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
