
using UnityEngine;
using System.Collections;
using System.Linq;

public class Enemy : MonoBehaviour {
	
	const float GOAL_REACHED_DIST = 0.1f;
	const float VELOCITY_SLOW = 0.75f;
	const float VELOCITY_FAST = 1.35f;
	const float PLAYER_APPROACH_DIST = 2.4f;
	const float BOMB_TIMEOUT = 1.7f;
	const float ALARM_TIMEOUT = 4.3f;

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
		if(!Globals.Level.IsPathBlocked(this.transform.position, player_pos)) {
			move.PlayerMinDistance = PLAYER_APPROACH_DIST;
			hasLastPlayerPos = true;
			lastPlayerPos = player_pos;
			alarmTimeout = 0.0f;
			Vector3 delta = player_pos - this.transform.position;
			if(delta.magnitude < PLAYER_APPROACH_DIST) {
				move.DisableGoal();
				// throw grenade
				if(bombTimeout >= BOMB_TIMEOUT) {
					Globals.BombManager.ThrowBomb(this.transform.position + new Vector3(0,0,-.8f), player_pos);
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
