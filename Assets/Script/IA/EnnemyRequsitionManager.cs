using UnityEngine;

public class EnnemyRequsitionManager : Singleton<EnnemyRequsitionManager>
{
    [SerializeField] private int requisitionStock = 0;

    public int RequisitionStock
    {
        get => requisitionStock;
        set
        {
            requisitionStock = value;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
