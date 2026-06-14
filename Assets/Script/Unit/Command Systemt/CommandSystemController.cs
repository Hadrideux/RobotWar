using UnityEngine;

public class CommandSystemController : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction = null;
    [SerializeField] private SelectableController selectionController = null;

    [SerializeField] private GameObject groundMarker = null;

    #region METHODE
    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInteraction = PlayerInteraction.Instance;

        playerInteraction.OnMoveOrder += ExecuteMoveOrder;

    }

    private void OnDestroy()
    {
        playerInteraction.OnMoveOrder -= ExecuteMoveOrder;
    }
    private void OnApplicationQuit()
    {
        playerInteraction.OnMoveOrder -= ExecuteMoveOrder;
    }
    #endregion


    private void ExecuteMoveOrder(Vector3 destination)
    {
        if (selectionController.SelectableObj.Count > 0)
        {
            bool orderSent = false;

            foreach (ISelectable selectable in selectionController.SelectableObj)
            {
                AUnitClass unit = selectable as AUnitClass;
                if (unit != null)
                {
                    unit.MovementUnit(destination);
                    orderSent = true;
                }
            }

            if (orderSent)
            {
                groundMarker.transform.position = new Vector3(destination.x, destination.y + 0.1f, destination.z);
            }
        }
    }
    #endregion


}
