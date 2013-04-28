using UnityEngine;
using System.Collections;

public class Rotating : MonoBehaviour {
	
	public float angularVelocity = 90.0f;
	
	// Use this for initialization
	void Start () {
		this.transform.Rotate(new Vector3(0,0,1), Random.Range(0.0f, 360.0f));
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(new Vector3(0,0,1), MyTime.deltaTime*angularVelocity);
	}
}
