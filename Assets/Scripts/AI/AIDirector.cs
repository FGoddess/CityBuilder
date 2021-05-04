using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    [SerializeField] private GameObject[] _humanPrefabs;

    public void SpawnAllAgents()
    {
        foreach(var house in PlacementManager.Instance.GetAllHouses())
        {
            TrySpawnAgent(house,  PlacementManager.Instance.GetRandomSpecialStrucutre());
        }
        foreach(var special in PlacementManager.Instance.GetAllSpecialStructures())
        {
            TrySpawnAgent(special, PlacementManager.Instance.GetRandomHouseStructure());
        }
    }

    private void TrySpawnAgent(StructureModel startStruct, StructureModel endStruct)
    {
        if(startStruct != null && endStruct != null)
        {
            Debug.Log("smth working");
            var startPos = ((INeedingRoad)startStruct).RoadPosition;
            var endPos = ((INeedingRoad)endStruct).RoadPosition;
            var agent = Instantiate(GetRandomHuman(), startPos, Quaternion.identity);
            var path = PlacementManager.Instance.GetPathBetween(startPos, endPos, true);
            if(path.Count > 0)
            {
                path.Reverse();
                var aiAgent = agent.GetComponent<AIAgent>();
                aiAgent.Initialize(new List<Vector3>(path.Select(x => (Vector3)x).ToList()));
            }
        }

    }

    private GameObject GetRandomHuman()
    {
        return _humanPrefabs[UnityEngine.Random.Range(0, _humanPrefabs.Length)];
    }
}
