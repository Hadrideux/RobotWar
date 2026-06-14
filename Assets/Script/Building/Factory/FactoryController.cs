using UnityEngine;

public class FactoryController : ABuildClass
{
    [Header("Manager")]
    [SerializeField] private RequisitionManager requisitionManager = null;
    [Header("Component")]
    [SerializeField] private SpawnerComponent spawnerComponent = null;

    [SerializeField] private EUnitType _unitProduction = EUnitType.NONE;

    [SerializeField] private AUnitClass unitToSpawn = null;

    [SerializeField] private float productionTime = 0f;

    [SerializeField] private bool isProductionEnabled = false;
    [SerializeField] private bool isProductionStart = false;


    #region METHODE
    #region MONO

    // Start is called before the first frame update
    void Start()
    {
        requisitionManager = RequisitionManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (placeableComponent.IsPlaced)
        {
            ProductionTimer();
        }
    }

    #endregion MONO
    #region ABSTARCT

    protected override void BuildDestroyed()
    {
    }

    #endregion ABSTRACT 
    private void ProductFinish()
    {
        spawnerComponent.SpawnUnit(unitToSpawn);
    }


    private void ProductionTimer()
    {
        if (isProductionEnabled == false)
        {
            return;
        }
        else if (unitToSpawn.UnitData.RequisitionCost <= requisitionManager.RequisitionStock && isProductionStart == false)
        {
            requisitionManager.RequisitionStock -= unitToSpawn.UnitData.RequisitionCost;
            isProductionStart = true;
        }

        if (productionTime >= unitToSpawn.UnitData.ProductionTime && isProductionStart == true)
        {
            ProductFinish();
            isProductionStart = false;
            productionTime = 0;
        }
        else
        {
            productionTime += Time.deltaTime;
        }
    }



    #endregion METHODE

}