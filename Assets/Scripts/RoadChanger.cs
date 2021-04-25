using System.Linq;
using UnityEngine;

public class RoadChanger : MonoBehaviour
{
    [SerializeField] private GameObject _deadEndRoad;
    [SerializeField] private GameObject _straightRoad;
    [SerializeField] private GameObject _cornerRoad;
    [SerializeField] private GameObject _threeWayRoad;
    [SerializeField] private GameObject _fourWayRoad;

    public GameObject DeadEndRoad => _deadEndRoad;


    private static RoadChanger _instance;
    public static RoadChanger Instance
    {
        get
        {
            if (_instance == null)
                Instantiate(_instance);
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void ChangeRoadAtPosition(Vector3Int tempPos)
    {
        var neighboursTypes = PlacementManager.Instance.GetNeighbourTypes(tempPos);
        int roadCount = 0;
        roadCount = neighboursTypes.Where(x => x == CellType.Road).Count();

        switch (roadCount)
        {
            case 0:
                CreateDeadEnd(neighboursTypes, tempPos);
                break;
            case 1:
                CreateDeadEnd(neighboursTypes, tempPos);
                break;
            case 2:
                if (CreateStraightRoad(neighboursTypes, tempPos))
                    return;
                CreateCorner(neighboursTypes, tempPos);
                break;
            case 3:
                CreateThreeWay(neighboursTypes, tempPos);
                break;
            case 4:
                CreateFourWay(tempPos);
                break;
        }
    }

    private bool CreateStraightRoad(CellType[] neighboursTypes, Vector3Int tempPos)
    {
        if (neighboursTypes[0] == CellType.Road && neighboursTypes[2] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _straightRoad, Quaternion.identity);
            return true;
        }
        else if (neighboursTypes[1] == CellType.Road && neighboursTypes[3] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _straightRoad, Quaternion.Euler(0, 90, 0));
            return true;
        }
        return false;
    }

    private void CreateDeadEnd(CellType[] neighboursTypes, Vector3Int tempPos)
    {
        if (neighboursTypes[1] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _deadEndRoad, Quaternion.Euler(0, 270, 0));
        }
        else if (neighboursTypes[2] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _deadEndRoad, Quaternion.identity);
        }
        else if (neighboursTypes[3] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _deadEndRoad, Quaternion.Euler(0, 90, 0));
        }
        else if (neighboursTypes[0] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _deadEndRoad, Quaternion.Euler(0, 180, 0));
        }
    }

    private void CreateCorner(CellType[] neighboursTypes, Vector3Int tempPos)
    {
        if (neighboursTypes[1] == CellType.Road && neighboursTypes[2] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _cornerRoad, Quaternion.Euler(0, 90, 0));
        }
        else if (neighboursTypes[2] == CellType.Road && neighboursTypes[3] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _cornerRoad, Quaternion.Euler(0, 180, 0));
        }
        else if (neighboursTypes[3] == CellType.Road && neighboursTypes[0] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _cornerRoad, Quaternion.Euler(0, 270, 0));
        }
        else if (neighboursTypes[0] == CellType.Road && neighboursTypes[1] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _cornerRoad, Quaternion.identity);
        }
    }

    private void CreateThreeWay(CellType[] neighboursTypes, Vector3Int tempPos)
    {
        if (neighboursTypes[1] == CellType.Road && neighboursTypes[2] == CellType.Road && neighboursTypes[3] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _threeWayRoad, Quaternion.identity);
        }
        else if (neighboursTypes[2] == CellType.Road && neighboursTypes[3] == CellType.Road && neighboursTypes[0] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _threeWayRoad, Quaternion.Euler(0, 90, 0));
        }
        else if (neighboursTypes[3] == CellType.Road && neighboursTypes[0] == CellType.Road && neighboursTypes[1] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _threeWayRoad, Quaternion.Euler(0, 180, 0));
        }
        else if (neighboursTypes[0] == CellType.Road && neighboursTypes[1] == CellType.Road && neighboursTypes[2] == CellType.Road)
        {
            PlacementManager.Instance.ModifyStructureModel(tempPos, _threeWayRoad, Quaternion.Euler(0, 270, 0));
        }
    }

    private void CreateFourWay(Vector3Int tempPos)
    {
        PlacementManager.Instance.ModifyStructureModel(tempPos, _fourWayRoad, Quaternion.identity);
    }
}
