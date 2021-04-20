using System;
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

    [SerializeField] private List<Vector3Int> _tempPlacementPositions = new List<Vector3Int>();
    [SerializeField] private List<Vector3Int> _roadPositionsToRecheck = new List<Vector3Int>();

    private Vector3Int startPos;
    private bool _isInPlacementMode;


    public void PlaceRoad(Vector3Int position)
    {
        if (!PlacementManager.Instance.CheckIfPositionInBound(position) || !PlacementManager.Instance.CheckIfPositionIsFree(position))
            return;

        if(!_isInPlacementMode)
        {
            _tempPlacementPositions.Clear();
            _roadPositionsToRecheck.Clear();

            _isInPlacementMode = true;
            startPos = position;

            _tempPlacementPositions.Add(position);
            PlacementManager.Instance.PlaceTempStructure(position, RoadChanger.Instance.DeadEndRoad, CellType.Road); //scriptable obj
        }
        else
        {
            PlacementManager.Instance.RemoveAllTempStructures();
            _tempPlacementPositions.Clear();


            foreach(var posToFix in _roadPositionsToRecheck)
            {
                RoadChanger.Instance.ChangeRoadAtPosition(posToFix);
            }



            _roadPositionsToRecheck.Clear();

            _tempPlacementPositions = PlacementManager.Instance.GetPathBetween(startPos, position);
            foreach(var pos in _tempPlacementPositions)
            {
                if (!PlacementManager.Instance.CheckIfPositionIsFree(pos))
                    continue;
                PlacementManager.Instance.PlaceTempStructure(pos, RoadChanger.Instance.DeadEndRoad, CellType.Road);
            }
        }

        ChangeRoadPrefabs();

    }

    private void ChangeRoadPrefabs()
    {
        foreach (var tempPos in _tempPlacementPositions)
        {
            RoadChanger.Instance.ChangeRoadAtPosition(tempPos);
            var neighbours = PlacementManager.Instance.GetNeighboursOfType(tempPos, CellType.Road);
            foreach(var roadPos in neighbours)
            {
                if (!_roadPositionsToRecheck.Contains(roadPos))
                {
                    _roadPositionsToRecheck.Add(roadPos);
                }
            }
        }
        foreach(var posToFix in _roadPositionsToRecheck)
        {
            RoadChanger.Instance.ChangeRoadAtPosition(posToFix);
        }

    }


    public void FinishPlacingRoad()
    {
        _isInPlacementMode = false;
        PlacementManager.Instance.AddTempStructureToDictionary();
        if(_tempPlacementPositions.Count > 0)
        {
            AudioPlayer.Instance.PlayPlacementSound();
        }
        _tempPlacementPositions.Clear();
        startPos = Vector3Int.zero;
    }
}
