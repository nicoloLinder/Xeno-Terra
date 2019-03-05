using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaSphere
{

	#region Variables

	#region PublicVariables

	#endregion

	#region PrivateVariables

	List<Cell> cells;
	Dictionary<Vertex, Vertex> vertices;
	Dictionary<Edge, Edge> edges;

	#endregion

	#endregion

	#region Properties

	public List<Cell> Cells { get { return cells; } }


	#endregion

	#region Constructor

	public HexaSphere (IcoSphere icosphere)
	{
		cells = new List<Cell> ();
		vertices = new Dictionary<Vertex, Vertex>();
		Cell newCell;
		foreach (Vertex vertex in icosphere.Vertices) {
			newCell = new Cell (vertex);
			cells.Add (newCell);
			foreach (Triangle t in vertex.Triangles){
				Vertex cellVertex = new Vertex (t.Center);
				if (!vertices.ContainsKey(cellVertex)) {
					vertices.Add (cellVertex, cellVertex);
					newCell += cellVertex;
				} else{
					newCell += vertices [cellVertex];
				}
			}
			newCell.ConnectVerices();
		}

		foreach(Cell c1 in cells){
			if(c1.NeighbourCount > 5){
				continue;
			}
			foreach(Cell c2 in cells){
				if(c2.NeighbourCount > 5){
					continue;
				}
				if(c1 != c2 && !c1.IsNeighbour(c2)){
					if(c1.CheckForNeighbourConditions (c2)){
						c2.AddNeighbour (c1);
					}

				}
				if (c1.NeighbourCount > 5) {
					break;
				}
			}
		}
	}

	#endregion

	#region Methods

	#region PublicMethods

	#endregion

	#region PrivateMethods

	#endregion

	#endregion
}
