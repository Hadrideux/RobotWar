using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTank : ARobotBase
{
    #region METHODE ABSTRACT

    public override void AttackDamage()
    {

    }

    public override void MovementUnit()
    {

    }

    public override void TakeDamage()
    {
        HealthUpdate(0);
    }

    #endregion METHODE ABSTRACT
}
