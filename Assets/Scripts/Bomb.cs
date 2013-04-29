using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	
	public AudioClip audioBump;
	public AudioClip[] audioExplosion;
	
	public GameObject pfExplosion;
	
	public bool IsPlayerBomb = false;
	
	public float Damage = 16.0f;
	public float Radius = 2.0f;

	float age;
	
	public float EXPLOSION_DELAY = 3.0f;
	const float AUDIO_BUMP_DELAY = 0.3f;

	float audioBumpDelay = 0.0f;
	
	public float ComputeDamage(float d)
	{
		if(d >= Radius) return 0.0f;
		d += 1.0f;
		float h1 = (1.0f + Radius);
		float W = h1*h1 / (h1*h1 + 1.0f);
		float f = W/(d*d) - W + 1.0f;
		return f * Damage;
	}

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
			audio.PlayOneShot(audioExplosion[Random.Range(0,audioExplosion.Length)]);
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		if(audioBumpDelay > AUDIO_BUMP_DELAY) {
			audio.PlayOneShot(audioBump);
			audioBumpDelay = 0.0f;
		}
	}
}
