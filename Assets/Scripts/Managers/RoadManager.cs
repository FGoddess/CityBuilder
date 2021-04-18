using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    private static RoadManager _instance;
    public static RoadManager Instance
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


    [SerializeField] private List<Vector3> _tempPlacementPositions = new List<Vector3>();
    [SerializeField] private GameObject _roadStraight;

    public void PlaceRoad(Vector3Int position)
    {
        if (!PlacementManager.Instance.CheckIfPositionInBound(position))
            return;

        if (!PlacementManager.Instance.CheckIfPositionIsFree(position))
            return;

        PlacementManager.Instance.PlaceTempStructure(position, _roadStraight, CellType.Road);
    }

}
