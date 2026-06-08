using System.Collections;
using UnityEngine;
    
public class Warehouse : ABuildClass  
{
    [Header("Manager")]
    [SerializeField] private RequisitionManager requisitionManger = null;
    [SerializeField] private PlayerInteraction playerInteraction = null;

    [Header("Linked Tower")]
    [SerializeField] private SupplyTower registeredTower = null;
    [SerializeField] private LayerMask layerMask;

    [Header("Stat")]
    [SerializeField] private int requisitionBonus = 0;

    #region METHODE
    #region MONO

    void Start()
    {
        playerInteraction = PlayerInteraction.Instance;

        playerInteraction.OnConfirmPlacement += FindAndRegisterTower;
    }

    private void OnApplicationQuit()
    {
        playerInteraction.OnConfirmPlacement -= FindAndRegisterTower;
    }

    private void OnDestroy()
    {
        playerInteraction.OnConfirmPlacement -= OnPlacemenConfirmed;

        if (registeredTower != null)
            registeredTower.RemoveWarehouseBonus(requisitionBonus);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (Application.isPlaying)
            // Draw a sphere where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    #endregion MONO
    #region ABSTRACT
    protected override void BuildDestroyed()
    {
        Destroy(gameObject);
    }
    #endregion ABSTRACT
    
    private void OnPlacemenConfirmed()
    {
        FindAndRegisterTower();
        playerInteraction.OnConfirmPlacement -= FindAndRegisterTower;
    }

    private void FindAndRegisterTower()
    {
        Collider[] zoneColliders = Physics.OverlapSphere(transform.position, 0.5f, layerMask);

        foreach (Collider col in zoneColliders)
        {
            SupplyTower tower = col.GetComponent<SupplyTower>();
            if (tower != null)
            {
                registeredTower = tower;
                registeredTower.AddWarehouseBonus(requisitionBonus);
                return;
            }
        }

        Debug.LogWarning("Warehouse : aucune SupplyTower trouvée");
    }


    #endregion METHODE    
}