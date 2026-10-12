using UnityEngine;
using UnityEngine.Serialization;

public class ModuleIA : ABuildClass
{
    #region ATTRIBUTS
    [FormerlySerializedAs("augmentLoad")]
    [SerializeField] private int bonusMaxLoad = 0;
    #endregion ATTRIBUTS

    #region METHODES
    #region MONO

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();

        NetworkManager.Instance.GetNetworkData(FactionObject).MaxLoad += bonusMaxLoad;

    }

    #endregion MONO
    #region ABSTRACT
    protected override void BuildDestroyed()
    {
        NetworkManager.Instance.GetNetworkData(FactionObject).MaxLoad -= bonusMaxLoad;

        Destroy(gameObject);
    }
    #endregion ABSTRACT
    #endregion METHODES
}
