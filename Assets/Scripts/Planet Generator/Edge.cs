using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{

	public Vertex A { get { return vertexA; } }
	public Vertex B { get { return vertexB; } }

	readonly Vertex vertexA;
	readonly Vertex vertexB;

	public Edge (Vertex _vertexA, Vertex _vertexB)
	{
		vertexA = _vertexA;
		vertexB = _vertexB;
	}

	public bool Equals (Edge edge)
	{
		return edge.vertexA.Equals (vertexA) && edge.vertexB.Equals (vertexB);
	}

	public bool IsOpposite (Edge edge)
	{
		return edge.vertexA.Equals (vertexB) && edge.vertexB.Equals (vertexA);
	}

	public bool Contains (Vertex vertex)
	{
		return vertex.Equals (vertexA) || vertex.Equals (vertexB);
	}

	public float Length ()
	{
		return Vector3.Distance (A, B);
	}

	public Vector3 GetNormal ()
	{
		return Vector3.Cross (A, B);
	}

}