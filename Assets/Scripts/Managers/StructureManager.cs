using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class StructureManager : MonoBehaviour
{
    public StructurePrefabWeighted[] housesPrefabs;
    public StructurePrefabWeighted[] specialPrefabs;

    private float[] _houseWeights;
    private float[] _specialWeights;


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
        _houseWeights = housesPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        _specialWeights = specialPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
    }

    public void PlaceHouse(Vector3Int position)
    {
        if(CheckPosition(position))
        {
            int randIndex = GetRandomWeightedIndex(_houseWeights);
            PlacementManager.Instance.PlaceObjOnTheMap(position, housesPrefabs[randIndex].prefab, CellType.Structure);
            AudioPlayer.Instance.PlayPlacementSound();
        }
    }
    public void PlaceSpecial(Vector3Int position)
    {
        if(CheckPosition(position))
        {
            int randIndex = GetRandomWeightedIndex(_specialWeights);
            PlacementManager.Instance.PlaceObjOnTheMap(position, specialPrefabs[randIndex].prefab, CellType.Structure);
            AudioPlayer.Instance.PlayPlacementSound();
        }
    }

    private int GetRandomWeightedIndex(float[] weights)
    {
        float sum = 0f;
        for(int i = 0; i < weights.Length; i++)
        {
            sum += weights[i];
        }

        float randomValue = UnityEngine.Random.Range(0, sum);
        float tempSum = 0;
        for(int i = 0; i < weights.Length; i++)
        {
            if(randomValue >= tempSum && randomValue < tempSum + weights[i])
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
        if(!PlacementManager.Instance.CheckIfPositionInBound(position) || !PlacementManager.Instance.CheckIfPositionIsFree(position))
        {
            return false;
        }
        if(PlacementManager.Instance.GetNeighboursOfType(position, CellType.Road).Count <= 0)
        {
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
