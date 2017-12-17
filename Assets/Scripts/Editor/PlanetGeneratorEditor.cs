using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (PlanetGenerator))]
public class PlanetGeneratorEditor : Editor
{

	#region Variables

	#region PublicVariables

	#endregion

	#region PrivateVariables

	#endregion

	#endregion

	#region Properties

	#endregion

	#region MonoBehabiourMethods

	public override void OnInspectorGUI ()
	{
		PlanetGenerator planetGen = (PlanetGenerator)target;
		GradientTexture gradientTexture = planetGen.GetComponent<GradientTexture> ();
		base.OnInspectorGUI ();

		if (GUILayout.Button ("Generate")) {
			planetGen.CreatePlanet ();
			//planetGen.CreateDetailedPlanet ();
			//gradientTexture.ChangeColor ();
		}
		if (GUILayout.Button ("Recreate")) {
			planetGen.icosphere = null;
			planetGen.CreatePlanet ();
			//planetGen.CreateDetailedPlanet ();
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
