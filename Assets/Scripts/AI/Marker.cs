using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public Vector3 Position { get => transform.position; }

    [SerializeField] private List<Marker> adjacentMarkers;

    [SerializeField] private bool _isOpenForConnections;

    public bool IsOpenForConnections { get => _isOpenForConnections; }

    public List<Vector3> GetAdjacentPositions() => new List<Vector3>(adjacentMarkers.Select(x => x.Position).ToList());


}
