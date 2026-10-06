using UnityEngine;

public class ModuleIA : ABuildClass
{
    #region ATTRIBUTS
    [SerializeField] private int augmentLoad = 0;
    #endregion ATTRIBUTS

    #region Methode 

    #region MONO

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (buildFaction == EFactionType.ALLY)
        {
            NetworkManager.Instance.CurrentLoad += augmentLoad;
        }
        else if (buildFaction == EFactionType.IA)
        {
            IANetworkManager.Instance.CurrentLoad += augmentLoad;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion MONO
    #region ABSTRACT
    protected override void BuildDestroyed()
    {
        if (buildFaction == EFactionType.ALLY)
        {
            NetworkManager.Instance.CurrentLoad -= augmentLoad;
        }
        else if (buildFaction == EFactionType.IA)
        {
            IANetworkManager.Instance.CurrentLoad -= augmentLoad;
        }

        Destroy(gameObject);
    }
    #endregion ABSTRACT


    #endregion METHODE


}
