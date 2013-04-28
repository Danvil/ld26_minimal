using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct ExplosionSite
{
	public Vector3 position;
	public float time;
};

public class BombManager : MonoBehaviour {
	
	const float THROW_VEL = 4.3f;
	
	public GameObject pfBomb;
	public GameObject pfMegaBomb;

	List<ExplosionSite> explosions = new List<ExplosionSite>();
	
	List<Bomb> bombs = new List<Bomb>();
	
	void Awake()
	{
		Globals.BombManager = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void DamageLiving(ExplosionSite site, Bomb bomb)
	{
		foreach(Living x in Globals.BlobManager.GetLifeBehaviours()) {
			if(Globals.Level.IsPathBlocked(site.position, x.transform.position)) {
				// in cover
				continue;
			}
			float d = (x.transform.position - site.position).magnitude;
			float dmg = Mathf.Max(0.0f, bomb.Damage / (1.0f + bomb.DistFalloffStrength*d*d) - 1.5f);
			if(bomb.IsPlayerBomb == (x == Globals.Player.living)) {
				dmg *= x.FriendlyFireMult;
			}
			if(dmg > 0) {
				x.Health -= dmg;
			}
		}
	}
	
	public Bomb[] GetBombs() {
		return bombs.ToArray();
	}
	
	public ExplosionSite[] GetExplosions() {
		return explosions.ToArray();
	}
	
	public void ThrowBomb(Vector3 start, Vector3 target, bool isplayer, bool ismega)
	{
		GameObject go;
		if(ismega) {
			go = (GameObject)Instantiate(pfMegaBomb);
		}
		else {
			go = (GameObject)Instantiate(pfBomb);
		}
		go.transform.parent = this.transform;
		go.transform.position = start;
		go.rigidbody.velocity = (target - start).normalized * THROW_VEL;
		Bomb bomb = go.GetComponent<Bomb>();
		bomb.IsPlayerBomb = isplayer;
		bombs.Add(bomb);
	}
	
	public void ExplodeBomb(Bomb bomb)
	{
		bombs.Remove(bomb);
		// mark explosion sight
		ExplosionSite site = new ExplosionSite();
		site.position = bomb.transform.position;
		site.time = MyTime.time;
		explosions.Add(site);
		// damage living
		DamageLiving(site, bomb);
	}

}
