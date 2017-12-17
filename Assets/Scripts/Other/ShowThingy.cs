using UnityEngine;

public class ShowThingy : MonoBehaviour
{
	public void Toggle(){
		GetComponent<Animator> ().SetTrigger ("Move");
		GetComponent<Animator> ().SetBool ("Up", !GetComponent<Animator> ().GetBool ("Up"));
	}
}
