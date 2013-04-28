using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	const float SHOOT_TIMEOUT = 1.0f;
	const float PLAYER_RADIUS = 0.2f;
	const float PLAYER_VELOCITY = 3.53f;
	
	float shootTimeout = 0;
	
	public Living living { get; private set; }

	public int NumCoinsCollected = 0;
	public int NumEnemiesKilled = 0;
	
	void Awake()
	{
		Globals.Player = this;
	}
	
	// Use this for initialization
	void Start()
	{
		Globals.BlobManager.AddBlob(gameObject);
		living = GetComponent<Living>();
		// position player
		this.transform.position = new Vector3(
			Globals.Level.PlayerStart.x,
			Globals.Level.PlayerStart.y,
			0.1f);
		this.transform.localScale = new Vector3(Globals.PlayerSize, Globals.PlayerSize, Globals.PlayerSize);
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
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			Vector3 target = ray.GetPoint(- ray.origin.z / ray.direction.z);
			Vector3 start = this.transform.position + new Vector3(0,0,-0.5f*this.transform.localScale.x -.3f);
			Globals.BombManager.ThrowBomb(start, target, true);
		}
	}

	void UpdateCameraPosition()
	{
		const float MARG = 2.5f;
		float w = (float)(Globals.Level.Cols);
		float h = (float)(Globals.Level.Rows);
		float px = this.transform.position.x;
		float py = this.transform.position.y;
		if(px < MARG) px = MARG;
		if(px > w - MARG) px = w - MARG;
		if(py < MARG) py = MARG;
		if(py > h - MARG) py = h - MARG;
		Camera.main.transform.position = new Vector3(px, py, Camera.main.transform.position.z);
	}
	
	public void EatPotatoe()
	{
		living.HealthMax += 5.0f;
		living.Health += 5.0f;
		float growth = 0.20f / this.transform.localScale.x;
		this.transform.localScale += new Vector3(growth, growth, growth);
		Globals.PlayerSize = this.transform.localScale.x;
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
		UpdateCameraPosition();
		// check for completion
		if(NumCoinsCollected == Globals.Level.NumCoins && NumEnemiesKilled == Globals.Level.NumEnemies) {
			// show princess
			Globals.Princess.ShowPrincess();
		}
	}
	
	void OnGUI()
	{
		if(living.IsDead) {
			GUI.Label(new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/2), "Game Over!");
		}
	}
}
