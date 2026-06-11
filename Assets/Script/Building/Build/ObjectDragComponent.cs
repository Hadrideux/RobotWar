using UnityEngine;

public class ObjectDragComponent : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction = null;

    [SerializeField] private BuilderSystemController builderSystemController = null;

    public BuilderSystemController BuilderSystemController
    {
        get => builderSystemController;
        set => builderSystemController = value;
    }

    void Start()
    {
        playerInteraction = PlayerInteraction.Instance;
    }

    void Update()
    {
        Vector3 pos = playerInteraction.GetMouseWorlPosition();
        transform.position = BuilderSystemController.SnapCoordinateToGrid(pos);
    }
}
