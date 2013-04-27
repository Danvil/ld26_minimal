using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		const float D = 0.1f;
		if(Input.GetKey(KeyCode.LeftArrow)) {
			camera.transform.Translate(new Vector3(-D,0,0));
		}
		if(Input.GetKey(KeyCode.RightArrow)) {
			camera.transform.Translate(new Vector3(+D,0,0));
		}
		if(Input.GetKey(KeyCode.UpArrow)) {
			camera.transform.Translate(new Vector3(0,+D,0));
		}
		if(Input.GetKey(KeyCode.DownArrow)) {
			camera.transform.Translate(new Vector3(0,-D,0));
		}
	}
}
