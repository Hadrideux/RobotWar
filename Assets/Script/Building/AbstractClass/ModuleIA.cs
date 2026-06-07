using UnityEngine;

public class ModuleIA : ABuildClass
{
    #region ATTRIBUTS

    [SerializeField] private NetworkManager networkManager = null;

    [SerializeField] private int augmentLoad = 0;

    #endregion ATTRIBUTS

    #region Methode 

    #region MONO

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        networkManager = NetworkManager.Instance;
        networkManager.IncreaseNetworkLoad(augmentLoad);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion MONO
    #region ABSTRACT
    protected override void BuildDestroyed()
    {
        NetworkManager.Instance.DecreseNetworkLoad(augmentLoad);

        Destroy(gameObject);
    }
    #endregion ABSTRACT


    #endregion METHODE


}
