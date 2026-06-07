using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotTank : AUnitClass
{
    private void Start()
    {
        base.InitUnit();
    }


    private void Update()
    {
        //base.Update();
        if(!freezeUnit)
        {
            targetComponent.ScanForTarget();
            MovementUnit();
        }
    }

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
}
