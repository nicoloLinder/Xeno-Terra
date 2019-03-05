using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Triangle
{

	public Vertex A { get { return vertexA; } }
	public Vertex B { get { return vertexB; } }
	public Vertex C { get { return vertexC; } }
	public Vertex Center { get { return center; } }
	public Edge AB { get { return edgeAB; } }
	public Edge BC { get { return edgeBC; } }
	public Edge CA { get { return edgeCA; } }


	readonly Vertex vertexA;
	readonly Vertex vertexB;
	readonly Vertex vertexC;

	Vertex center;


	readonly Edge edgeAB;
	readonly Edge edgeBC;
	readonly Edge edgeCA;

	Vector3 normal;

	/*
	 * 
	 Nx = UyVz - UzVy

	 Ny = UzVx - UxVz

	 Nz = UxVy - UyVx
	 * 
	 */

	public Triangle (Vertex _vertexA, Vertex _vertexB, Vertex _vertexC)
	{
		vertexA = _vertexA;
		vertexB = _vertexB;
		vertexC = _vertexC;

		center = new Vertex ((vertexA + vertexB + vertexC) / 3);

		edgeAB = new Edge (vertexA, vertexB);
		edgeBC = new Edge (vertexB, vertexC);
		edgeCA = new Edge (vertexC, vertexA);

		vertexA.AddTriangle (this);
		vertexB.AddTriangle (this);
		vertexC.AddTriangle (this);

		vertexA.AddVertex (vertexB);
		vertexA.AddVertex (vertexC);

		vertexB.AddVertex (vertexA);
		vertexB.AddVertex (vertexC);

		vertexC.AddVertex (vertexA);
		vertexC.AddVertex (vertexB);
	}

	public override bool Equals (System.Object obj)
	{
		if (obj == null || GetType () != obj.GetType ()) {
			return false;
		}

		Triangle triangle = (Triangle)obj;
		return triangle.Contains (vertexA) && triangle.Contains (vertexB) && triangle.Contains (vertexC);
	}

	public override int GetHashCode ()
	{
		return vertexA.GetHashCode () * vertexB.GetHashCode () * vertexC.GetHashCode ();
	}

	public bool Contains (Vertex vertex)
	{
		return vertex.Equals (vertexA) || vertex.Equals (vertexB) || vertex.Equals (vertexC);
	}

	public bool Contains (Edge edge)
	{
		return edge.Equals (edgeAB) || edge.Equals (edgeBC) || edge.Equals (edgeCA);
	}

	public bool Contains (Vector3 point)
	{
		//contained = (v • n0 > 0) && (v • n1 > 0) && (v • n2 > 0)
		return Vector3.Dot (point, edgeAB.GetNormal ()) > 0 && Vector3.Dot (point, edgeBC.GetNormal ()) > 0 && Vector3.Dot (point, edgeCA.GetNormal ()) > 0;
	}

	public void DeleteTriangle ()
	{
		vertexA.RemoveTriangle (this);
		vertexB.RemoveTriangle (this);
		vertexC.RemoveTriangle (this);

		vertexA.RemoveVertex (vertexB);
		vertexA.RemoveVertex (vertexC);

		vertexB.RemoveVertex (vertexA);
		vertexB.RemoveVertex (vertexC);

		vertexC.RemoveVertex (vertexA);
		vertexC.RemoveVertex (vertexB);
	}

	public Vector3 CalculateNormal ()
	{
		Vector3 U = (Vector3)vertexB - vertexA;
		Vector3 V = (Vector3)vertexC - vertexA;
		normal = Vector3.Cross (U, V);
		return normal;
	}

	public Edge[] GetEdgesWithVertex(Vertex v){
		Edge [] edges = new Edge [2];
		if(!edgeAB.Contains(v)){
			edges [0] = edgeBC;
			edges [1] = edgeCA;
		} else if (!edgeBC.Contains (v)) {
			edges [0] = edgeAB;
			edges [1] = edgeCA;
		} else if (!edgeCA.Contains (v)) {
			edges [0] = edgeAB;
			edges [1] = edgeBC;
		}else {
			Debug.Log ("This triangle does not contain this vertex");
			return null;
		}
		return edges;
	}
}