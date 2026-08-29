using UnityEngine;

public class CommandSystemController : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction = null;
    [SerializeField] private SelectableManager selectionManager = null;

    [SerializeField] private GameObject groundMarker = null;

    #region METHODE
    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInteraction = PlayerInteraction.Instance;
        selectionManager = SelectableManager.Instance;

        playerInteraction.OnExecuteOrder += ExecuteOrder;
    }

    private void OnDestroy()
    {
        playerInteraction.OnExecuteOrder -= ExecuteOrder;
    }
    private void OnApplicationQuit()
    {
        playerInteraction.OnExecuteOrder -= ExecuteOrder;
    }
    #endregion


    private void ExecuteOrder(RaycastHit hit)
    {
        if (selectionManager.SelectableObj.Count > 0)
        {
            bool orderSent = false;

            if (hit.collider == null)
            {
                OrderData orderData = new OrderData(EOrderType.STOP);
            }
            else if (hit.collider.TryGetComponent(out ITargetableObject target))
            {
                OrderData orderData = new OrderData(EOrderType.ATTACK, target);
                orderSent = PushOrder(orderData);

                if (orderSent)
                {
                    groundMarker.transform.position = target.TargetObject.transform.position;
                }
            }
            else
            {
                OrderData orderData = new OrderData(EOrderType.MOVETO, hit.point);
                orderSent = PushOrder(orderData);

                if (orderSent)
                {
                    groundMarker.transform.position = hit.point;
                }
            }
        }
    }

    private bool PushOrder(OrderData orderData)
    {
        foreach (ISelectable selectable in selectionManager.SelectableObj)
        {
            IOrderReceiver unit = selectable as IOrderReceiver;
            if (unit != null)
            {
                unit.ReceiveOrder(orderData);
            }
        }

        return true;
    }
    #endregion


}
