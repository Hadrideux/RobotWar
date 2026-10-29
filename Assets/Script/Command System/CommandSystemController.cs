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
        if (selectionManager.SelectableObj.Count <= 0)
            return;

        OrderData orderData = null;
        Vector3 markerPos = Vector3.zero;

        if (hit.collider == null)
        {
            orderData = new OrderData(EOrderType.STOP);
        }
        else if (hit.collider.TryGetComponent(out ITargetableObject target))
        {
            if (!target.IsTargetable || target.FactionObject == EFactionType.ALLY)
                return;

            orderData = new OrderData(EOrderType.ATTACK, target);
            markerPos = target.TargetObject.transform.position;
        }
        else
        {
            orderData = new OrderData(EOrderType.MOVETO, hit.point);
            markerPos = hit.point;

        }

        if (PushOrder(orderData) && orderData.OrderType != EOrderType.STOP)
        {
            groundMarker.transform.position = markerPos;
        }
    }

    private bool PushOrder(OrderData orderData)
    {
        bool received = false;

        foreach (ISelectable selectable in selectionManager.SelectableObj)
        {
            IOrderReceiver unit = selectable as IOrderReceiver;
            if (unit != null)
            {
                unit.ReceiveOrder(orderData);
                received = true;
            }
        }
        return received;
    }
    #endregion


}
