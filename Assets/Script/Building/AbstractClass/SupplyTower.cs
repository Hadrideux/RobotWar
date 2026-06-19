using UnityEngine;

public class SupplyTower : ABuildClass
{
    [SerializeField] private float productionTime = 0;
    [SerializeField] private float currentProductionTime = 0;
    [SerializeField] private int requisitionRate = 0;

    #region METHODE
    #region MONO

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ProductionTimer();
    }

    #endregion MONO
    #region ABSTRACT
    protected override void BuildDestroyed()
    {
        Destroy(gameObject);
    }

    #endregion ABSTRACT

    public override void InitBuild()
    {
    }

    private void ProductionTimer()
    {
        if (currentProductionTime >= productionTime)
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
        switch (buildFaction)
        {
            case EFactionType.ALLY:
                RequisitionManager.Instance.RequisitionStock += requisitionRate;
                break;
            case EFactionType.IA:
                EnnemyRequsitionManager.Instance.RequisitionStock += requisitionRate;
                break;
            default:
                break;
        }
    }

    public void AddWarehouseBonus(int bonus)
    {
        requisitionRate += bonus;
    }

    public void RemoveWarehouseBonus(int bonus)
    {
        requisitionRate -= bonus;
    }


    #endregion METHODE
}
