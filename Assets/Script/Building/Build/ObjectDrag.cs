using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction;

    void Start()
    {
        playerInteraction = PlayerInteraction.Instance;
    }

    void Update()
    {
        Vector3 pos = playerInteraction.GetMouseWorlPosition();
        transform.position = BuilderSystem.current.SnapCoordinateToGrid(pos);
    }
}
