using UnityEngine;

public class AModuleIA : ABuildClass
{
    #region ATTRIBUTS

    [SerializeField] private NetworkManager networkManager = null;

    [SerializeField] private int loadAugment = 0;

    #endregion ATTRIBUTS

    #region Methode 

    #region MONO

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        networkManager = NetworkManager.Instance;

        networkManager.MaxLoad += loadAugment;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildingDestroyed()
    {
        networkManager.MaxLoad -= loadAugment;
        Destroy(gameObject);
    }

    #endregion MONO
    #region ABSTRACT
    protected override void ChangeFaction()
    {
        
    }

    protected override void UpdateBuildingCapture()
    {
        
    }

    protected override void UpdateRateProduction()
    {
        
    }

    #endregion ABSTRACT
    #endregion METHODE


}
