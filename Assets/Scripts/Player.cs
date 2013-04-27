using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	const float SHOOT_TIMEOUT = 1.0f;
	const float THROW_VEL = 4.3f;
	const float PLAYER_RADIUS = 0.2f;
	const float PLAYER_VELOCITY = 2.53f;
	
	float shootTimeout = 0;

	public AudioClip[] audioCoin;
	
	Living living;
	
	// Use this for initialization
	void Start () {
		living = GetComponent<Living>();
		Globals.Player = this;
		// position player
		this.transform.position = new Vector3(
			Globals.Level.PlayerStart.x,
			Globals.Level.PlayerStart.y,
			this.transform.position.z);
	}
	
	void Move()
	{
		// move with keyboard
		Vector3 movedir = Vector3.zero;
		if(Input.GetKey(KeyCode.A)) {
			movedir += new Vector3(-1,0,0);
		}
		if(Input.GetKey(KeyCode.D)) {
			movedir += new Vector3(+1,0,0);
		}
		if(Input.GetKey(KeyCode.W)) {
			movedir += new Vector3(0,+1,0);
		}
		if(Input.GetKey(KeyCode.S)) {
			movedir += new Vector3(0,-1,0);
		}
		if(movedir.magnitude > 0.0f) {
			movedir = movedir.normalized * PLAYER_VELOCITY * MyTime.deltaTime;
			Vector3 newpos = this.transform.position + movedir;
			if(!Globals.Level.IsBlocking(newpos, PLAYER_RADIUS)) {
				this.transform.position = newpos;
			}
		}
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
		Vector3 campos = Globals.Level.LevelCenter + 0.4f*(this.transform.position - Globals.Level.LevelCenter);
		Globals.MainCamera.transform.position = new Vector3(campos.x, campos.y, Globals.MainCamera.transform.position.z);
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
