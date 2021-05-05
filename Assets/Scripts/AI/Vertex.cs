using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : IEquatable<Vertex>
{
    public Vector3 Position { get; set; }

    public Vertex(Vector3 position)
    {
        this.Position = position;
    }

    public bool Equals(Vertex other)
    {
        return Vector3.SqrMagnitude(Position - other.Position) < 0.001f; //Mathf.Epsilon;
    }

    public override string ToString()
    {
        return Position.ToString();
    }
}
