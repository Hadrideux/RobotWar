using System.Collections.Generic;
using UnityEngine;

public class TargetComponent : MonoBehaviour
{
    [SerializeField] private AUnitClass unit = null;
    [SerializeField] protected LayerMask[] layerMask = null;

    [SerializeField] protected List<AUnitClass> targetAcquired = new List<AUnitClass>();

    public List<AUnitClass> TargetAcquired
    {
        get => targetAcquired;
        set => targetAcquired = value;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (Application.isPlaying)
            // Draw a sphere where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireSphere(transform.position, unit.UnitData.AttackRange);
    }
    public void ScanForTarget()
    {
        Collider[] targetCol = Physics.OverlapSphere(transform.position, unit.UnitData.AttackRange, layerMask[0]);

        for (int i = 0; i < targetCol.Length; i++)
        {
            AUnitClass unitScanned = targetCol[i].gameObject.GetComponent<AUnitClass>();
            if (!targetAcquired.Contains(unitScanned) && unitScanned != unit)
            {
                targetAcquired.Add(unitScanned);
            }
        }
        
        TargetUnit();
    }
    public void TargetUnit()
    {
        foreach (AUnitClass unitTargeted in targetAcquired)
        {
            float closestTarget = Vector3.Distance(unit.transform.position, transform.position);
            if (closestTarget <= unitTargeted.UnitData.AttackRange)
            {
                unit.TurnTurret(unitTargeted);                 
            }
        }
    }
    public void RefreshTargetList(AUnitClass unit)
    {
            targetAcquired.Remove(unit);
        if (targetAcquired.Contains(unit))
        {
        }
    }
}
