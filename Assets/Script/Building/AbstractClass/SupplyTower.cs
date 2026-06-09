using UnityEngine;

public class SupplyTower : ABuildClass
{
    [Header("Manager")]
    [SerializeField] private RequisitionManager requisitionManger = null;

    [SerializeField] private float productionTime = 0;
    [SerializeField] private float currentProductionTime = 0;
    [SerializeField] private int requisitionRate = 0;
    [SerializeField] private int currentRequisitionRate = 0;

    #region METHODE
    #region MONO
                                                                        
    // Start is called before the first frame update
    void Start()
    {
        InitBuild();
    }

    // Update is called once per frame
    void Update()
    {
        if(placeableComponent.IsPlaced)
        {
            ProductionTimer();
        }
    }

    #endregion MONO
    #region ABSTRACT
    protected override void BuildDestroyed()
    {
        Destroy(gameObject);
    }

    #endregion ABSTRACT

    public  override void InitBuild()
    {
        requisitionManger = RequisitionManager.Instance;

        currentRequisitionRate = requisitionRate;
    }

    private void ProductionTimer()
    {
        if(currentProductionTime >= productionTime)
        {
            Debug.Log($"Livraison de: {requisitionRate} réquisition");
            ProduceRequisition();
            currentProductionTime = 0;
        }
        else
        {
            currentProductionTime += Time.deltaTime;
        }
    }

    private void ProduceRequisition()
    {
        requisitionManger.RequisitionStock += currentRequisitionRate;
    }

    public void AddWarehouseBonus(int bonus)
    {
        currentRequisitionRate += bonus;
    }

    public void RemoveWarehouseBonus(int bonus)
    {
        currentRequisitionRate -= bonus;
    }
    

    #endregion METHODE
}
