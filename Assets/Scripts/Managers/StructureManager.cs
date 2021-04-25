using System;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    [SerializeField] private StructurePrefabWeighted[] _housesPrefabs;
    [SerializeField] private StructurePrefabWeighted[] _specialPrefabs;
    [SerializeField] private StructurePrefabWeighted[] _bigStructuresPrefabs;

    private float[] _houseWeights;
    private float[] _specialWeights;
    private float[] _bigStructureWeights;

    private readonly int _width = 2;
    private readonly int _height = 2;


    private static StructureManager _instance;
    public static StructureManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
    }

    private void Start()
    {
        _houseWeights = _housesPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        _specialWeights = _specialPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        _bigStructureWeights = _bigStructuresPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
    }

    public void PlaceHouse(Vector3Int position)
    {
        if (CheckPosition(position))
        {
            int randIndex = GetRandomWeightedIndex(_houseWeights);
            PlacementManager.Instance.PlaceObjOnTheMap(position, _housesPrefabs[randIndex].prefab, CellType.Structure);
            AudioPlayer.Instance.PlayPlacementSound();
        }
    }

    internal void PlaceBigStructure(Vector3Int pos)
    {
        if (CheckBigStructure(pos))
        {
            int randIndex = GetRandomWeightedIndex(_bigStructureWeights);
            PlacementManager.Instance.PlaceObjOnTheMap(pos, _bigStructuresPrefabs[randIndex].prefab, CellType.Structure, _width, _height); //2x2 cell type
            AudioPlayer.Instance.PlayPlacementSound();
        }
    }

    private bool CheckBigStructure(Vector3Int pos)
    {
        bool nearRoad = false;
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                var newPos = pos + new Vector3Int(x, 0, z);
                if (!DefaultCheck(newPos))
                {
                    return false;
                }
                if (!nearRoad)
                {
                    nearRoad = RoadCheck(newPos);
                }
                
            }
        }
        return nearRoad;
    }

    public void PlaceSpecial(Vector3Int position)
    {
        if (CheckPosition(position))
        {
            int randIndex = GetRandomWeightedIndex(_specialWeights);
            PlacementManager.Instance.PlaceObjOnTheMap(position, _specialPrefabs[randIndex].prefab, CellType.Structure);
            AudioPlayer.Instance.PlayPlacementSound();
        }
    }

    private int GetRandomWeightedIndex(float[] weights)
    {
        float sum = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            sum += weights[i];
        }

        float randomValue = UnityEngine.Random.Range(0, sum);
        float tempSum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            if (randomValue >= tempSum && randomValue < tempSum + weights[i])
            {
                return i;
            }
            else
            {
                tempSum += weights[i];
            }
        }
        return 0;
    }

    private bool CheckPosition(Vector3Int position)
    {
        return !DefaultCheck(position) && !RoadCheck(position);
    }

    private bool DefaultCheck(Vector3Int pos)
    {
        if (!PlacementManager.Instance.CheckIfPositionInBound(pos))
        {
            Debug.Log("This position is out of bounds");
            return false;
        }
        if (!PlacementManager.Instance.CheckIfPositionIsFree(pos))
        {
            Debug.Log("This position is not EMPTY");
            return false;
        }
        return true;
    }

    private bool RoadCheck(Vector3Int pos)
    {
        if (PlacementManager.Instance.GetNeighboursOfType(pos, CellType.Road).Count <= 0)
        {
            //Debug.Log("Must be placed near a road");
            return false;
        }
        return true;
    }
}

[Serializable]
public struct StructurePrefabWeighted
{
    public GameObject prefab;
    [Range(0, 1)]
    public float weight;
}
