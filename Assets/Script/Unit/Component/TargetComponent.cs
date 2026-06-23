using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TargetComponent : MonoBehaviour
{
    [SerializeField] private AUnitClass unit = null;
    [SerializeField] private LayerMask layerMask = 0;

    [SerializeReference] private List<ITargetableObject> targetAcquired = new List<ITargetableObject>();

    [Header("Unit Debug")]
    [SerializeField] protected bool isPeacfully = false;

    public List<ITargetableObject> TargetAcquired
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
        if (!isPeacfully) 
            ScanForTarget();
    }
    void OnDestroy()
    {
        UnitManager.Instance.OnUnitDestroyed -= RefreshTargetList;
    }

    private void OnApplicationQuit()
    {
        UnitManager.Instance.OnUnitDestroyed -= RefreshTargetList;
    }
    #endregion

    public void ScanForTarget()
    {
        Collider[] targetCol = Physics.OverlapSphere(transform.position, unit.UnitData.AttackRange, layerMask);

        List<ITargetableObject> removeTargetColl = new List<ITargetableObject>();
        bool isFound = false;

        foreach (ITargetableObject target in targetAcquired)
        {
            isFound = false;

            for (int i = 0; i < targetCol.Length; i++)
            {
                if (target == targetCol[i].gameObject.GetComponent<ITargetableObject>())
                {
                    isFound = true;
                }
            }

            if (isFound == false)
            {
                removeTargetColl.Add(target);
            }
        }

        for (int i = 0; i < removeTargetColl.Count; i++)
        {
            targetAcquired.Remove(removeTargetColl[i]);
        }

        for (int i = 0; i < targetCol.Length; i++)
        {
            ITargetableObject targetScanned = targetCol[i].gameObject.GetComponent<ITargetableObject>();

            if (targetScanned != unit as ITargetableObject)
            {
                if (targetScanned.ObjectFaction != unit.ObjectFaction && !targetAcquired.Contains(targetScanned))
                {
                    Debug.Log(targetScanned.ToString());
                    targetAcquired.Add(targetScanned);
                }
            }
        }
        TargetUnit();
    }
    public void TargetUnit()
    {
        AUnitClass unitTemp = null;
        ABuildClass buildTemp = null;

        foreach (ITargetableObject unitTargeted in targetAcquired)
        {
            if(unitTargeted as AUnitClass)
            {
                unitTemp = unitTargeted as AUnitClass;
                float closestTarget = Vector3.Distance(transform.position, transform.position);

                if (closestTarget <= unit.UnitData.AttackRange)
                {
                    unit.TurnTurret(unitTemp.gameObject);
                }
            }
            else if(unitTargeted as ABuildClass)
            {
                buildTemp = unitTargeted as ABuildClass;
                float closestTarget = Vector3.Distance(transform.position, transform.position);

                if (closestTarget <= unit.UnitData.AttackRange && buildTemp.BuildType != EBuildType.SPACEELEVATOR)
                {
                    unit.TurnTurret(buildTemp.gameObject);
                }
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
