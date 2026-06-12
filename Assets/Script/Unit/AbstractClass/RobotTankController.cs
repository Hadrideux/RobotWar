
using UnityEngine;

public class RobotTankController : AUnitClass
{
    #region ATTRIBUTS

    #endregion

    #region METHODE
    #region MONO

    void Update()
    {
        if (!isFreezeUnit)
        {
            MovementUnit();
        }
    }

    #endregion
    #region ABSTRACT

    public override void MovementUnit()
    {
        if (!navMeshAgent.isStopped && navMeshAgent.remainingDistance == 0)
        {
            float randomPoint = Random.Range(0, 50);
            float randomPoint2 = Random.Range(0, 50);
            Vector3 destination = new Vector3(randomPoint, transform.position.y, randomPoint2);
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
