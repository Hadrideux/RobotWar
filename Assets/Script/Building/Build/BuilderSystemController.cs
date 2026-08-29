using UnityEngine;
using UnityEngine.Tilemaps;

public class BuilderSystemController : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private PlayerInteraction playerInteraction = null;
    [SerializeField] private BuilderSystemManager builderManager = null;

    [Header("Grid")]
    [SerializeField] private Grid buildGrid = null;
    [SerializeField] private GridLayout gridLayout = null;

    [SerializeField] private Tilemap buildTileMap = null;
    [SerializeField] private TileBase buildTileBase = null;

    [SerializeField] private PlaceableObjectComponent objectToPlace = null;


    public GridLayout GridLayout => gridLayout;

    #region METHODES
    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInteraction = PlayerInteraction.Instance;
        builderManager = BuilderSystemManager.Instance;

        builderManager.OnBuildSelected += InitializeWithObject;
        playerInteraction.OnConfirmPlacement += PlaceBuilding;
        playerInteraction.OnCancelAction += CancelBuilding;
    }

    private void OnDestroy()
    {
        builderManager.OnBuildSelected -= InitializeWithObject;
        playerInteraction.OnConfirmPlacement -= PlaceBuilding;
        playerInteraction.OnCancelAction -= CancelBuilding;
    }
    private void OnApplicationQuit()
    {
        builderManager.OnBuildSelected -= InitializeWithObject;
        playerInteraction.OnConfirmPlacement -= PlaceBuilding;
        playerInteraction.OnCancelAction -= CancelBuilding;
    }
    #endregion
    public Vector3 SnapCoordinateToGrid(Vector3 pos)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(pos);
        pos = buildGrid.GetCellCenterWorld(cellPos);

        return pos;
    }
    public void InitializeWithObject(ABuildClass prefab)
    {
        Vector3 pos = SnapCoordinateToGrid(playerInteraction.GetMouseWorlPosition(playerInteraction.GroundMask).point);

        ABuildClass obj = Instantiate(prefab, pos, Quaternion.identity);

        objectToPlace = obj.GetComponent<PlaceableObjectComponent>();
        objectToPlace.BuilderSystemController = this;
        objectToPlace.DragComponent.BuilderSystemController = this;

        //obj.gameObject.AddComponent<ObjectDragComponent>();
        //obj.GetComponent<ObjectDragComponent>().BuilderSystemController = this;
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (Vector3Int v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }
    private bool CanBePlaced(PlaceableObjectComponent placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;

        TileBase[] baseArray = GetTilesBlock(area, buildTileMap);

        foreach (TileBase b in baseArray)
        {
            if (b == buildTileBase)
            {
                return false;
            }
        }

        return true;
    }

    private void TakeArea(Vector3Int start, Vector3Int size)
    {
        buildTileMap.BoxFill(start, buildTileBase, start.x, start.y, start.x, start.y);
    }

    public void PlaceBuilding()
    {
        if(objectToPlace == null)
        {
            return;
        }

        if (CanBePlaced(objectToPlace))
        {
            objectToPlace.Place();

            Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
            TakeArea(start, objectToPlace.Size);

            objectToPlace = null;
        }
        else
        {
            Destroy(objectToPlace.gameObject);
        }
    }

    public void CancelBuilding()
    {
        Destroy(objectToPlace.gameObject);
    }
    #endregion


}
