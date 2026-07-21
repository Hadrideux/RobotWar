using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AUnitState
{
    [SerializeField] private GameObject currentTarget = null;
    [SerializeReference] private List<ITargetableObject> targetAcquired = new List<ITargetableObject>();

    public AttackState()
    {
    }
    public AttackState(List<ITargetableObject> target)
    {
        targetAcquired = target;
    }

    public override void Enter(AUnitClass unit)
    {
        unit.NavMeshAgent.SetDestination(unit.Order.OrderTarget.TargetObject.transform.position);
    }

    public override void Update(AUnitClass unit)
    {

        if (currentTarget == null && targetAcquired.Count == 0)
        {
            unit.NavMeshAgent.ResetPath();
            unit.ChangeState(new IdleState());
            return;
        }
        
        float targetDistance = Vector3.Distance(unit.Order.OrderTarget.TargetObject.transform.position, unit.transform.position);

        if (targetAcquired.Count > 0)
        {
            TargetUnit(unit);
        }
        else if (targetDistance <= unit.UnitData.AttackRange)
        {
            unit.TurnTurret(unit.Order.OrderTarget.TargetObject);
        }
    }

    public override void Exit(AUnitClass unit)
    {
        unit.Order = null;
    }

    public void TargetUnit(AUnitClass unit)
    {
        foreach (ITargetableObject unitTargeted in unit.ScanningComponent.TargetedObject)
        {
            float closestTarget = Vector3.Distance(unitTargeted.TargetObject.transform.position, unit.transform.position);

            if (closestTarget <= unit.UnitData.AttackRange)
            {
                unit.TurnTurret(unitTargeted.TargetObject);
            }
        }
    }
}