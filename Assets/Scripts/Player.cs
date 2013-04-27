using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		const float V = 1.83f;
		float D = V * MyTime.deltaTime;
		if(Input.GetKey(KeyCode.LeftArrow)) {
			this.transform.Translate(new Vector3(-D,0,0));
		}
		if(Input.GetKey(KeyCode.RightArrow)) {
			this.transform.Translate(new Vector3(+D,0,0));
		}
		if(Input.GetKey(KeyCode.UpArrow)) {
			this.transform.Translate(new Vector3(0,+D,0));
		}
		if(Input.GetKey(KeyCode.DownArrow)) {
			this.transform.Translate(new Vector3(0,-D,0));
		}
	}
}
