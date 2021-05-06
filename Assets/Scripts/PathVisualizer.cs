using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    private static PathVisualizer _instance;
    public static PathVisualizer Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
    }

    private LineRenderer _lineRenderer;
    private AIAgent _currentAgent;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;
    }

    public void ShowPath(List<Vector3> path, AIAgent agent, Color color)
    {
        ResetPath();
        _lineRenderer.positionCount = path.Count;
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;

        for(int i = 0; i < path.Count; i++)
        {
            _lineRenderer.SetPosition(i, path[i] + new Vector3(0, agent.transform.position.y, 0));
        }

        _currentAgent = agent;
        _currentAgent.OnDeath += ResetPath;

    }

    public void ResetPath()
    {
        if(_lineRenderer != null)
        {
            _lineRenderer.positionCount = 0;
        }
        if(_currentAgent != null)
        {
            _currentAgent.OnDeath -= ResetPath;
        }
        _currentAgent = null;
    }
}
