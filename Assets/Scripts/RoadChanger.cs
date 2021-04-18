using System.Linq;
using UnityEngine;
using System;

public class RoadChanger : MonoBehaviour
{
    [SerializeField] private GameObject _deadEndRoad;
    [SerializeField] private GameObject _straightRoad;
    [SerializeField] private GameObject _cornerRoad;
    [SerializeField] private GameObject _threeWayRoad;
    [SerializeField] private GameObject _fourWayRoad;

    public void ChangeRoadAtPosition(Vector3Int tempPos)
    {
        var result = PlacementManager.Instance.GetNeighbourTypes(tempPos);
        int roadCount = result.Where(x => x == CellType.Road).Count();

        if(roadCount == 0 || roadCount == 1)
        {
            CreateDeadEnd(result, tempPos);
        }
        else if(roadCount == 2)
        {
            if (CreateStraightRoad(result, tempPos))
                return;
            CreateCorner();
        }
        else if(roadCount == 3)
        {
            CreateThreeWay(result, tempPos);
        }

        switch(roadCount)
        {
            case 0:
                CreateDeadEnd(result, tempPos);
                break;
            case 1:
                CreateDeadEnd(result, tempPos);
                break;
            case 2:
                if (CreateStraightRoad(result, tempPos))
                    return;
                CreateCorner();
                break;
            case 3:
                CreateThreeWay(result, tempPos);
                break;
            case 4:
                CreateFourWay(result, tempPos);
                break;
        }

    }

    private bool CreateStraightRoad(CellType[] result, Vector3Int tempPos)
    {
        throw new NotImplementedException();
    }

    private void CreateDeadEnd(CellType[] result, Vector3Int tempPos)
    {
        throw new NotImplementedException();
    }

    private void CreateCorner()
    {
        throw new NotImplementedException();
    }

    private void CreateThreeWay(CellType[] result, Vector3Int tempPos)
    {
        throw new NotImplementedException();
    }

    private void CreateFourWay(CellType[] result, Vector3Int tempPos)
    {
        throw new NotImplementedException();
    }    
}
