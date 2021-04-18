using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    private static PlacementManager _instance;
    public static PlacementManager Instance
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


    private int _width = 15;
    private int _height = 15;
    private MapGrid _placementGrid;

    private Dictionary<Vector3Int, StructureModel> _tempRoadObjects = new Dictionary<Vector3Int, StructureModel>();

    private void Start()
    {
        _placementGrid = new MapGrid(_width, _height);
    }

    public bool CheckIfPositionInBound(Vector3Int position)
    {
        return position.x >= 0 && position.x < _width && position.z >= 0 && position.z < _height;
    }

    public CellType[] GetNeighbourTypes(Vector3Int tempPos)
    {
        return _placementGrid.GetAllAdjacentCellTypes(tempPos.x, tempPos.y);
    }

    public bool CheckIfPositionIsFree(Vector3Int position)
    {
        return CheckIfPositionOfType(position, CellType.Empty);
    }

    public bool CheckIfPositionOfType(Vector3Int position, CellType cellType)
    {
        return _placementGrid[position.x, position.z] == cellType;
    }

    public void PlaceTempStructure(Vector3Int position, GameObject structurePrefab, CellType cellType)
    {
        _placementGrid[position.x, position.z] = cellType;
        var structure = CreateNewStructureModel(position, structurePrefab, cellType);
        _tempRoadObjects.Add(position, structure);
    }

    private StructureModel CreateNewStructureModel(Vector3Int position, GameObject structPrefab, CellType type)
    {
        GameObject tempStruce = new GameObject(type.ToString());
        tempStruce.transform.SetParent(transform);
        tempStruce.transform.localPosition = position;
        var structureModel = structPrefab.AddComponent<StructureModel>();
        structureModel.CreateModel(structPrefab);

        return structureModel;
    }

    public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
    {
        if(_tempRoadObjects.ContainsKey(position))
        {
            _tempRoadObjects[position].SwapModel(newModel, rotation);
        }
    }

}
