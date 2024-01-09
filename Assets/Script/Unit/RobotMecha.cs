using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotMecha : ARobotBase
{
    private void Start()
    {
        MovementUnit();
    }

    private void Update()
    {
        
    }

    #region METHODE ABSTRACT

    public override void AttackDamage()
    {

    }

    public override void MovementUnit()
    {
        float randomPoint = Random.Range(0, 100);
        float randomPoint2 = Random.Range(0, 100);
        Vector3 destination = new Vector3(randomPoint, transform.position.y, randomPoint2);
        GetComponent<NavMeshAgent>().SetDestination(destination);
    }

    public override void TakeDamage()
    {
        HealthUpdate(0);
    }

    #endregion METHODE ABSTRACT
}
