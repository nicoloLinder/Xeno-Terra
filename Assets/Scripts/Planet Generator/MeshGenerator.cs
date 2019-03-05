using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{

	#region Variables

	#region PublicVariables

	#endregion

	#region PrivateVariables

	#endregion

	#endregion

	#region Properties

	#endregion

	#region Methods

	#region PublicMethod

	public static Mesh GenerateMesh (List<Vertex> vertices, List<Triangle> triangles, bool faceted, float minHeight, float maxHeight)
	{

		List<Vector3> meshVertices = new List<Vector3> ();
		List<int> meshTriangles = new List<int> ();
		List<Vector2> uvs = new List<Vector2> ();
		List<Vector3> normals = new List<Vector3> ();

		if (!faceted) {
			foreach (Triangle triangle in triangles) {
				float heightPercentageA = (triangle.A.Height - minHeight) / (maxHeight - minHeight);
				float heightPercentageB = (triangle.B.Height - minHeight) / (maxHeight - minHeight);
				float heightPercentageC = (triangle.C.Height - minHeight) / (maxHeight - minHeight);
				float triHeightPercentage = (heightPercentageA + heightPercentageB + heightPercentageC) / 3;
				if((triangle.A).index == -1){
					meshVertices.Add (triangle.A);
					triangle.A.index = meshVertices.Count-1;
					float heightPercentage = (triangle.A.Height - minHeight) / (maxHeight - minHeight);
					uvs.Add (new Vector2 (heightPercentage, 0));				
					normals.Add (triangle.A.CalculateNormal ());
				} 
				if ((triangle.B).index == -1) {
					meshVertices.Add (triangle.B);
					triangle.B.index = meshVertices.Count - 1;
					float heightPercentage = (triangle.B.Height - minHeight) / (maxHeight - minHeight);
					uvs.Add (new Vector2 (heightPercentage, 0));
					normals.Add (triangle.B.CalculateNormal ());
				}
				if ((triangle.C).index == -1) {
					meshVertices.Add (triangle.C);
					triangle.C.index = meshVertices.Count - 1;
					float heightPercentage = (triangle.C.Height - minHeight) / (maxHeight - minHeight);
					uvs.Add (new Vector2 (heightPercentage, 0));
					normals.Add (triangle.C.CalculateNormal ());
				}

				meshTriangles.Add ((triangle.A).index);
				meshTriangles.Add ((triangle.B).index);
				meshTriangles.Add ((triangle.C).index);
			}
		} else {
			foreach (Triangle triangle in triangles) {
				meshVertices.Add (triangle.A);
				meshTriangles.Add (meshVertices.Count - 1);

				meshVertices.Add (triangle.B);
				meshTriangles.Add (meshVertices.Count - 1);

				meshVertices.Add (triangle.C);
				meshTriangles.Add (meshVertices.Count - 1);

				float heightPercentageA = (triangle.A.Height - minHeight) / (maxHeight - minHeight);
				float heightPercentageB = (triangle.B.Height - minHeight) / (maxHeight - minHeight);
				float heightPercentageC = (triangle.C.Height - minHeight) / (maxHeight - minHeight);
				float triHeightPercentage = (heightPercentageA + heightPercentageB + heightPercentageC) / 3;

				float heightPercentage = (triangle.A.Height - minHeight) / (maxHeight - minHeight);
				uvs.Add (new Vector2 (triHeightPercentage, 0));
				heightPercentage = (triangle.B.Height - minHeight) / (maxHeight - minHeight);
				uvs.Add (new Vector2 (triHeightPercentage, 0));
				heightPercentage = (triangle.C.Height - minHeight) / (maxHeight - minHeight);
				uvs.Add (new Vector2 (triHeightPercentage, 0));

				normals.Add (triangle.CalculateNormal ());
				normals.Add (triangle.CalculateNormal ());
				normals.Add (triangle.CalculateNormal ());
			}
		}


		Mesh mesh = new Mesh ();
		mesh.vertices = meshVertices.ToArray ();
		mesh.triangles = meshTriangles.ToArray ();
		mesh.uv = uvs.ToArray ();
		mesh.normals = normals.ToArray ();
		//mesh.RecalculateNormals ();
		return mesh;
	}

	public static Mesh GenerateMesh (IcoSphere icosphere, bool faceted)
	{

		List<Vertex> vertices = icosphere.Vertices;
		List<Triangle> triangles = icosphere.Triangles;

		return GenerateMesh (vertices, triangles, faceted, icosphere.minHeight, icosphere.maxHeight);
	}

	#endregion

	#region PrivateMethods

	#endregion

	#endregion
}
