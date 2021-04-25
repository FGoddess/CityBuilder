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


    private readonly int _width = 15;
    private readonly int _height = 15;
    private MapGrid _placementGrid;

    private Dictionary<Vector3Int, StructureModel> _tempRoadObjects = new Dictionary<Vector3Int, StructureModel>();
    private Dictionary<Vector3Int, StructureModel> _structureDictionary = new Dictionary<Vector3Int, StructureModel>();

    internal void PlaceObjOnTheMap(Vector3Int position, GameObject structurePrefab, CellType type, int width = 1, int height = 1)
    {
        var structure = CreateNewStructureModel(position, structurePrefab, type);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var newPos = position + new Vector3Int(x, 0, z);
                _placementGrid[newPos.x, newPos.z] = type;
                _structureDictionary.Add(newPos, structure);
                DestroyNatureAt(newPos);
            }
        }
    }

    private void DestroyNatureAt(Vector3Int position)
    {
        RaycastHit[] hits = Physics.BoxCastAll(position + new Vector3(0, 0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f), transform.up, Quaternion.identity, 1f, 1 << LayerMask.NameToLayer("Nature"));
        foreach(var item in hits)
        {
            Destroy(item.collider.gameObject);
        }
    }

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
        return _placementGrid.GetAllAdjacentCellTypes(tempPos.x, tempPos.z);
    }

    public bool CheckIfPositionIsFree(Vector3Int position)
    {
        return CheckIfPositionOfType(position, CellType.Empty);
    }

    public bool CheckIfPositionOfType(Vector3Int position, CellType cellType)
    {
        return _placementGrid[position.x, position.z] == cellType;
    }

    public List<Vector3Int> GetNeighboursOfType(Vector3Int tempPos, CellType type)
    {
        var neighbourVertices = _placementGrid.GetAdjacentCellsOfType(tempPos.x, tempPos.z, type);
        List<Vector3Int> neighbours = new List<Vector3Int>();
        foreach(var point in neighbourVertices)
        {
            neighbours.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return neighbours;
    }

    public void PlaceTempStructure(Vector3Int position, GameObject structurePrefab, CellType cellType)
    {
        _placementGrid[position.x, position.z] = cellType;
        var structure = CreateNewStructureModel(position, structurePrefab, cellType);
        _tempRoadObjects.Add(position, structure);
        DestroyNatureAt(position);

    }

    private StructureModel CreateNewStructureModel(Vector3Int position, GameObject structPrefab, CellType type)
    {
        GameObject structure = new GameObject(type.ToString());
        structure.transform.SetParent(transform);
        structure.transform.localPosition = position;
        var structureModel = structure.AddComponent<StructureModel>();
        structureModel.CreateModel(structPrefab);
        return structureModel;
    }

    public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
    {
        if(_tempRoadObjects.ContainsKey(position))
        {
            _tempRoadObjects[position].SwapModel(newModel, rotation);
        }
        else if(_structureDictionary.ContainsKey(position))
        {
            _structureDictionary[position].SwapModel(newModel, rotation);
        }
    }

    public void RemoveAllTempStructures()
    {
        foreach(var structure in _tempRoadObjects.Values)
        {
            var pos = Vector3Int.RoundToInt(structure.transform.position);
            _placementGrid[pos.x, pos.z] = CellType.Empty;
            Destroy(structure.gameObject);
        }

        _tempRoadObjects.Clear();
    }

    public void AddTempStructureToDictionary()
    {
        foreach (var structure in _tempRoadObjects)
        {
            _structureDictionary.Add(structure.Key, structure.Value);
            DestroyNatureAt(structure.Key);
        }
        _tempRoadObjects.Clear();
    }
    public List<Vector3Int> GetPathBetween(Vector3Int startPos, Vector3Int endPos)
    {
        var result = GridSearch.AStarSearch(_placementGrid, new Point(startPos.x, startPos.z), new Point(endPos.x, endPos.z));
        List<Vector3Int> path = new List<Vector3Int>();
        foreach(Point point in result)
        {
            path.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return path;
    }

}
