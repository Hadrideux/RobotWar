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
        if (placeableComponent.IsPlaced)
        {
            ProductionTimer();
        }
    }

    #endregion

    private void ProductionTimer()
    {
        if (isProductionEnabled == false)
        {
            return;
        }
        else
        {
            switch (buildFaction)
            {
                case EFactionType.ALLY:
                    if(!isProductionStart)
                    {

                    }
                    if (UnitAssembled.UnitData.RequisitionCost <= requisitionManager.RequisitionStock && !isProductionStart)
                    {
                        requisitionManager.RequisitionStock -= UnitAssembled.UnitData.RequisitionCost;
                        isProductionStart = true;
                    }

                    if (productionTime >= UnitAssembled.UnitData.ProductionTime && isProductionStart == true)
                    {
                        SpawnUnit(UnitAssembled);
                    }
                    else
                    {
                        productionTime += Time.deltaTime;
                    }
                    break;
                case EFactionType.IA:
                    if (UnitAssembled.UnitData.RequisitionCost <= IARequsition.RequisitionStock && !isProductionStart)
                    {
                        requisitionManager.RequisitionStock -= UnitAssembled.UnitData.RequisitionCost;
                        isProductionStart = true;
                    }

                    if (productionTime >= UnitAssembled.UnitData.ProductionTime && isProductionStart == true)
                    {
                        SpawnUnit(UnitAssembled);
                    }
                    else
                    {
                        productionTime += Time.deltaTime;
                    }
                    break;
                default:
                    break;
            }
        }


    }
    public void SpawnUnit(AUnitClass spawnUnit)
    {
        AUnitClass unit = Instantiate(spawnUnit, spawner.position, Quaternion.identity, unitContainer);
        unit.FactionObject = buildFaction;

        UnitManager.Instance.UnitAvailable(unit);

        isProductionStart = false;
        productionTime = 0;
    }
    protected override void BuildDestroyed()
    {

    }
    #endregion
}