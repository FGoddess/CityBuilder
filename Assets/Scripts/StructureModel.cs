using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureModel : MonoBehaviour, INeedingRoad
{
    private float yHeight = 0;

    public Vector3Int RoadPosition { get; set; }

    public void CreateModel(GameObject model)
    {
        GameObject temp = Instantiate(model, transform);
        yHeight = temp.transform.position.y;
    }

    public void SwapModel(GameObject model, Quaternion rotation)
    {
        foreach(Transform item in transform)
        {
            Destroy(item.gameObject);
        }

        GameObject structure = Instantiate(model, transform);
        structure.transform.localPosition = new Vector3(0, yHeight, 0);
        structure.transform.localRotation = rotation;

    }

    public Vector3 GetNearestMarkerTo(Vector3 position)
    {
        return transform.GetChild(0).GetComponent<RoadHelper>().GetClosestHumanToPosition(position);
    }

    public Marker GetHumanSpawnMarker(Vector3 position)
    {
        return transform.GetChild(0).GetComponent<RoadHelper>().GetPositionForHumanToSpawn(position);
    }

    public List<Marker> GetHumanMarkers()
    {
        return transform.GetChild(0).GetComponent<RoadHelper>().GetAllHumanMarkers();
    }
}
