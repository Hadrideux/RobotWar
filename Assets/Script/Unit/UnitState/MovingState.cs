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
            NavMeshPath path = new NavMeshPath();
            bool found = NavMesh.CalculatePath(unit.transform.position, hit.position, NavMesh.AllAreas, path);
            Debug.Log($"{unit.name} : onNavMesh = {unit.NavMeshAgent.isOnNavMesh}, CalculatePath = {found}, " +
                      $"status = {path.status}, coins = {path.corners.Length}"); 
            
            bool ok = unit.NavMeshAgent.SetDestination(hit.position);
            Debug.Log($"{unit.name} MOVETO {currentDestination} → point NavMesh {hit.position} (SetDestination = {ok})");
        }
        else
        {
            unit.NavMeshAgent.ResetPath();
        }

    }
    public override void Update(AUnitClass unit)
    {
        if (!unit.NavMeshAgent.pathPending && unit.NavMeshAgent.remainingDistance <= unit.NavMeshAgent.stoppingDistance)
        {
            Debug.Log($"{unit.name} considéré arrivé : pathStatus = {unit.NavMeshAgent.pathStatus}, " +
          $"remaining = {unit.NavMeshAgent.remainingDistance}, hasPath = {unit.NavMeshAgent.hasPath}");
            unit.ChangeState(new IdleState());
        }
    }

    public override void Exit(AUnitClass unit)
    {
        unit.NavMeshAgent.ResetPath();
    }

}