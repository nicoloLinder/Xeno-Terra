using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{

	public Vertex Center { get { return center; } }
	public List<Vertex> Vertices { get { return vertices; } }
	public List<Cell> Neighbours { get { return neighbours; } }
	public int NeighbourCount { get { return neighbours.Count; } }

	public bool CellState { get { return cellState; } set { prevCellState = cellState; cellState = value; } }
	public bool PrevCellState { get { return prevCellState; } }

	Vertex center;

	List<Edge> edges;
	List<Vertex> vertices;
	List<Cell> neighbours;

	bool cellState;
	bool prevCellState;

	public Cell (Vertex _center)
	{
		center = _center;
		vertices = new List<Vertex> ();
		neighbours = new List<Cell> ();
		CellState = Random.Range (0, 2) == 1;
	}

	public Cell (Vertex _center, params Vertex [] _vertices)
	{
		center = _center;
		vertices = new List<Vertex> (vertices);
		neighbours = new List<Cell> ();
		CellState = Random.Range (0, 2) == 1;
	}

	public static Cell operator + (Cell c1, Vertex v1)
	{
		foreach (Vertex v in c1.vertices) {
			if (v.Equals (v1)) {
				Debug.Log ("This cell already contains vertex at position: " + v1.ToString ());
				return c1;
			}
		}
		c1.vertices.Add (v1);

		return c1;
	}

	public void ConnectVerices ()
	{
		foreach (Vertex v1 in vertices) {
			foreach (Vertex v2 in vertices) {
				if (!v1.Equals (v2) && Vertex.Distance (v1, v2) < (10 / Mathf.Pow (2, HexPlanetGenerator.Instance.divisions))) {
					v1.AddVertex (v2);
				}
			}
		}
	}

	public bool Contains (Vertex v)
	{
		return vertices.Contains (v);
	}

	public bool IsNeighbour (Cell c)
	{
		return neighbours.Contains (c);
	}

	public bool CheckForNeighbourConditions (Cell c)
	{
		foreach (Vertex v in vertices) {
			if (c.Contains (v)) {
				neighbours.Add (c);
				return true;
			}
		}
		return false;
	}

	public void AddNeighbour (Cell c)
	{
		neighbours.Add (c);
	}

}
