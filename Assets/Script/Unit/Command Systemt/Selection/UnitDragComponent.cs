using UnityEngine;

public class UnitDragComponent : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction = null;
    [SerializeField] private SelectableManager selectionManager = null;

    [SerializeField] private RectTransform boxVisual = null;

    [SerializeField] private Rect selectionBox = new Rect();
    [SerializeField] private Vector2 startPosition = Vector2.zero;
    [SerializeField] private Vector2 endPosition = Vector2.zero;

    #region METHODE
    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInteraction = PlayerInteraction.Instance;
        selectionManager = SelectableManager.Instance;

        playerInteraction.OnDragStarted += StartSelection;
        playerInteraction.OnDragUpdate += DragSelection;
        playerInteraction.OnDragReleased += ReleaseSelection;
        DrawVisual();
    }
    void OnDestroy()
    {
        playerInteraction.OnDragStarted -= StartSelection;
        playerInteraction.OnDragUpdate -= DragSelection;
        playerInteraction.OnDragReleased -= ReleaseSelection;
    }
    void OnApplicationQuit()
    {
        playerInteraction.OnDragStarted -= StartSelection;
        playerInteraction.OnDragUpdate -= DragSelection;
        playerInteraction.OnDragReleased -= ReleaseSelection;
    }

    #endregion
    public void StartSelection(Vector2 mousePosition)
    {
        startPosition = mousePosition;
    }

    public void DragSelection(Vector2 mousePosition)
    {
        endPosition = mousePosition;
        DrawVisual();
        DrawSelection(mousePosition);
    }
    public void ReleaseSelection()
    {
        SelectUnits();
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    public void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
        boxVisual.sizeDelta = boxSize;

    }
    public void DrawSelection(Vector2 mousePosition)
    {
        if (mousePosition.x < startPosition.x)
        {
            selectionBox.xMin = mousePosition.x;
            selectionBox.xMax = startPosition.x;

        }
        else
        {
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = mousePosition.x;
        }

        if (mousePosition.y < startPosition.y)
        {
            selectionBox.yMin = mousePosition.y;
            selectionBox.yMax = startPosition.y;

        }
        else
        {
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = mousePosition.y;
        }
    }
    public void SelectUnits()
    {
        foreach (AUnitClass unit in UnitManager.Instance.ActiveUnits)
        {
            if (selectionBox.Contains(Camera.main.WorldToScreenPoint(unit.transform.position)))
            {
                selectionManager.DargSelect(unit);
            }

        }

    }
    #endregion
}
