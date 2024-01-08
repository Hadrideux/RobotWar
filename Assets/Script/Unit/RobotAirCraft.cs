using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAirCraft : ARobotBase
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
