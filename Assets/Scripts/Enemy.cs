
using UnityEngine;
using System.Collections;
using System.Linq;

public class Enemy : MonoBehaviour {
	
	const float GOAL_REACHED_DIST = 0.1f;
	const float VELOCITY_SLOW = 0.8f;
	const float VELOCITY_FAST = 1.8f;

	bool isAlarmed = false;
	
	BlobMove move;
	
	void UpdateGoal()
	{
		// go to explosion
		ExplosionSite[] sites = Globals.BombManager.GetExplosions();
		if(sites.Length > 0) {
			ExplosionSite site = sites.Last();
			// check if new goal
			if(site.time > move.goalTime) {
				// check if in sight
				if(!Globals.Level.IsPathBlocked(this.transform.position, site.position)) {
					move.SetGoal(site.position);
				}
			}
		}
		// go to player
		
	}
	
	void Awake()
	{
		Globals.BlobManager.AddBlob(gameObject);
		move = this.GetComponent<BlobMove>();
	}
	
	void Start()
	{
	
	}
	
	void MoveToGoal()
	{
		move.speed = (isAlarmed ? VELOCITY_FAST : VELOCITY_SLOW);
	}
	
	void Update()
	{
		// move to bomb explosion
		// move around a bit
		// throw bombs at player
		UpdateGoal();
		MoveToGoal();
	}

}
