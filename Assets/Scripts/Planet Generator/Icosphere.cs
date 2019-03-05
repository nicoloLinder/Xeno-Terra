using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;

public class IcoSphere
{

	#region Variables

	#region PublicVariables

	public Perlin noise = new Perlin ();
    public RidgedMultifractal ridge = new RidgedMultifractal();

    #endregion

    #region PrivateVariables

    List<Vertex> vertices = new List<Vertex> ();
	List<Edge> edges = new List<Edge> ();
	List<Triangle> triangles = new List<Triangle> ();

	public float maxHeight = float.MinValue;
	public float minHeight = float.MaxValue;

	public Mesh mesh;

	#endregion

	#endregion

	#region Properties

	public List<Vertex> Vertices { get { return vertices; } }
	public List<Triangle> Triangles { get { return triangles; } }

	#endregion


	//private void OnDrawGizmos ()
	//{
	//	foreach (Vertex v in vertices) {
	//		//float height = Mathf.PerlinNoise (v.x * 20, v.y * 20);
	//		//Gizmos.color = new Color (height, height, height, 1);
	//		Gizmos.DrawCube (v.UV, Vector3.one / 200);
	//	}
	//	//foreach (Vector2 v in centers) {
	//	//	Gizmos.color = new Color(1, 1, 1, 0.5f);
	//	//	Gizmos.DrawCube (v, Vector3.one / 100);
	//	//}

	//	//foreach (Triangle t in triangles) {
	//	//Vector2 pos = new Vector2 (0.5f + (Mathf.Atan2 (t.Center.Z, t.Center.X) / (Mathf.PI * 2)), 0.5f - (Mathf.Asin (t.Center.Y) / Mathf.PI));
	//	//float height = Mathf.PerlinNoise (pos.x, pos.y)*3;
	//	//Gizmos.color = new Color (height, height, height, 1);
	//	//Gizmos.DrawCube (t.Center, Vector3.one / 100);
	//	//Gizmos.DrawLine (uvs [vertices.IndexOf (t.A)], uvs [vertices.IndexOf (t.B)]);
	//	//Gizmos.DrawLine (uvs [vertices.IndexOf (t.B)], uvs [vertices.IndexOf (t.C)]);
	//	//Gizmos.DrawLine (uvs [vertices.IndexOf (t.C)], uvs [vertices.IndexOf (t.A)]);
	//	//}

	//}

	#region Constructor

	public IcoSphere(int divisions){
		GenerateIcosahedron ();

		for (int i = 0; i < divisions; i++) {
			Subdivide ();
		}
	}

	public IcoSphere (int divisions, int octaves, float persistance, float lacunarity, float scale, AnimationCurve heightCurve, float heightMultiplyer, bool variableMaxHeight, int seed)
	{
        SetNoiseParameters(seed, octaves, persistance, lacunarity);
		GenerateIcosahedron ();

		for (int i = 0; i < divisions; i++) {
			Subdivide ();
		}

		ApplyNoiseMap (scale, heightCurve, heightMultiplyer);



		if (variableMaxHeight) {
			maxHeight = float.MinValue;
			minHeight = float.MaxValue;

			foreach (Vertex v in vertices) {
				if (v.Height < minHeight) {
					minHeight = v.Height;
				}
				if (v.Height > maxHeight) {
					maxHeight = v.Height;
				}
			}
		} else{
			minHeight = 10;
			maxHeight = 10 + heightMultiplyer * heightCurve.Evaluate(1);
		}
	}

	public IcoSphere (int divisions, int octaves, float persistance, float lacunarity, float scale, AnimationCurve heightCurve, float heightMultiplyer, int seed, Vector3 position)
	{
		noise.Seed = seed;
		noise.Persistence = persistance;
		noise.Lacunarity = lacunarity;
		noise.OctaveCount = octaves;
		GenerateIcosahedron ();
		float centerDistance = (Vector3.Distance (((Vector3)triangles [0].Center).normalized, ((Vector3)triangles [1].Center).normalized)) * 1.5f;
		//Subdivide ();
		//Subdivide ();
		for (int i = 1; i < divisions; i++) {
			//float centerDistance = (Vector3.Distance (((Vector3)triangles [0].Center).normalized, ((Vector3)triangles [1].Center).normalized)) * 2;
			List<Triangle> oldTrinagles = new List<Triangle> (triangles);
			foreach(Triangle t in oldTrinagles){
				//if(t.Contains (position)){
				//	SubdivideTriangle (t);
				//}
				if (Vector3.Distance(position.normalized, ((Vector3)t.Center).normalized) < centerDistance/i) {
					SubdivideTriangle (t);
				}
			}
		}

		ApplyNoiseMap (scale, heightCurve, heightMultiplyer);

		//minHeight = 10;
		//maxHeight = 10 + heightMultiplyer * heightCurve.Evaluate(1);

		foreach (Vertex v in vertices) {
			if (v.Height < minHeight) {
				minHeight = v.Height;
			}
			if (v.Height > maxHeight) {
				maxHeight = v.Height;
			}
		}
	}

	public void Recreate (int octaves, float persistance, float lacunarity, float scale, AnimationCurve heightCurve, float heightMultiplyer, bool variableMaxHeight, int seed)
	{
        SetNoiseParameters(seed, octaves, persistance, lacunarity);

        foreach (Vertex vertex in vertices) {
			vertex.Normalize ();
			vertex.index = -1;
		}

		ApplyNoiseMap (scale, heightCurve, heightMultiplyer);

		if (variableMaxHeight) {
			maxHeight = float.MinValue;
			minHeight = float.MaxValue;

			foreach (Vertex v in vertices) {
				if (v.Height < minHeight) {
					minHeight = v.Height;
				}
				if (v.Height > maxHeight) {
					maxHeight = v.Height;
				}
			}
		} else{
			minHeight = 10;
			maxHeight = 10 + heightMultiplyer * heightCurve.Evaluate(1);
		}
	}

	public void Reset(){
		foreach (Vertex vertex in vertices) {
			vertex.index = -1;
		}
	}

	#endregion

	#region Methods

	#region PublicMethods

	#endregion


	#region PrivateMethods

	void GenerateIcosahedron ()
	{

		float S = 0.1f + 2 * (1 / Mathf.Sqrt (((1 + Mathf.Sqrt (5)) / 2) * Mathf.Sqrt (5)));
		//float t1 = 2 * Mathf.PI / 5;
		float t2 = Mathf.PI / 10;
		float t4 = Mathf.PI / 5;
		//float t3 = -3 * Mathf.PI / 10;
		float R = (S / 2) / Mathf.Sin (t4);
		float H = Mathf.Cos (t4) * R;
		float Cx = R * Mathf.Sin (t2);
		float Cz = R * Mathf.Cos (t2);
		float H1 = Mathf.Sqrt (S * S - R * R);
		float H2 = Mathf.Sqrt ((H + R) * (H + R) - H * H);
		float Y2 = (H2 - H1) / 2.0f;
		float Y1 = Y2 + H1;

		vertices.Add (new Vertex (0, Y1, 0).Normalize ());
		vertices.Add (new Vertex (R, Y2, 0).Normalize ());
		vertices.Add (new Vertex (Cx, Y2, Cz).Normalize ());
		vertices.Add (new Vertex (-H, Y2, S / 2).Normalize ());
		vertices.Add (new Vertex (-H, Y2, -S / 2).Normalize ());
		vertices.Add (new Vertex (Cx, Y2, -Cz).Normalize ());
		vertices.Add (new Vertex (-R, -Y2, 0).Normalize ());
		vertices.Add (new Vertex (-Cx, -Y2, -Cz).Normalize ());
		vertices.Add (new Vertex (H, -Y2, -S / 2).Normalize ());
		vertices.Add (new Vertex (H, -Y2, S / 2).Normalize ());
		vertices.Add (new Vertex (-Cx, -Y2, Cz).Normalize ());
		vertices.Add (new Vertex (0, -Y1, 0).Normalize ());

		triangles.Add (new Triangle (vertices [2], vertices [1], vertices [0]));
		triangles.Add (new Triangle (vertices [3], vertices [2], vertices [0]));
		triangles.Add (new Triangle (vertices [4], vertices [3], vertices [0]));
		triangles.Add (new Triangle (vertices [5], vertices [4], vertices [0]));
		triangles.Add (new Triangle (vertices [1], vertices [5], vertices [0]));

		triangles.Add (new Triangle (vertices [2], vertices [9], vertices [1]));
		triangles.Add (new Triangle (vertices [10], vertices [9], vertices [2]));
		triangles.Add (new Triangle (vertices [3], vertices [10], vertices [2]));
		triangles.Add (new Triangle (vertices [6], vertices [10], vertices [3]));
		triangles.Add (new Triangle (vertices [4], vertices [6], vertices [3]));
		triangles.Add (new Triangle (vertices [7], vertices [6], vertices [4]));
		triangles.Add (new Triangle (vertices [5], vertices [7], vertices [4]));
		triangles.Add (new Triangle (vertices [8], vertices [7], vertices [5]));
		triangles.Add (new Triangle (vertices [1], vertices [8], vertices [5]));
		triangles.Add (new Triangle (vertices [9], vertices [8], vertices [1]));

		triangles.Add (new Triangle (vertices [10], vertices [11], vertices [9]));
		triangles.Add (new Triangle (vertices [6], vertices [11], vertices [10]));
		triangles.Add (new Triangle (vertices [7], vertices [11], vertices [6]));
		triangles.Add (new Triangle (vertices [8], vertices [11], vertices [7]));
		triangles.Add (new Triangle (vertices [9], vertices [11], vertices [8]));
	}

	void Subdivide ()
	{
		List<Triangle> oldTrinagles = new List<Triangle> (triangles);

		foreach (Triangle t in oldTrinagles) {
			SubdivideTriangle (t);
		}

		//scale *= 2;
		//transform.localScale = new Vector3 (scale, scale, scale);

	}

	void SubdivideTriangle (Triangle triangle)
	{
		triangles.Remove (triangle);
		triangle.DeleteTriangle ();

		//Vertex d = new Vertex ((triangle.A + triangle.B) * 0.5f).Normalize ();
		//Vertex e = new Vertex ((triangle.B + triangle.C) * 0.5f).Normalize ();
		//Vertex f = new Vertex ((triangle.C + triangle.A) * 0.5f).Normalize ();

		Vertex d = new Vertex ((triangle.A.Coordinates + triangle.B.Coordinates) * 0.5f).Normalize ();
		Vertex e = new Vertex ((triangle.B.Coordinates + triangle.C.Coordinates) * 0.5f).Normalize ();
		Vertex f = new Vertex ((triangle.C.Coordinates + triangle.A.Coordinates) * 0.5f).Normalize ();

		//Debug.Log ((triangle.A.Contains (d) && triangle.B.Contains (d)) + "ADB");
		//Debug.Log ((triangle.B.Contains (e) && triangle.C.Contains (e)) + "BEC");
		//Debug.Log ((triangle.C.Contains (f) && triangle.A.Contains (f)) + "CFA");

		if (triangle.A.AddVertex (d) && triangle.B.AddVertex (d)) {
			vertices.Add (d);
		} else {
			d = triangle.A.GetEqual (d);
		}
		if (triangle.B.AddVertex (e) && triangle.C.AddVertex (e)) {
			vertices.Add (e);
		} else {
			e = triangle.B.GetEqual (e);
		}
		if (triangle.C.AddVertex (f) && triangle.A.AddVertex (f)) {
			vertices.Add (f);
		} else {
			f = triangle.C.GetEqual (f);
		}

		triangles.Add (new Triangle (triangle.A, d, f));
		triangles.Add (new Triangle (d, triangle.B, e));
		triangles.Add (new Triangle (f, e, triangle.C));
		triangles.Add (new Triangle (d, e, f));
	}

	void ApplyNoiseMap (float scale, AnimationCurve heightCurve, float heightMultiplyer)
	{
		foreach (Vertex vertex in vertices) {
			//vertex.Move (noiseMap [(int)((noiseMap.GetLength (0) - 1) * vertex.U), (int)((noiseMap.GetLength (1) - 1) * vertex.V)]);
            float perlinValue = (float)noise.GetValue (vertex.Coordinates.normalized * scale) + (float)ridge.GetValue(vertex.Coordinates.normalized * (scale));
			perlinValue = heightCurve.Evaluate (perlinValue) * heightMultiplyer;

			vertex.Move (perlinValue);
		}
	}

    void SetNoiseParameters(int seed, int octaves, float persistance, float lacunarity)
    {
        noise.Seed = seed;
        noise.Persistence = persistance;
        noise.Lacunarity = lacunarity;
        noise.OctaveCount = octaves;

        ridge.Seed = seed;
        ridge.Lacunarity = lacunarity;
        ridge.OctaveCount = octaves;
    }

	#endregion

	#endregion

}






