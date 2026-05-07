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
        if(!freezeUnit)
        {
            MovementUnit();
        }
    }

    #region METHODE ABSTRACT

    public override void AttackDamage()
    {
        
    }

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

    public override void TakeDamage()
    {
        HealthUpdate(0);
    }

    #endregion METHODE ABSTRACT
}
