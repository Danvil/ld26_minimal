using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	const float SHOOT_TIMEOUT = 1.0f;
	const float THROW_VEL = 4.3f;
	const float PLAYER_RADIUS = 0.2f;
	
	float shootTimeout = 0;

	public AudioClip[] audioCoin;
	
	Living living;
	
	// Use this for initialization
	void Start () {
		living = GetComponent<Living>();
		Globals.Player = this;
	}
	
	void Move()
	{
		// move with keyboard
		const float V = 1.83f;
		float D = V * MyTime.deltaTime;
		Vector3 newpos = this.transform.position;
		if(Input.GetKey(KeyCode.A)) {
			newpos += new Vector3(-D,0,0);
		}
		if(Input.GetKey(KeyCode.D)) {
			newpos += new Vector3(+D,0,0);
		}
		if(Input.GetKey(KeyCode.W)) {
			newpos += new Vector3(0,+D,0);
		}
		if(Input.GetKey(KeyCode.S)) {
			newpos += new Vector3(0,-D,0);
		}
//		Vector3 dir = newpos - this.transform.position;
//		RaycastHit[] info = rigidbody.SweepTestAll(dir.normalized, dir.magnitude);
//		bool hits = false;
//		foreach(RaycastHit h in info) {
//			if(h.point.z > 0.5) {
//				hits = true;
//			}
//		}
//		if(!hits) {
			this.transform.position = newpos;
//		}
	}
	
	void Shoot()
	{
		shootTimeout -= MyTime.deltaTime;
		// shoot and aim with mouse
		if(Input.GetButton("Fire1") && shootTimeout <= 0) {
			shootTimeout = SHOOT_TIMEOUT;
			Ray ray = Globals.MainCamera.ScreenPointToRay(Input.mousePosition); 
			Vector3 target = ray.GetPoint(- ray.origin.z / ray.direction.z);
			Vector3 start = this.transform.position + new Vector3(0,0,-.8f);
			Vector3 dir = (target - start).normalized;
			Globals.BombManager.ThrowBomb(start, THROW_VEL*dir);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Move();
		Shoot();
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
	
	void PickupCoin(Coin coin)
	{
		audio.PlayOneShot(audioCoin[Random.Range(0,audioCoin.Length-1)]);
		Object.Destroy(coin.gameObject);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		Coin coin = collision.gameObject.GetComponent<Coin>();
		if(coin != null) {
			PickupCoin(coin);
		}
	}

	void OnGUI()
	{
		if(living.IsDead) {
			Debug.Log("game over");
			GUI.Label(new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/2), "Game Over!");
		}
	}
}
