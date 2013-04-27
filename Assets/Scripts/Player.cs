using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	const float SHOOT_TIMEOUT = 1.0f;
	const float THROW_VEL = 4.3f;
	
	float shootTimeout = 0;
	
	Living living;
	
	// Use this for initialization
	void Start () {
		living = GetComponent<Living>();
		Globals.Player = this;
	}
	
	// Update is called once per frame
	void Update () {
		shootTimeout -= MyTime.deltaTime;
		// move with keyboard
		const float V = 1.83f;
		float D = V * MyTime.deltaTime;
		if(Input.GetKey(KeyCode.A)) {
			this.transform.Translate(new Vector3(-D,0,0));
		}
		if(Input.GetKey(KeyCode.D)) {
			this.transform.Translate(new Vector3(+D,0,0));
		}
		if(Input.GetKey(KeyCode.W)) {
			this.transform.Translate(new Vector3(0,+D,0));
		}
		if(Input.GetKey(KeyCode.S)) {
			this.transform.Translate(new Vector3(0,-D,0));
		}
		// shoot and aim with mouse
		if(Input.GetButton("Fire1") && shootTimeout <= 0) {
			shootTimeout = SHOOT_TIMEOUT;
			Ray ray = Globals.MainCamera.ScreenPointToRay(Input.mousePosition); 
			Vector3 target = ray.GetPoint(- ray.origin.z / ray.direction.z);
			Vector3 start = this.transform.position + new Vector3(0,0,-.8f);
			Vector3 dir = (target - start).normalized;
			Globals.BombManager.ThrowBomb(start, THROW_VEL*dir);
		}
		// stop time if dead
		if(living.IsDead) {
			MyTime.Pause = true;
		}
		// move camera
		Globals.MainCamera.transform.position = new Vector3(
			0.5f*this.transform.position.x,
			0.5f*this.transform.position.y,
			Globals.MainCamera.transform.position.z);
	}
	
	void OnGUI()
	{
		if(living.IsDead) {
			Debug.Log("game over");
			GUI.Label(new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/2), "Game Over!");
		}
	}
}
