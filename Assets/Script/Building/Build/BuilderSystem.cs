using UnityEngine;
using UnityEngine.Tilemaps;

public class BuilderSystem : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private PlayerInteraction playerInteraction = null;
    [SerializeField] private BuilderManager builderManager = null;

    [Header("Grid")]
    [SerializeField] private Grid buildGrid = null;
    [SerializeField] private GridLayout gridLayout = null;    

    [SerializeField] private Tilemap buildTileMap = null;
    [SerializeField] private TileBase buildTileBase = null;

    [SerializeField] private PlaceableObject objectToPlace = null;


    public GridLayout GridLayout => gridLayout;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInteraction = PlayerInteraction.Instance;
        builderManager = BuilderManager.Instance;

        builderManager.OnBuildSelected += InitializeWithObject;
        playerInteraction.OnConfirmPlacement += PlaceBuilding;
        playerInteraction.OnCancelPlacement += CancelBuilding;
    }
    public Vector3 SnapCoordinateToGrid(Vector3 pos)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(pos);
        pos = buildGrid.GetCellCenterWorld(cellPos);

        return pos;
    }
    public void InitializeWithObject(ABuildClass prefab)
    {
        Vector3 pos = SnapCoordinateToGrid(playerInteraction.GetMouseWorlPosition());

        ABuildClass obj = Instantiate(prefab, pos, Quaternion.identity);

        objectToPlace = obj.GetComponent<PlaceableObject>();
        objectToPlace.BuilderSystem = this;
        
        obj.gameObject.AddComponent<ObjectDrag>();
        obj.GetComponent<ObjectDrag>().BuilderSystem = this;
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
    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;

        TileBase[] baseArray = GetTilesBlock(area, buildTileMap);

        foreach (var b in baseArray)
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
        if (CanBePlaced(objectToPlace) && objectToPlace != null)
        {
            objectToPlace.Place();
            Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
            TakeArea(start, objectToPlace.Size);
            objectToPlace = null;
        }
        else
        {
            Debug.Log("Construction échoué");
            Destroy(objectToPlace.gameObject);
        }
    }

    public void CancelBuilding()
    {
        Destroy(objectToPlace.gameObject);
    }
}
