using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{

	#region Variables

	#region PublicVariables

	public int subdivisions;
	public int octaves;
	[Range (0, 1)]
	public float persistance;
	public float lacunarity;
	[Range (0, 10)]
	public float scale;
	public float heighMultiplyer;
	public AnimationCurve heightCurve;
	public bool flatShading;
	public bool variableMaxHeight;

	public bool randomSeed;
	public int seed;


	public float lerpTime;

	public bool drawWire;




	#endregion

	#region PrivateVariables

	public IcoSphere icosphere;
	public IcoSphere[] LODs = new IcoSphere[6];
	IcoSphere current;

	Animator animator;

	bool changing;

	#endregion

	#endregion

	#region Properties

	#endregion

	#region MonoBehabiourMethods


	void Awake ()
	{
		animator = GetComponent<Animator> ();
	}

	void Start ()
	{
		CreatePlanet ();
	}

	void OnEnable ()
	{

	}

	void OnDisable ()
	{

	}

	void OnDestroy ()
	{

	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.B)) {
			
			CreatePlanet ();
		}
	}

	private void OnDrawGizmos ()
	{
		Gizmos.DrawCube (Vector3.one, Vector3.one / 10);
		if (current != null && drawWire) {
			Gizmos.color = new Color (0, 0, 0, 0.5f);
			Gizmos.DrawWireMesh (current.mesh, transform.position, transform.rotation, transform.localScale);
		}
		//foreach(Vertex vertex in icosphere.Vertices){
		//	float height = noiseMap [(int)((noiseMap.GetLength (0) - 1) * vertex.U), (int)((noiseMap.GetLength (1) - 1) * vertex.V)];
		//	Gizmos.color = new Color (height, height, height, 1);
		//	Gizmos.DrawCube (vertex.UV,Vector3.one/100);
		//}
	}

	#endregion

	#region Methods

	#region PublicMethods

	public void ChangeResolution(int resolution) {
		switch (resolution) {
		case 0:
			ChangeLOD (LODs[resolution], resolution, true);
			break;
		case 1:
			ChangeLOD (LODs [resolution], resolution, true);
			break;
		case 2:
			ChangeLOD (LODs [resolution], resolution, true);
			break;
		case 3:
			ChangeLOD (LODs [resolution], resolution, true);
			break;
		case 4:
			ChangeLOD (LODs [resolution], resolution, true);
			break;
		case 5:
			ChangeLOD (LODs [resolution], resolution, true);
			break;
		case 6:
			ChangeLOD (icosphere, resolution, false);
			break;
		}
	
	}

	public void StartChangePlanet(){
		if(!changing){
			changing = true;
			animator.SetTrigger ("Shrink");
		}
	}

	public void StopChanging(){
		changing = false;
	}

	public void CreatePlanet(){
		if (randomSeed) {
			seed = (int)System.DateTime.Now.Ticks;
		}
		Random.InitState (seed);
		//scale = Random.Range (0.5f, 1.5f);

		if (icosphere == null) {
			icosphere = new IcoSphere (subdivisions, octaves, persistance, lacunarity, scale, heightCurve, heighMultiplyer, variableMaxHeight, seed);
		} else{
			icosphere.Recreate (octaves, persistance, lacunarity, scale, heightCurve, heighMultiplyer, variableMaxHeight, seed);
		}
		icosphere.mesh = MeshGenerator.GenerateMesh (icosphere, flatShading);
		GetComponent<MeshFilter>().mesh = icosphere.mesh;
		//GetComponent<MeshCollider>().sharedMesh = mesh;

		foreach(IcoSphere LOD in LODs){
			if (LOD != null) {
				LOD.mesh = null;
			}
		}
		if (LODs [3] == null) {
			LODs [3] = new IcoSphere (3, octaves, persistance, lacunarity, scale, heightCurve, heighMultiplyer, variableMaxHeight, seed);
		} else{
			LODs[3].Recreate (octaves, persistance, lacunarity, scale, heightCurve, heighMultiplyer, variableMaxHeight, seed);
		}

		current = icosphere;

		LODs[3].mesh = MeshGenerator.GenerateMesh (LODs [3], true);
		GetComponent<MeshCollider> ().sharedMesh = LODs[3].mesh;
		animator.SetTrigger ("Grow");
	}

	public void ChangeLOD(IcoSphere lodSphere, int resolution, bool flatShading){
		current = lodSphere;
		if (lodSphere == null && resolution < 6) {
			lodSphere = LODs[resolution] = new IcoSphere (resolution, octaves, persistance, lacunarity, scale, heightCurve, heighMultiplyer, variableMaxHeight, seed);
			lodSphere.mesh = MeshGenerator.GenerateMesh (lodSphere, flatShading);
		} else if(lodSphere.mesh == null && resolution < 6){
			lodSphere.Recreate (octaves, persistance, lacunarity, scale, heightCurve, heighMultiplyer, variableMaxHeight, seed);
			lodSphere.mesh = MeshGenerator.GenerateMesh (lodSphere, flatShading);
		} 
		GetComponent<MeshFilter> ().mesh = lodSphere.mesh;
		//GetComponent<MeshCollider>().sharedMesh = mesh;
		//animator.SetTrigger ("Grow");
	}

	public void CreateDetailedPlanet ()
	{
		if (randomSeed) {
			seed = (int)System.DateTime.Now.Ticks;
		}
		Random.InitState (seed);
		//scale = Random.Range (0.5f, 1.1f);

		icosphere = new IcoSphere (subdivisions, octaves, persistance, lacunarity, scale, heightCurve, heighMultiplyer, seed, Vector3.one);

		icosphere.mesh = MeshGenerator.GenerateMesh (icosphere, flatShading);
		GetComponent<MeshFilter> ().mesh = icosphere.mesh;
	}


	#endregion

	#region PrivateMethods

	#endregion

	#endregion
}
