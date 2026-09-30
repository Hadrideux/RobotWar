using UnityEngine;

public class IAAttackController : MonoBehaviour
{

    #region ATTRIBUTS
    [SerializeField] private OrderData order = null;
    #endregion  ATTRIBUTS

    #region METHODES
    #region MONO
    
    #endregion MONO
    public void LaunchAttack()
    {
        order = new OrderData(EOrderType.ATTACK, IAManager.Instance.PlayerHQ);
        foreach (AUnitClass unit in IAManager.Instance.AttackUnit)
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
