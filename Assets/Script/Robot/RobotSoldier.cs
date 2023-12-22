using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSoldier : ARobotBase
{
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void MovementUnit()
    {
        throw new System.NotImplementedException();
    }

    public override void ProductionCost()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage()
    {
        HealthUpdate(0);
        throw new System.NotImplementedException();
    }

}
