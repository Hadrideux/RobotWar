using UnityEngine;

public class SupplyTower : ABuildClass
{
    [Header("Ressource")]
    [SerializeField] private float productionTime = 0;
    [SerializeField] private float currentProductionTime = 0;
    [SerializeField] private int requisitionRate = 0;

    [Header("Capture")]
    [SerializeField] private float captureDistance = 0;
    [SerializeField] private float captureDuration = 0;
    [SerializeField] private float currentCaptureProgression = 0;
    [SerializeField] private float captureMultiplier = 0;
    [SerializeField] private LayerMask layerMask = 0;


    #region METHODE
    #region MONO
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (Application.isPlaying)
            // Draw a sphere where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireSphere(transform.position, captureDistance);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ProductionTimer();
        ScanForCapture();
        CaptureTimer();
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

    public void ScanForCapture()
    {
        Collider[] targetCol = Physics.OverlapSphere(transform.position, captureDistance, layerMask);

        float friendlyUnit = 0;
        float hotileUnit = 0;

        for (int i = 0; i < targetCol.Length; i++)
        {
            AUnitClass unitScanned = targetCol[i].gameObject.GetComponent<AUnitClass>();

            if (unitScanned.FactionObject != ObjectFaction)
            {
                hotileUnit++;
            }
            else if (unitScanned.FactionObject == ObjectFaction)
            {
                friendlyUnit++;
            }
        }

        captureMultiplier = Mathf.Clamp(hotileUnit - friendlyUnit, 0, Mathf.Infinity);
    }

    private void CaptureTimer()
    {
        if (currentCaptureProgression > captureDuration)
        {
            switch (ObjectFaction)
            {
                case EFactionType.ALLY:
                    buildFaction = EFactionType.IA;
                    break;
                case EFactionType.IA:
                    buildFaction = EFactionType.ALLY;
                    break;
                default:
                    buildFaction = EFactionType.NONE;
                    break;
            }

            currentCaptureProgression = 0;
        }
        else
        {
            currentCaptureProgression += captureMultiplier * Time.deltaTime;
        }
    }


    #endregion METHODE
}
