using UnityEngine;

public class MovingState : AUnitState
{
    [SerializeField] private Vector3 currentDestination = Vector3.zero;

    public MovingState() { }

    public MovingState(Vector3 destination)
    {
        currentDestination = destination;
    }
    public override void Enter(AUnitClass unit)
    {
        unit.NavMeshAgent.SetDestination(currentDestination);
    }
    public override void Update(AUnitClass unit)
    {
        if (!unit.NavMeshAgent.pathPending && unit.NavMeshAgent.remainingDistance <= unit.NavMeshAgent.stoppingDistance)
        {
            unit.ChangeState(new IdleState());
        }
    }

    public override void Exit(AUnitClass unit)
    {
        unit.Order = null;
        unit.NavMeshAgent.ResetPath();
    }

}