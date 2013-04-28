using UnityEngine;
using System.Collections;

public class Wobble : MonoBehaviour {
	
	public float centerZ = 0.0f;
	public float wobbleMax = 0.3f;
	public float wobbleForce = 0.5f;
	
	float dir = 1.0f;
	float z;

	// Use this for initialization
	void Start () {
		z = centerZ;
	}
	
	// Update is called once per frame
	void Update () {
		float target = centerZ + dir*wobbleMax;
		z += (1.3f*target - z) * wobbleForce * MyTime.deltaTime;
		if(dir > 0) {
			if(z > target) dir = -1.0f;
		}
		else {
			if(z < target) dir = +1.0f;
		}
		this.transform.position = this.transform.position.WithChangedZ(z);
	}
}
