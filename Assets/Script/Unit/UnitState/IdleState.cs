using System.Collections;
using UnityEngine;

public class IdleState : AUnitState
{
    public override void Enter(AUnitClass unit)
    {
        unit.NavMeshAgent.ResetPath();
    }

    public override void Update(AUnitClass unit)
    {
    }

    public override void Exit(AUnitClass unit)
    {
    }
}