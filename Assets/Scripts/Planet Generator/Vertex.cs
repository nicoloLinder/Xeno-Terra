using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
	public float U { get { return uv.x; } }
	public float V { get { return uv.y; } }
	public float X { get { return coordinates.x; } }
	public float Y { get { return coordinates.y; } }
	public float Z { get { return coordinates.z; } }
	public float Height { get { return Vector3.Distance (Vector3.zero, coordinates); } }

	public Vector3 Coordinates { get { return coordinates; } set { coordinates = value; } }
	public Vector2 UV { get { return uv; } }

	Vector3 coordinates;
	Vector2 uv;
	Vector3 normal;
	List<Triangle> triangles = new List<Triangle> ();

	HashSet<Vertex> vertices = new HashSet<Vertex> ();

	public int index = -1;

	public Vertex (Vector3 _coordinates)
	{
		coordinates = _coordinates;
		uv = new Vector2 (0.5f + (Mathf.Atan2 (Z, X) / (Mathf.PI * 2)), 0.5f - (Mathf.Asin (Y) / Mathf.PI));
	}

	public Vertex (float x, float y, float z)
	{
		coordinates = new Vector3 (x, y, z);
		uv = new Vector2 (0.5f + (Mathf.Atan2 (Z, X) / (Mathf.PI * 2)), 0.5f - (Mathf.Asin (Y) / Mathf.PI));
	}

	public override bool Equals (System.Object obj)
	{
		if (obj == null || GetType () != obj.GetType ()) {
			return false;
		}

		Vertex vertex = (Vertex)obj;

		return vertex.X == X && vertex.Y == Y && vertex.Z == Z;
	}

	public override int GetHashCode ()
	{
		return (int)(X + (Y * 255) + (Z * 255));
	}

	public override string ToString ()
	{
		return coordinates.ToString ();
	}

	//public static bool operator == (Vertex v1, Vertex v2)
	//{
	//	return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
	//}

	//public static bool operator != (Vertex v1, Vertex v2)
	//{
	//	return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
	//}

	public static Vertex operator + (Vertex v1, Vertex v2)
	{
		return new Vertex (v1.coordinates + v2.coordinates);
	}

	public static Vertex operator * (Vertex v1, float scalar)
	{
		return new Vertex (v1.coordinates *= scalar);
	}

	public static Vertex operator / (Vertex v1, float scalar)
	{
		return new Vertex (v1.coordinates /= scalar);
	}

	public static implicit operator Vector3 (Vertex v)
	{
		return v.coordinates;
	}

	public Vertex Normalize ()
	{
		coordinates.Normalize ();
		uv = new Vector2 (0.5f + (Mathf.Atan2 (Z, X) / (Mathf.PI * 2)), 0.5f - (Mathf.Asin (Y) / Mathf.PI));
		coordinates *= 10;
		return this;
	}

	public void Move (float amount)
	{
		//radius = Vector3.Distance (Vector3.zero, coordinates)
		coordinates += coordinates.normalized * amount;

	}

	public void AddTriangle (Triangle triangle)
	{
		if (!triangles.Contains (triangle)) {
			triangles.Add (triangle);
		}
	}

	public void RemoveTriangle (Triangle triangle)
	{
		if (triangles.Contains (triangle)) {
			triangles.Remove (triangle);
		}
	}

	public bool AddVertex (Vertex vertex)
	{
		return vertices.Add (vertex);
	}

	public void RemoveVertex (Vertex vertex)
	{
		vertices.Remove (vertex);
	}

	public bool Contains (Vertex vertex)
	{
		return vertices.Contains (vertex);
	}

	public Vertex GetEqual (Vertex vertex)
	{
		foreach (Vertex v in vertices) {
			if (vertex.Equals (v)) {
				return v;
			}
		}
		return null;
	}

	public bool IsNeighbour (Vertex vertex)
	{
		return vertices.Contains (vertex);
	}

	public Vector3 CalculateNormal ()
	{
		normal = Vector3.zero;
		foreach (Triangle triangle in triangles) {
			normal += triangle.CalculateNormal ();
		}
		return normal;
	}

}