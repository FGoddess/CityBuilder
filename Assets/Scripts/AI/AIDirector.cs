﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    [SerializeField] private GameObject[] _humanPrefabs;

    private AdjacencyGraph graph = new AdjacencyGraph();

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

    private void TrySpawnAgent(StructureModel startStructure, StructureModel endStructure)
    {
        if (startStructure != null && endStructure != null)
        {
            var startPosition = ((INeedingRoad)startStructure).RoadPosition;
            var endPosition = ((INeedingRoad)endStructure).RoadPosition;

            var startMarkerPosition = PlacementManager.Instance.GetStructureAt(startPosition).GetNearestMarkerTo(startStructure.transform.position);
            var endMarkerPosition = PlacementManager.Instance.GetStructureAt(endPosition).GetNearestMarkerTo(endStructure.transform.position);

            var agent = Instantiate(GetRandomPedestrian(), startMarkerPosition, Quaternion.identity);
            var path = PlacementManager.Instance.GetPathBetween(startPosition, endPosition, true);
            if (path.Count > 0)
            {
                path.Reverse();
                List<Vector3> agentPath = GetPedestrianPath(path, startMarkerPosition, endMarkerPosition);
                var aiAgent = agent.GetComponent<AIAgent>();
                aiAgent.Initialize(agentPath);
            }
        }
    }

    private List<Vector3> GetPedestrianPath(List<Vector3Int> path, Vector3 startPosition, Vector3 endPosition)
    {
        graph.ClearGraph();
        CreatAGraph(path);
        Debug.Log(graph);
        return AdjacencyGraph.AStarSearch(graph, startPosition, endPosition);
    }

    private void CreatAGraph(List<Vector3Int> path)
    {
        Dictionary<Marker, Vector3> tempDictionary = new Dictionary<Marker, Vector3>();

        for (int i = 0; i < path.Count; i++)
        {
            var currentPosition = path[i];
            var roadStructure = PlacementManager.Instance.GetStructureAt(currentPosition);
            var markersList = roadStructure.GetHumanMarkers();
            bool limitDistance = markersList.Count == 4;
            tempDictionary.Clear();
            foreach (var marker in markersList)
            {
                graph.AddVertex(marker.Position);
                foreach (var markerNeighbourPosition in marker.GetAdjacentPositions())
                {
                    graph.AddEdge(marker.Position, markerNeighbourPosition);
                }

                if (marker.IsOpenForConnections && i + 1 < path.Count)
                {
                    var nextRoadStructure = PlacementManager.Instance.GetStructureAt(path[i + 1]);
                    if (limitDistance)
                    {
                        tempDictionary.Add(marker, nextRoadStructure.GetNearestMarkerTo(marker.Position));
                    }
                    else
                    {
                        graph.AddEdge(marker.Position, nextRoadStructure.GetNearestMarkerTo(marker.Position));
                    }
                }
            }
            if (limitDistance && tempDictionary.Count == 4)
            {
                var distanceSortedMarkers = tempDictionary.OrderBy(x => Vector3.Distance(x.Key.Position, x.Value)).ToList();
                for (int j = 0; j < 2; j++)
                {
                    graph.AddEdge(distanceSortedMarkers[j].Key.Position, distanceSortedMarkers[j].Value);
                }
            }
        }
    }

    private GameObject GetRandomPedestrian()
    {
        return _humanPrefabs[UnityEngine.Random.Range(0, _humanPrefabs.Length)];
    }

    private void Update()
    {
        foreach (var vertex in graph.GetVertices())
        {
            foreach (var vertexNeighbour in graph.GetConnectedVerticesTo(vertex))
            {
                Debug.DrawLine(vertex.Position + Vector3.up, vertexNeighbour.Position + Vector3.up, Color.red);
            }
        }
    }
}
