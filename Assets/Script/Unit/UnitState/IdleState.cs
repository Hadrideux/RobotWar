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
        if (unit.ScanningComponent.TargetedObject.Count > 0)
        {
            unit.ChangeState(new AttackState(unit.ScanningComponent.TargetedObject));
        }
    }

    public override void Exit(AUnitClass unit)
    {
    }
}