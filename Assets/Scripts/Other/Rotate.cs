using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

	#region Variables

	#region PublicVariables

	public float speed;
	public float moveSpeed;
	public float lerpFactor;
	public Vector3 direction;

	public float tapTimeLength;
	public float maxTapDuration;
	public float maxTapDistace;

	#endregion

	#region PrivateVariables

	Vector2 startPos;
	Vector2 fingerPos;
	Vector2 lerpFingerPos;

	float timeSinceLastTap;
	float tapDuration;
	bool fingerOnThing;

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

	private void Update ()
	{
		if (!fingerOnThing && Vector3.Distance (fingerPos, lerpFingerPos) < 20) {
			transform.Rotate (direction, speed * Time.deltaTime, Space.World);
			//ScreenCapture.CaptureScreenshot ("Screenshot.png");
		} else if (Vector3.Distance (fingerPos, lerpFingerPos) > 20) {
			lerpFingerPos = Vector2.Lerp (lerpFingerPos, fingerPos, lerpFactor);
			float speedPercentX = Mathf.Abs (lerpFingerPos.x - fingerPos.x) / 25;
			float speedPercentY = Mathf.Abs (lerpFingerPos.y - fingerPos.y) / 25;
			Vector3 dir;
			//if (Mathf.Abs (lerpFingerPos.x - fingerPos.x) > Mathf.Abs (lerpFingerPos.y - fingerPos.y)) {
			//	dir = Vector3.up * Mathf.Sign (lerpFingerPos.x - fingerPos.x);
			//} else{
			//	dir = Vector3.left * Mathf.Sign (lerpFingerPos.y - fingerPos.y);
			//}
			Vector3 temp = (lerpFingerPos - fingerPos).normalized;
			dir = new Vector3 (temp.x, -temp.y);
			transform.Rotate (Vector3.up * dir.x, moveSpeed * Time.deltaTime * speedPercentX, Space.World);
			transform.Rotate (Vector3.right * dir.y, moveSpeed * Time.deltaTime * speedPercentY, Space.World);


		}



		if (GetFingerDown () && GetFingerHit ()) {
			fingerOnThing = true;
			startPos = FingerPosition ();
			fingerPos = startPos;
			lerpFingerPos = startPos;
		} else if (GetFinger () && fingerOnThing) {
			fingerPos = FingerPosition ();
			tapDuration += Time.deltaTime;
			 
		} else if (GetFingerUp () && fingerOnThing){
			fingerOnThing = false;
			if(timeSinceLastTap < tapTimeLength && tapDuration < maxTapDuration && Vector3.Distance (startPos, fingerPos) < maxTapDistace){
				GetComponent<PlanetGenerator>().StartChangePlanet();
			}
			tapDuration = 0;
			timeSinceLastTap = 0;
		} else if(timeSinceLastTap < tapTimeLength){
			timeSinceLastTap += Time.deltaTime;
		}
	}

	#endregion

	#region Methods

	#region PublicMethods

	#endregion

	#region PrivateMethods

	bool GetFingerDown ()
	{
		return Input.GetMouseButtonDown (0);

	}

	bool GetFinger ()
	{
		return Input.GetMouseButton (0);
	}

	bool GetFingerUp ()
	{
		return Input.GetMouseButtonUp (0);
	}

	bool GetFingerHit ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100)) {
			return true;
		}
		return false;
	}

	Vector2 FingerPosition ()
	{
		//Vector2 fingerPos = new Vector3 (Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
		return Input.mousePosition;
	}


	#endregion

	#endregion
}
