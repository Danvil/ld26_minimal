
using UnityEngine;
using System.Collections;
using System.Linq;

public class Enemy : MonoBehaviour {
	
	const float GOAL_REACHED_DIST = 0.1f;
	public float VELOCITY_SLOW = 0.75f;
	public float VELOCITY_FAST = 1.35f;
	public float PLAYER_APPROACH_DIST = 2.4f;
	public float BOMB_TIMEOUT = 1.7f;
	public float ALARM_TIMEOUT = 4.3f;
	
	public float AimProbability = 1.0f;
	public float MegaBombProbability = 0.0f;
	public int NumBombs = 1;
	public float BombStartHeight = -0.8f;
	
	public bool IsBoss = false;

	BlobMove move;
	Living life;
	
	float goalTime = 0.0f;
	
	float bombTimeout = 0.0f;
	
	float alarmTimeout = 0.0f;
	
	bool hasLastPlayerPos = false;
	Vector3 lastPlayerPos;

	public bool IsAlarmed
	{
		get { return alarmTimeout <= ALARM_TIMEOUT; }
	}
	
	void UpdateGoal()
	{
		move.PlayerMinDistance = 0.0f;
		// go to explosion
		ExplosionSite[] sites = Globals.BombManager.GetExplosions();
		if(sites.Length > 0) {
			ExplosionSite site = sites.Last();
			// check if new goal
			if(site.time > goalTime) {
				// check if in sight
				if(!Globals.Level.IsPathBlocked(this.transform.position, site.position)) {
					move.SetGoal(site.position);
					goalTime = MyTime.time;
				}
			}
		}
		// go to player
		Vector3 player_pos = Globals.Player.transform.position;
		if(!Globals.Level.IsPathBlocked(this.transform.position, player_pos) && (player_pos - this.transform.position).magnitude <= 5.0f) {
			move.PlayerMinDistance = PLAYER_APPROACH_DIST;
			hasLastPlayerPos = true;
			lastPlayerPos = player_pos;
			alarmTimeout = 0.0f;
			Vector3 delta = player_pos - this.transform.position;
			if(delta.magnitude < PLAYER_APPROACH_DIST) {
				move.DisableGoal();
				// throw grenade
				bool isMega = (Random.value < MegaBombProbability);
				if(bombTimeout >= BOMB_TIMEOUT) {
					if(Random.value < AimProbability) {
						if(NumBombs == 1) {
							Globals.BombManager.ThrowBomb(this.transform.position + new Vector3(0,0,BombStartHeight), player_pos, false, isMega);
						}
						else {
							for(int i=0; i<NumBombs; i++) {
								float dx = Random.Range(-0.2f, +0.2f);
								float dy = Random.Range(-0.2f, +0.2f);
								Globals.BombManager.ThrowBomb(this.transform.position + new Vector3(dx,dy,BombStartHeight), player_pos, false, isMega);
							}
						}
					}
					else {
						float deltaPhi = 6.283f / (float)(NumBombs);
						float phi = 0.0f;
						for(int i=0; i<NumBombs; i++, phi+=deltaPhi) {
							float dx = Mathf.Cos(phi);
							float dy = Mathf.Sin(phi);
							Globals.BombManager.ThrowBomb(
								this.transform.position + new Vector3(0.4f*dx,0.4f*dy,BombStartHeight),
								this.transform.position + new Vector3(5.0f*dx, 5.0f*dy, 0.0f),
								false, isMega);
						}
					}
					bombTimeout = 0.0f;
				}
			}
			else {
				move.SetGoal(player_pos);
			}
		}
		else {
			if(hasLastPlayerPos) {
				move.SetGoal(lastPlayerPos);
				if(move.isGoalReached()) {
					hasLastPlayerPos = false;
					move.DisableGoal();
				}
			}
		}
		
	}
	
	void Start()
	{
		move = this.GetComponent<BlobMove>();
		life = this.GetComponent<Living>();
		Globals.BlobManager.AddBlob(gameObject);
	}

	public bool isKilled = false;
	
	void Update()
	{
		bombTimeout += MyTime.deltaTime;
		alarmTimeout += MyTime.deltaTime;
		if(life.IsDead) {
			if(!isKilled) {
				Globals.Player.NumEnemiesKilled ++;
				isKilled = true;
			}
			move.speed = 0.0f;
		}
		else {
			move.speed = (IsAlarmed ? VELOCITY_FAST : VELOCITY_SLOW);
			UpdateGoal();
		}
	}

}
