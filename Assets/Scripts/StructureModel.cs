using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureModel : MonoBehaviour
{
    private float yHeight = 0;

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
}
