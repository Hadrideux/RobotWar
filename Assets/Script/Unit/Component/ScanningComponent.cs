using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ScanningComponent : MonoBehaviour
{
    [SerializeField] private AUnitClass unit = null;
    [SerializeField] private LayerMask layerMask = 0;

    [SerializeReference] private List<ITargetableObject> targetedObject = new List<ITargetableObject>();

    [Header("Unit Debug")]
    [SerializeField] protected bool isPeacfully = false;

    public List<ITargetableObject> TargetedObject
    {
        get => targetedObject;
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

    private void Update()
    {
        if (!isPeacfully) 
            ScanForTarget();
    }
    #endregion

    public void ScanForTarget()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, unit.UnitData.AttackRange, layerMask);

        RefreshTargetList(col);        

        for (int i = 0; i < col.Length; i++)
        {
            ITargetableObject scannedObject = col[i].gameObject.GetComponent<ITargetableObject>();

            if (scannedObject != unit as ITargetableObject)
            {
                if (scannedObject.FactionObject != unit.FactionObject && !targetedObject.Contains(scannedObject))
                {
                    targetedObject.Add(scannedObject);
                }
            }
        }
    }

    public void RefreshTargetList(Collider[] col)
    {
        List<ITargetableObject> removeTargetColl = new List<ITargetableObject>();
        bool isFound = false;

        foreach (ITargetableObject target in targetedObject)
        {
            isFound = false;

            for (int i = 0; i < col.Length; i++)
            {
                if (target == col[i].gameObject.GetComponent<ITargetableObject>())
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
            targetedObject.Remove(removeTargetColl[i]);
        }
    }

    #endregion
}
