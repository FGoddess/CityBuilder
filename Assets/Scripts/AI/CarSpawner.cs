using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _carsPrefabs;
    private void Start()
    {
        Instantiate(SelectACarPrefab(), transform);
    }

    private GameObject SelectACarPrefab()
    {
        var randomIndex = UnityEngine.Random.Range(0, _carsPrefabs.Length);
        return _carsPrefabs[randomIndex];
    }
}
