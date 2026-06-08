using System.Collections.Generic;
using UnityEngine;

public class TargetComponent : MonoBehaviour
{
    [SerializeField] private AUnitClass unit = null;
    [SerializeField] private LayerMask layerMask = 0;

    [SerializeField] private List<AUnitClass> targetAcquired = new List<AUnitClass>();

    public List<AUnitClass> TargetAcquired
    {
        get => targetAcquired;
        set => targetAcquired = value;
    }

    #region METHODE

    #region MONO
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (Application.isPlaying)
            // Draw a sphere where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireSphere(transform.position, unit.UnitData.AttackRange);
    }
    void Start()
    {
        UnitManager.Instance.OnUnitDestroyed += RefreshTargetList;
    }

    private void Update()
    {
        if(!unit.IsFreezeUnit) ScanForTarget();
    }
    /*
    void OnDestroy()
    {
        UnitManager.Instance.OnUnitDestroyed -= RefreshTargetList;
    }
     */
    private void OnApplicationQuit()
    {
        UnitManager.Instance.OnUnitDestroyed -= RefreshTargetList;
    }
    #endregion

    public void ScanForTarget()
    {
        Collider[] targetCol = Physics.OverlapSphere(transform.position, unit.UnitData.AttackRange, layerMask);

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
            float closestTarget = Vector3.Distance(transform.position, transform.position);
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

    #endregion






}
