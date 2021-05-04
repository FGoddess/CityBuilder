using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHelper : MonoBehaviour
{
    [SerializeField] protected List<Marker> _humanMarkers;
    [SerializeField] protected bool _isCorner;
    [SerializeField] protected bool _hasCrosswalks;

    private float _approximateThreesholdCornre = 0.3f;

    public virtual Marker GetPositionForHumanToSpawn(Vector3 structurePosition) => GetClosestMarkerTo(structurePosition, _humanMarkers);

    private Marker GetClosestMarkerTo(Vector3 structurePosition, List<Marker> humanMarkers)
    {
        if(_isCorner)
        {
            foreach(var marker in _humanMarkers)
            {
                var dir = marker.Position - structurePosition;
                if (Mathf.Abs(dir.x) < _approximateThreesholdCornre || Mathf.Abs(dir.z) < _approximateThreesholdCornre)
                {
                    return marker;
                }
            }
            return null;
        }
        else
        {
            Marker closestMarker = null;
            float distance = float.MaxValue;
            foreach (var marker in _humanMarkers)
            {
                var markerDistance = Vector3.Distance(structurePosition, marker.Position);
                if(distance > markerDistance)
                {
                    distance = markerDistance;
                    closestMarker = marker;
                }
            }
            return closestMarker;
        }

    }

    public Vector3 GetClosestHumanToPosition(Vector3 currentPos) => GetClosestMarkerTo(currentPos, _humanMarkers).Position;

    public List<Marker> GetAllHumanMarkers() => _humanMarkers; //property


}
