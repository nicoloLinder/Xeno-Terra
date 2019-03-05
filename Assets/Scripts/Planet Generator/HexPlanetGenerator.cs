using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexPlanetGenerator : MonoBehaviour
{

	#region Variables

	#region PublicVariables

	[Range (1, 6)]
	public int divisions;

	public Vector2 birth;
	public Vector2 death;
	public int iterationCount;

	#endregion

	#region PrivateVariables

	IcoSphere icosphere;
	HexaSphere hexaSphere;

	static HexPlanetGenerator instance;

	#endregion

	#endregion

	#region Properties

	public static HexPlanetGenerator Instance { get { return instance; } }

	#endregion

	#region MonoBehabiourMethods


	void Awake ()
	{
		if(instance == null){
			instance = this;
		} else if(instance != this){
			Destroy (this);
		}
	}

	void Start ()
	{
		CreatePlanet ();
		CellularAutomata.IterateHexaSphere (ref hexaSphere, birth, death, iterationCount);
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

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.B)) {
			CreatePlanet ();
			CellularAutomata.IterateHexaSphere (ref hexaSphere, birth, death, iterationCount);
		}
	}

	private void OnDrawGizmos ()
	{
		if (icosphere != null) {	
			foreach (Cell c in hexaSphere.Cells) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube (c.Center.Coordinates * 10, Vector3.one);
				if (c.CellState) {
					Gizmos.color = new Color (0.5f, 0.5f, 1f, 0.5f);
				} else {
					Gizmos.color = new Color (1, 0, .5f, 0.5f);
				}
				foreach (Vertex v in c.Vertices) {
					foreach(Vertex v1 in v.Vertices){
						if (c.CellState) {
							Gizmos.DrawLine (v1.Coordinates * 10, v.Coordinates * 10);
						} else {
							Gizmos.DrawLine (v1.Coordinates * 10.5f, v.Coordinates * 10.5f);
						}
					}
					Gizmos.DrawCube (v.Coordinates * 10, Vector3.one);
				}
				//foreach(Cell n in c.Neighbours){
				//	Gizmos.DrawLine (c.Center.Coordinates * 10, n.Center.Coordinates * 10);
				//}
			}

		}
	}

	#endregion

	#region Methods

	#region PublicMethods

	public void CreatePlanet ()
	{
		icosphere = new IcoSphere (divisions);
		hexaSphere = new HexaSphere (icosphere);
	}


	#endregion

	#region PrivateMethods

	#endregion

	#endregion
}
