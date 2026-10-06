using UnityEngine;

public class IAAttackController : MonoBehaviour
{

    #region ATTRIBUTS
    [SerializeField] private OrderData order = null;
    #endregion  ATTRIBUTS

    #region METHODES
    #region MONO
    void Start()
    {

        IAManager.Instance.OnAttack += LaunchAttack;
        IAManager.Instance.OnUnitJoinAttack += TransmitOrder;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDestroy()
    {
        IAManager.Instance.OnAttack -= LaunchAttack;
        IAManager.Instance.OnUnitJoinAttack -= TransmitOrder;
    }
    private void OnApplicationQuit()
    {
        IAManager.Instance.OnAttack -= LaunchAttack;
        IAManager.Instance.OnUnitJoinAttack -= TransmitOrder;
    }
    #endregion MONO
    public void LaunchAttack()
    {
        order = new OrderData(EOrderType.ATTACK, IAManager.Instance.PlayerHQ);
        foreach (AUnitClass unit in IAManager.Instance.AttackUnits)
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
