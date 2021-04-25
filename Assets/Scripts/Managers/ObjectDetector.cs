using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    private static ObjectDetector _instance;
    public static ObjectDetector Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
    }

    [SerializeField] private LayerMask _groundMask;

    public Vector3Int? RaycastGround(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundMask))
        {
            Transform objectHit = hit.transform;
            Vector3Int positionInt = Vector3Int.RoundToInt(hit.point);
            return positionInt;
        }
        return null;
    }

    public GameObject RaycastAll(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.transform.gameObject;
        }
        return null;
    }
}
