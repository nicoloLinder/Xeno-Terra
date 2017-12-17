using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientTexture : MonoBehaviour
{

	#region Variables

	#region PublicVariables

	public Texture2D gradientTexture;
	public List<ColorTuple> colors;
	public int colorIndex = 0;
	public ColorTuple selectedColor;

	#endregion

	#region PrivateVariables

	#endregion

	#endregion

	#region Properties

	#endregion

	#region MonoBehabiourMethods

	void Awake ()
	{

	}

	void Start ()
	{
		gradientTexture = new Texture2D (1024, 1);
		gradientTexture.anisoLevel = 0;
		gradientTexture.wrapMode = TextureWrapMode.Clamp;
		gradientTexture.filterMode = FilterMode.Bilinear;
		colorIndex--;
		ChangeColor ();
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

	void Update()
	{
		//if(Input.GetKeyDown (KeyCode.B)){
		//	ChangeColor ();
		//}
	}

	#endregion

	#region Methods

	#region PublicMethods

	#endregion

	#region PrivateMethods

	public void ChangeColor() {
		colorIndex = ((colorIndex < colors.Count - 1) ? colorIndex + 1 : 0);
		selectedColor = colors [colorIndex];
		GetComponent<MeshRenderer> ().sharedMaterial.mainTexture = gradientTexture;
		for (int i = 0; i < gradientTexture.width; i++){
			//Debug.Log (i / (float)gradientTexture.width);
			gradientTexture.SetPixel (i, 0, Color.Lerp (selectedColor.colorL, selectedColor.colorR, i/(float)gradientTexture.width));
		}
		gradientTexture.Apply ();
		//Camera.main.backgroundColor = selectedColor.colorL;
	}

	#endregion

	#endregion
}

[System.Serializable]
public class ColorTuple{
	public Color colorL = Color.white;
	public Color colorR = Color.white;
}