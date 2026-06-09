using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction = null;

    [SerializeField] private BuilderSystem builderSystem = null;

    public BuilderSystem BuilderSystem
    {
        get => builderSystem;
        set => builderSystem = value;
    }

    void Start()
    {
        playerInteraction = PlayerInteraction.Instance;
    }

    void Update()
    {
        Vector3 pos = playerInteraction.GetMouseWorlPosition();
        transform.position = builderSystem.SnapCoordinateToGrid(pos);
    }
}
