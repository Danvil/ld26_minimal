using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	
	public GameObject pfExplosion;
	
	public float age;
	
	const float EXPLOSION_DELAY = 3.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		age += MyTime.deltaTime;
		if(age > EXPLOSION_DELAY) {
			// create explosion
			GameObject go_exp = (GameObject)Instantiate(pfExplosion);
			go_exp.transform.position = this.transform.position;
			Object.Destroy(go_exp, go_exp.particleSystem.duration);
			// destroy self
			Object.Destroy(this.gameObject);
		}
	}
}
