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
            
            if (hit.collider.GetComponent<ISelectable>() != null)
            {
                OrderData orderData = new OrderData(EOrderType.ATTACK, hit.collider.GetComponent<ISelectable>());
                PushOrder(orderData);
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                bool orderSent = false;

                OrderData orderData = new OrderData(EOrderType.MOVETO, hit.point);
                PushOrder(orderData);

                if (orderSent)
                {
                    groundMarker.transform.position = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
                }
            }
        }
    }

    private void PushOrder(OrderData orderData)
    {
        foreach (ISelectable selectable in selectionManager.SelectableObj)
        {
            IOrderReceiver unit = selectable as IOrderReceiver;
            if (unit != null)
            {
                unit.ReceiveOrder(orderData);
            }
        }
    }
    #endregion


}
