using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	const float SHOOT_TIMEOUT = 1.0f;
	const float PLAYER_RADIUS = 0.2f;
	const float PLAYER_VELOCITY = 3.53f;
	
	public int MegaBombCoins = 10;
	
	float shootTimeout = 0;
	
	public Living living { get; private set; }

	public int NumCoinsCollected = 0;
	public int NumEnemiesKilled = 0;
	
	public int NumCoins = 0;
	
	public AudioClip audioGetMegaBomb;
	
	public GameObject pfBombInventory;
	
	public List<GameObject> inventory = new List<GameObject>();
	
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
		for(int i=0; i<Globals.NumMegaBombs; i++) {
			AddMegaBomb();
		}
	}
	
	void AddMegaBomb()
	{
		GameObject go = (GameObject)Instantiate(pfBombInventory);
		inventory.Add(go);
		go.transform.parent = this.transform;
		float s = 0.5f*go.transform.localScale.x;
		int i = inventory.Count - 1;
		float phi = ((float)i) * 40.0f;
		go.transform.localPosition = new Vector3(s*Mathf.Cos(phi), s*Mathf.Sin(phi), -0.4f);
	}
	
	public void OnPickupCoin()
	{
		NumCoinsCollected ++;
		NumCoins ++;
		if(NumCoins >= MegaBombCoins) {
			NumCoins -= MegaBombCoins;
			Globals.NumMegaBombs ++;
			AddMegaBomb();			
			audio.PlayOneShot(audioGetMegaBomb);
		}
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
		bool fire1 = Input.GetButton("Fire1");
		bool fire2 = Input.GetButton("Fire2");
		if((fire1 || fire2) && shootTimeout <= 0) {
			shootTimeout = SHOOT_TIMEOUT;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			Vector3 target = ray.GetPoint(- ray.origin.z / ray.direction.z);
			Vector3 start = this.transform.position + new Vector3(0,0,-0.5f*this.transform.localScale.x -.3f);
			bool usemega = fire2 && Globals.NumMegaBombs > 0;
			Globals.BombManager.ThrowBomb(start, target, true, usemega);
			if(usemega) {
				Globals.NumMegaBombs --;
				GameObject go = inventory.Last();
				inventory.Remove(go);
				Object.Destroy(go);
			}
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
		foreach(GameObject go in inventory) {
			go.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
		}
	}
	
	float minWinTime = 3.0f;
	
	// Update is called once per frame
	void Update () {
		minWinTime -= MyTime.deltaTime;
		Move();
		Shoot();
		// stop time if dead
		if(living.IsDead) {
			Globals.SceneTransition.Loose();
		}
		// move camera
		UpdateCameraPosition();
		// check for completion
		if(NumCoinsCollected == Globals.Level.NumCoins && NumEnemiesKilled == Globals.Level.NumEnemies) {
			// show princess
			Globals.Princess.ShowPrincess();
		}
		if(minWinTime <= 0.0f && Globals.RoomManager != null && Globals.RoomManager.currentRoom.isBoss) {
			// test if boss is dead
			int bossnum = Globals.BlobManager.GetLifeBehaviours()
				.Where(x => !x.IsDead)
				.Select(x => x.GetComponent<Enemy>())
				.Where(x => x != null)
				.Select(x => x.IsBoss)
				.Count();
			if(bossnum == 0) {
				//WIN THE GAME
				Globals.SceneTransition.Win();
			}
		}
	}
	
}
