using UnityEngine;

public abstract class AUnitState
{
    public abstract void Enter(AUnitClass unit);
    public abstract void Update(AUnitClass unit);
    public abstract void Exit(AUnitClass unit);
}