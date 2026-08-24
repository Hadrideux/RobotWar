using UnityEngine;

public class PlaceableObjectComponent : MonoBehaviour
{
    #region ATTRIBUT
    [SerializeField] private bool isPlaced = false;

    [SerializeField] private Vector3Int size = Vector3Int.zero;
    [SerializeField] private Vector3[] objectVertices = null;

    [SerializeField] private BoxCollider boxCollider = null;

    [SerializeField] private BuilderSystemController builderSystemController = null;
    #endregion

    #region PROPERTIES
    public bool IsPlaced => isPlaced;

    public Vector3Int Size
    {
        get => size;
        set => size = value;
    }

    public BuilderSystemController BuilderSystemController
    {
        get => builderSystemController;
        set => builderSystemController = value;
    }

    #endregion

    void Start()
    {
        if (isPlaced) return;

        GetColliderVectexPositionLocal();
        CalculateSizeInCell();
    }
    public void GetColliderVectexPositionLocal()
    {
        objectVertices = new Vector3[4];
        objectVertices[0] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z);
        objectVertices[1] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z);
        objectVertices[2] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z);
        objectVertices[3] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z);
    }

    public void CalculateSizeInCell()
    {
        Vector3Int[] vertices = new Vector3Int[objectVertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(objectVertices[i]);
            vertices[i] = BuilderSystemController.GridLayout.WorldToCell(worldPos);
        }

        Size = new Vector3Int(Mathf.Abs((vertices[0] - vertices[1]).x), Mathf.Abs((vertices[0] - vertices[3]).y), 1);
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(objectVertices[0]);
    }

    public void Place()
    {
        ObjectDragComponent drag = GetComponent<ObjectDragComponent>();
        Destroy(drag);

        isPlaced = true;
    }

}
