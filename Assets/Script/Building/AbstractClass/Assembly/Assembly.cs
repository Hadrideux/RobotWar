using UnityEngine;
public class Assembly : ABuildClass
{
#region ATTRIBUTS

    [SerializeField] protected AUnitClass unitAssembled = null;

    [SerializeField] protected float productionTime = 0f;

    [SerializeField] protected bool isProductionEnabled = false;
    [SerializeField] protected bool isProductionStart = false;

    [SerializeField] protected Transform spawner = null;
    [SerializeField] protected Transform unitContainer = null;
#endregion

#region PROPERTY
    public AUnitClass UnitAssembled
    {
        get => unitAssembled;
        set => unitAssembled = value;
    }
#endregion

#region METHODE
#region MONO

    void Update()
    {
        if (placeableComponent.IsPlaced && isProductionEnabled)
        {
            if(isProductionStart == false)
            {
                TryStartProduction();
            }

            if (isProductionStart)
            {
                if (productionTime >= UnitAssembled.UnitData.ProductionTime)
                {
                    productionTime = 0;
                    isProductionStart = false;

                    SpawnUnitProduction(unitAssembled);
                }
                else
                {
                    productionTime += Time.deltaTime;
                }
            }

            
        }
    }
    #endregion MONO

    #region ABSTRACT
    protected override void BuildDestroyed()
    {
    }
    #endregion ABSTRACT
    private void TryStartProduction()
    {
        switch (buildFaction)
        {
            case EFactionType.ALLY:
                if (UnitAssembled.UnitData.RequisitionCost <= requisitionManager.RequisitionStock)
                {
                    isProductionStart = true;
                    requisitionManager.RequisitionStock -= UnitAssembled.UnitData.RequisitionCost;
                    return;
                }
                break;
            case EFactionType.IA:
                if (UnitAssembled.UnitData.RequisitionCost <= IARequsition.RequisitionStock)
                {
                    IARequsition.RequisitionStock -= UnitAssembled.UnitData.RequisitionCost;
                    isProductionStart = true;
                    return;
                }
                break;
            default:
                break;
        }
    }
    public void SpawnUnitProduction(AUnitClass spawnUnit)
    {
        AUnitClass unit = Instantiate(spawnUnit, spawner.position, Quaternion.identity, unitContainer);
        unit.FactionObject = buildFaction;

        UnitManager.Instance.UnitAvailable(unit);
        
    }
#endregion
}