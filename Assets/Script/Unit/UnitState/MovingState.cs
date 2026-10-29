using UnityEngine;
using UnityEngine.AI;

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
        if (NavMesh.SamplePosition(currentDestination, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {            
            unit.NavMeshAgent.SetDestination(hit.position);
        }
        else
        {
            unit.NavMeshAgent.ResetPath();
        }

    }
    public override void Update(AUnitClass unit)
    {
        if (unit.NavMeshAgent.pathPending)
            return;

        if (unit.NavMeshAgent.remainingDistance <= unit.NavMeshAgent.stoppingDistance)
        {
            unit.ChangeState(new IdleState());
        }
    }

    public override void Exit(AUnitClass unit)
    {
        unit.NavMeshAgent.ResetPath();
    }

}