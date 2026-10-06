using UnityEngine;

public class IADefenseController : MonoBehaviour
{
    #region ATTRIBUTS
    [SerializeField] private OrderData order = null;
    #endregion  ATTRIBUTS

    #region METHODES
    #region MONO
    // Use this for initialization
    void Start()
    {

        IAManager.Instance.OnDefend += LaunchDefense;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDestroy()
    {
        IAManager.Instance.OnDefend -= LaunchDefense;
    }
    private void OnApplicationQuit()
    {
        IAManager.Instance.OnDefend -= LaunchDefense;
    }
    #endregion MONO

    public void LaunchDefense(ABuildClass buildAttacked)
    {
        order = new OrderData(EOrderType.MOVETO, buildAttacked.transform.position);

        foreach (AUnitClass unit in IAManager.Instance.DefenseUnits)
        {
            TransmitOrder(unit);
        }
    }

    public void TransmitOrder(AUnitClass unit)
    {
        unit.ReceiveOrder(order);
    }
    #endregion METHODES
}

