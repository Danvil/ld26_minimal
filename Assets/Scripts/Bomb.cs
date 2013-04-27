using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	
	public AudioClip audioBump;
	public AudioClip audioExplosion;
	
	public GameObject pfExplosion;
	
	float age;
	
	const float EXPLOSION_DELAY = 3.0f;
	const float AUDIO_BUMP_DELAY = 0.3f;

	float audioBumpDelay = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		age += MyTime.deltaTime;
		audioBumpDelay += MyTime.deltaTime;
		if(age > EXPLOSION_DELAY) {
			age = -10000.0f;
			// create explosion
			GameObject go_exp = (GameObject)Instantiate(this.pfExplosion);
			go_exp.transform.position = this.transform.position;
			// shedule particle effect for destruction
			Object.Destroy(go_exp, go_exp.particleSystem.duration);
			// destroy bomb
			Object.Destroy(this.gameObject, 3.0f);
			this.GetComponentInChildren<Renderer>().enabled = false;
			// notify explosion manager
			Globals.BombManager.ExplodeBomb(this);
			// play sound
			audio.PlayOneShot(audioExplosion);
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		if(audioBumpDelay > AUDIO_BUMP_DELAY) {
			audio.PlayOneShot(audioBump);
			audioBumpDelay = 0.0f;
		}
	}
}
