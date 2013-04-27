using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct ExplosionSite
{
	public Vector3 position;
	public float time;
};

public class BombManager : MonoBehaviour {
	
	public GameObject pfBomb;

	List<ExplosionSite> explosions = new List<ExplosionSite>();
	
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
	
	void DamageLiving(ExplosionSite site)
	{
		foreach(Living x in Globals.LivingManager.GetLivings()) {
			float d = (x.transform.position - site.position).magnitude;
			float dmg = Mathf.Max(0.0f, 12.0f / (1.0f + 1.5f*d*d) - 1.0f);
			if(dmg > 0) {
				x.Health -= dmg;
			}
		}
	}
	
	public ExplosionSite[] GetExplosions() {
		return explosions.ToArray();
	}
	
	public void ThrowBomb(Vector3 start, Vector3 vel)
	{
		GameObject go = (GameObject)Instantiate(pfBomb);
		go.transform.parent = this.transform;
		go.transform.position = start;
		go.rigidbody.velocity = vel;
	}
	
	public void ExplodeBomb(Bomb bomb)
	{
		// mark explosion sight
		ExplosionSite site = new ExplosionSite();
		site.position = bomb.transform.position;
		site.time = MyTime.time;
		explosions.Add(site);
		// damage living
		DamageLiving(site);
	}

}
