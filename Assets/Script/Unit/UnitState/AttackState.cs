using UnityEngine;

public class AttackState : AUnitState
{
    [SerializeField] private GameObject currentTarget = null;

    public AttackState(GameObject target)
    {
        currentTarget = target;
    }

    public override void Enter(AUnitClass unit)
    {
        unit.NavMeshAgent.SetDestination(currentTarget.transform.position);
    }

    public override void Update(AUnitClass unit)
    {
        Debug.Log("Attack State");

        if (currentTarget == null)
        {
            unit.NavMeshAgent.ResetPath();
            unit.ChangeState(new IdleState());
            return;
        }
        else if (currentTarget != null)
        {
            float targetDistance = Vector3.Distance(currentTarget.transform.position, unit.transform.position);

            if (targetDistance <= unit.UnitData.AttackRange)
            {
                if (unit.NavMeshAgent.isStopped == false)
                    unit.NavMeshAgent.ResetPath();

                unit.TargetUnit();
            }
            else
            {
                unit.NavMeshAgent.SetDestination(currentTarget.transform.position);
            }
        }
    }

    public override void Exit(AUnitClass unit)
    {
        unit.Order = null;
    }
}